using BackEnd.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
namespace BackEnd.Controllers
{
    [Route("Parqueadero/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static modUser user = new modUser();
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor para la Inyección de Dependecias
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="userService"></param>
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

       
        /// <summary>
        /// Regsitra al usuario y encripta las claves
        /// </summary>
        /// <param name="request"></param>
        /// <returns>las claves encriptadas</returns>
        [HttpPost("Register")]
        public async Task<ActionResult<modUser>> Register(modUserDto request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            return Ok(user);
        }

        /// <summary>
        /// Validar usuario y clave
        /// </summary>
        /// <param name="request"></param>
        /// <returns>token generada por el usuario</returns>
        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(modUserDto request)
        {
            if (user.Username != request.Username)
            {
                return BadRequest("Usuario no encontrado");
            }

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Clave Inconrrecta");
            }

            string token = CreateToken(user);

            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken);

            return Ok(token);
        }

        /// <summary>
        /// Refresca el token del usuario
        /// </summary>
        /// <returns>Token del usuario</returns>
        [HttpPost("Refresh-token")]
        public async Task<ActionResult<string>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (!user.RefreshToken.Equals(refreshToken))
            {
                return Unauthorized("No se puede actualizar token");
            }
            else if (user.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Su Token ha expirado");
            }

            string token = CreateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken);

            return Ok(token);
        }

        /// <summary>
        /// Token valida hasta 7 dias
        /// </summary>
        /// <returns>Nueva Token</returns>
        private modRefreshToken GenerateRefreshToken()
        {
            var refreshToken = new modRefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };

            return refreshToken;
        }

        /// <summary>
        /// Setea la nueva token y su fecha de caducidad
        /// </summary>
        /// <param name="newRefreshToken"></param>
        private void SetRefreshToken(modRefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

            user.RefreshToken = newRefreshToken.Token;
            user.TokenCreated = newRefreshToken.Created;
            user.TokenExpires = newRefreshToken.Expires;
        }

        /// <summary>
        /// Crea el token del usuario
        /// </summary>
        /// <param name="user"></param>
        /// <returns>JSON WEB TOKEN</returns>
        private string CreateToken(modUser user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        /// <summary>
        /// Encripta la clave del usuario
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        /// <summary>
        /// verifica la clave ecriptada del usuario
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        /// <returns></returns>
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
