using ParqueaderoBackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParqueaderoBackEnd.Interfaces
{
    public interface IDatos
    {
        /// <summary>
        /// Interfaz para insertar el usuario y la clave
        /// </summary>
        /// <param name="oUsuario"></param>
        public void InsertarUsario(ModParqueadero oUsuario);
        /// <summary>
        /// Verificar Usuario
        /// </summary>
        /// <param name="oUsuario"></param>
        public bool VerificarUsuario(ModParqueadero oUsuario);
        /// <summary>
        /// ingresa los transportes
        /// </summary>
        /// <param name="oTransporte"></param>
        public void IngresarTransporte(ModTransporte oTransporte);
        /// <summary>
        /// elimina los transportes
        /// </summary>
        /// <param name="Placa"></param>
        public void EliminarTransporte(string Placa);
        /// <summary>
        /// muestra los datos del parqueadeadero
        /// </summary>
        /// <returns></returns>
        public List<ModTransporte> DatosParqueaderoS();
        /// <summary>
        /// actualza los datos del parqueadero
        /// </summary>
        /// <param name="oTransporte"></param>
        public void UpdateParqueadero(ModTransporte oTransporte);

    }
}
