using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.IO;



namespace pyRuizdeOlano_IEFI
{
    internal class bdUsuarios
    {
        private OleDbConnection conexion;
        private OleDbCommand comando;


        private String cadena;
        public bdUsuarios()
        {
            cadena = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=usuarios.accdb;";
            conexion = new OleDbConnection(cadena);
            comando = new OleDbCommand();

        }
        public void Conectar() 
        {
            if (conexion.State != System.Data.ConnectionState.Open)
            {
                conexion.Open();
                comando.Connection = conexion;



            }
        }
        public void Desconectar()
        {
            if (conexion.State == System.Data.ConnectionState.Open)
            {
                conexion.Close();
            }
        }
        public bool UsuarioExiste(string nombreUsuario)
        {
            try
            {
                Conectar();
                comando.CommandText = "SELECT COUNT(*) FROM usuarios WHERE NombreUsuarios = @NombreUsuarios";
                comando.Parameters.Clear();
                comando.Parameters.AddWithValue("@NombreUsuarios", nombreUsuario);
                int count = (int)comando.ExecuteScalar();
                return count > 0;
            }
            catch (OleDbException ex)
            {
                MessageBox.Show($"Error al verificar el usuario: {ex.Message}", "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                Desconectar();
            }

        }
        public bool InsertarUsuario(string nombreUsuario, string contraseña)
        {
            try
            {
                Conectar();
                comando.CommandText = "INSERT INTO usuarios (NombreUsuarios, Contraseña) VALUES (@NombreUsuarios, @Contraseña)";
                comando.Parameters.Clear();
                comando.Parameters.AddWithValue("@NombreUsuarios", nombreUsuario);
                comando.Parameters.AddWithValue("@Contraseña", contraseña);
                int filas = comando.ExecuteNonQuery();
                return filas > 0;
            }
            catch (OleDbException ex)
            {
                MessageBox.Show($"Error al insertar el usuario: {ex.Message}", "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;



            }
            finally
            {
                Desconectar();
            }


        }
        public bool IniciarSesion(string nombreUsuario, string contraseña)
        {
            try
            {
                Conectar();
                comando.CommandText = "SELECT Contraseña FROM usuarios WHERE NombreUsuarios = @NombreUsuarios";
                comando.Parameters.Clear();
                comando.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
                object result = comando.ExecuteScalar();
                if (result != null)
                {
                    string contraseñaAlmacenada = result.ToString();
                    return contraseña == contraseñaAlmacenada;
                }
                else
                {
                    return false;
                }
            }
            catch (OleDbException ex)
            {
                MessageBox.Show($"Error al intentar iniciar sesión: {ex.Message}", "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                Desconectar(); 
            }
        }
        public int RegistrarInicioSesion(string nombreUsuario)
        {
            try
            {
                Conectar();

                comando.CommandText = "INSERT INTO usuarios (NombreUsuarios, FechaInicio, FechaFin, DuracionSegundos) VALUES (@NombreUsuario, @FechaInicio, @FechaFin, @DuracionSegundos)";
                comando.Parameters.Clear();
                comando.Parameters.AddWithValue("@NombreUsuarios", nombreUsuario);
                comando.Parameters.AddWithValue("@FechaInicio", DateTime.Now);
                comando.Parameters.AddWithValue("@FechaFin", DBNull.Value);
                comando.Parameters.AddWithValue("@DuracionSegundos", 0);
                return 1;


            }
            catch (OleDbException ex)
            {
                MessageBox.Show($"Error al registrar el inicio de sesión: {ex.Message}", "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            finally
            {
                Desconectar();
            }

        }
        public bool ActualizarFinSesion(int idSesion, DateTime fechaFin, int duracionSegundos)
        {
            try 
            {

                Conectar();
                comando.CommandText = "UPDATE Sesiones SET FechaFin = @FechaFin, DuracionSegundos = @DuracionSegundos WHERE IdSesion = @IdSesion";
                comando.Parameters.Clear();
                comando.Parameters.AddWithValue("@FechaFin", fechaFin);
                comando.Parameters.AddWithValue("@DuracionSegundos", duracionSegundos);
                comando.Parameters.AddWithValue("@IdSesion", idSesion);
                int filasAfectadas = comando.ExecuteNonQuery();
                return filasAfectadas > 0;
            }
            catch (OleDbException ex)
            {
                MessageBox.Show($"Error al actualizar el fin de sesión: {ex.Message}", "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;


            }
            finally
            {
                Desconectar();
            }
        }


    }
}
