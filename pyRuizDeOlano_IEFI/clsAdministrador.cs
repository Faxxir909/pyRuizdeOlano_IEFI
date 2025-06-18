using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static pyRuizdeOlano_IEFI.clsAdministrador;

namespace pyRuizdeOlano_IEFI
{
    internal class clsAdministrador
    {
        private readonly string cadenaConexion = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={AppDomain.CurrentDomain.BaseDirectory}Productos1.accdb;";
        private OleDbConnection conexion;
        public clsAdministrador()
        {
            conexion = new OleDbConnection(cadenaConexion);
        }
        public OleDbConnection ObtenerConexion()
        {
            return conexion;
        }
        public void Abrir()
        {
            if (conexion.State == System.Data.ConnectionState.Closed)
            {
                conexion.Open();
            }
        }
        public void Cerrar()
        {
            if (conexion.State == System.Data.ConnectionState.Open)
            {
                conexion.Close();
            }


        }
    }
}
