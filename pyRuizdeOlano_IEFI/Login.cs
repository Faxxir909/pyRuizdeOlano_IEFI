using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace pyRuizdeOlano_IEFI
{
    public partial class frmLogin : Form
    {
        
        public frmLogin()
        {
            InitializeComponent();
            
        }
        bdUsuarios usuarios = new bdUsuarios();


        private void frmLogin_Load(object sender, EventArgs e)
        {

        }

        private void btnRegistro_Click(object sender, EventArgs e)
        {

            string usuario = txtUser.Text;
            string contra = txtPass.Text;
            lblTitulo.Text = "INICIAR SESION";
            btnEntrar.Visible = true;
            btnRegistro.Visible = false;
            txtUser.Text = "";
            txtPass.Text = "";
            txtPass.Visible = true;
            


            try
            {
                if (usuarios.UsuarioExiste(usuario))
                {
                    MessageBox.Show("El nombre de usuario ya está en uso. Por favor, elija otro.", "Usuario Existente", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtUser.Focus();
                }
                else
                {
                    if (usuarios.InsertarUsuario(usuario, contra))
                    {
                        MessageBox.Show("Usuario registrado exitosamente. Ahora puede iniciar sesión.", "Registro Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("No se pudo registrar el usuario en la base de datos. Intente de nuevo.", "Error de Registro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Ocurrió un error inesperado durante el registro: {ex.Message}", "Error General", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }




        }

        private void lblRegi_Click(object sender, EventArgs e)
        {
            lblTitulo.Text = "REGISTRARSE";
            btnEntrar.Visible = false;
            btnRegistro.Visible = true;
            txtUser.Text = "";
            txtPass.Text = "";
            lblRegi.Visible = false;
           
            txtUser.Focus();
        }

        private void chkMostrar_CheckedChanged(object sender, EventArgs e)
        {
            if (chkMostrar.Checked)
            {
                if (txtPass.PasswordChar == '●')
                {
                    txtPass.PasswordChar = '\0';
                }
            }
            else
            {
                txtPass.PasswordChar = '●';
            }
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            string usuario = txtUser.Text.Trim(); // .Trim() para eliminar espacios en blanco
            string contra = txtPass.Text.Trim();
            if (string.IsNullOrWhiteSpace(usuario) || string.IsNullOrWhiteSpace(contra))
            {
                MessageBox.Show("Por favor, ingrese un nombre de usuario y una contraseña.", "Campos Vacíos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                if (usuarios.IniciarSesion(usuario, contra))
                {
                    MessageBox.Show("Inicio de sesión exitoso!", "Bienvenido", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmAuditoria Auditoria = new frmAuditoria(usuario);
                    Auditoria.ShowDialog();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.", "Error de Inicio de Sesión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPass.Clear(); // Limpia la contraseña para que el usuario la reingrese
                    txtUser.Focus(); // O enfoca el campo de usuario para que lo reingrese si lo desea



                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error inesperado durante el inicio de sesión: {ex.Message}", "Error General", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }




        }
    }
}
