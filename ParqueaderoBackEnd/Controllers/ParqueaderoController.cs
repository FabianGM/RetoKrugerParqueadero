using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParqueaderoBackEnd.Interfaces;
using ParqueaderoBackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParqueaderoBackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ParqueaderoController : ControllerBase
    {
        //private readonly ILogger<ParqueaderoController> _logger;
        private readonly IDatos _Service;
        /// <summary>
        /// Constructor para Inyección de dependencias
        /// </summary>
        /// <param name="oUsuario"></param>
        public ParqueaderoController(IDatos oUsuario)
        {
            _Service = oUsuario;
        }

        /// <summary>
        /// Registra usuario y clave
        /// Verbo Post: /parqueadero/Register
        /// </summary>
        /// <param name="oUser"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public IActionResult Register([FromBody] ModParqueadero oUser)
        {
            _Service.InsertarUsario(oUser);
            return Ok(oUser);
        }

        // <summary>
        /// envia los datos del login
        /// Verbo Post: /parqueadero/Login
        /// </summary>
        /// <param name="oUser"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public IActionResult DatosUser([FromBody] ModParqueadero oUser)
        {
            bool VerificarUser=_Service.VerificarUsuario(oUser);
            if (VerificarUser)
            {
                return Ok(oUser);
            }
            else
            {
                oUser.Usuario = "no";
                oUser.Clave = "no";
                return Ok(oUser);
            }
            
        }
        /// <summary>
        /// ingresa el transporte
        /// verbo: post
        /// </summary>
        /// <param name="oTransporte"></param>
        /// <returns></returns>
        [HttpPost("IngresarTransporte")]
        public IActionResult IngresarTransporte([FromBody] ModTransporte oTransporte)
        {
            string DatetimeHora = DateTime.Now.ToString("HH");
            string DatetimeMinutos = DateTime.Now.ToString("mm");
            oTransporte.Hora = DatetimeHora;
            oTransporte.Minutos = DatetimeMinutos;
            _Service.IngresarTransporte(oTransporte);
            return Ok(oTransporte);
        }

        /// <summary>
        /// elimina el transporte
        /// verbo: delete
        /// </summary>
        /// <param name="Placa"></param>
        /// <returns></returns>
        [HttpDelete("EliminarTransporte/{Placa}")]
        public IActionResult EliminarTransporte(string Placa)
        {
            ModTransporte oTransporte = new ModTransporte();
            _Service.EliminarTransporte(Placa);
            return Ok(oTransporte);
        }

        /// <summary>
        /// muestra los datos del parqueadero
        /// verbo: get
        /// </summary>
        /// <returns></returns>
        [HttpGet("DatosParqueaderoS")]
        public IActionResult DatosParqueaderoS()
        {
            return Ok(_Service.DatosParqueaderoS());
        }

        /// <summary>
        /// actualiza los datos del transporte
        /// verbo: put
        /// </summary>
        /// <param name="oTransporte"></param>
        /// <returns></returns>
        [HttpPut("UpdateParqueadero")]
        public IActionResult UpdateParqueadero([FromBody] ModTransporte oTransporte)
        {
            _Service.UpdateParqueadero(oTransporte);
            return Ok(oTransporte);
        }
    }
}
