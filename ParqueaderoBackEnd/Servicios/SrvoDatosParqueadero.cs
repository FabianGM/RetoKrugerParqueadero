using ParqueaderoBackEnd.Interfaces;
using ParqueaderoBackEnd.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace ParqueaderoBackEnd.Servicios
{
    /// <summary>
    /// Servicios para la conexión con la base de datos
    /// </summary>
    public class SrvoDatosParqueadero: IDatos
    {
        const string cadenaConexion = "Data Source=ParqueaderoDatos.db;Version=3;New=True;Compress=True;";
        private SQLiteCommand sql_cmd;
        private string txtQuery;
        SQLiteConnection sql_con = new SQLiteConnection(cadenaConexion);
        /// <summary>
        /// Insertar un usuario en la base de datos
        /// </summary>
        /// <param name="oUsuario"></param>
        public void InsertarUsario(ModParqueadero oUsuario)
        {
            try
            {
                sql_con.Open();
                sql_cmd = sql_con.CreateCommand();
                txtQuery = "insert into Usuario (USUARIO, CLAVE) values ('" + oUsuario.Usuario + "','" + oUsuario.Clave + "');";
                sql_cmd.CommandText = txtQuery;
                sql_cmd.ExecuteNonQuery();
                sql_con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        /// <summary>
        /// verificar un usuario existente en la base de datos
        /// </summary>
        /// <param name="oUsuario"></param>
        /// <returns></returns>
        public bool VerificarUsuario(ModParqueadero oUsuario)
        {
            try
            {
                sql_con.Open();
                string stm = "SELECT * FROM Usuario";
                using var cmd = new SQLiteCommand(stm, sql_con);
                using SQLiteDataReader rdr = cmd.ExecuteReader();
                
                while (rdr.Read())
                {
                    if (oUsuario.Usuario == rdr.GetString(1) && oUsuario.Clave == rdr.GetString(2))
                    {
                        Console.WriteLine("acceso autorizado");
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return false;
        }

        /// <summary>
        /// Ingresa los transportes del parqueadero
        /// </summary>
        /// <param name="oTransporte"></param>
        public void IngresarTransporte(ModTransporte oTransporte)
        {
            try
            {
                sql_con.Open();
                sql_cmd = sql_con.CreateCommand();
                txtQuery = "insert into Transportes (PLACA, TRANSPORTE, ESTADO, HORA, MINUTOS) values ('" + oTransporte.Placa + "','" + oTransporte.Transporte + "','"+ oTransporte.Estado + "','" + oTransporte.Hora+ "','" + oTransporte.Minutos+"');";
                sql_cmd.CommandText = txtQuery;
                sql_cmd.ExecuteNonQuery();
                sql_con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
        /// <summary>
        /// Eliminas los transportes del parqueadero
        /// </summary>
        /// <param name="Placa"></param>
        public void EliminarTransporte(string Placa)
        {
            try
            {
                sql_con.Open();
                sql_cmd = sql_con.CreateCommand();
                txtQuery = "delete from Transportes where PLACA ='" + Placa + "'";
                sql_cmd.CommandText = txtQuery;
                sql_cmd.ExecuteNonQuery();
                sql_con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
        /// <summary>
        /// muestra los datos del parqueadero
        /// </summary>
        /// <returns></returns>
        public List<ModTransporte> DatosParqueaderoS()
        {
            List<ModTransporte> oDatosSO = new List<ModTransporte>();
            ModTransporte oDatosMod;
            sql_con.Open();
            string stm = "SELECT * FROM Transportes";
            using var cmd = new SQLiteCommand(stm, sql_con);
            using SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                oDatosMod = new ModTransporte();
                oDatosMod.Placa = rdr.GetString(1);
                oDatosMod.Transporte = rdr.GetString(2);
                oDatosMod.Estado = rdr.GetString(3);
                oDatosMod.Hora = rdr.GetString(4);
                oDatosMod.Minutos = rdr.GetString(5);
                oDatosSO.Add(oDatosMod);
            }
          
            return oDatosSO;
        }
        /// <summary>
        /// actuliza los datos de los transportes 
        /// </summary>
        /// <param name="oTransporte"></param>
        public void UpdateParqueadero(ModTransporte oTransporte)
        {
            try
            {
                sql_con.Open();
                sql_cmd = sql_con.CreateCommand();
                txtQuery = "update Transportes set ESTADO ='" + oTransporte.Estado + "' WHERE" + " PLACA ='" + oTransporte.Placa + "'";
                sql_cmd.CommandText = txtQuery;
                sql_cmd.ExecuteNonQuery();
                sql_con.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }


    }
}
