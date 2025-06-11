using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using pryRuiz_de_Olano;

namespace pyRuizdeOlano_IEFI
{
    public partial class frmAuditoria : Form
    {
        public frmAuditoria()
        {

        }
        private string nombreUsuarioLogueado;
        private int Tiempo = 0;
        private int sesionActual = -1;
        private void frmAuditoria_Load(object sender, EventArgs e)
        {
            rbAdministrador.Visible = true;
            rbUsuario.Visible = true;
            btnSiguiente.Enabled = false;
            rbAdministrador.Checked = false;
            rbUsuario.Checked = false;
            bdUsuarios usuarios = new bdUsuarios();
            sesionActual = usuarios.RegistrarInicioSesion(nombreUsuarioLogueado);
            if (sesionActual != -1)
            {
                timer.Start();
                lblTiempo.Text = "00:00:00";
            }
            else
            {
                MessageBox.Show("No se pudo iniciar el registro de sesión. El tiempo no se guardará.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            lblFecha.Text = "Fecha" + DateTime.Now.ToShortDateString();
        }
        public frmAuditoria(string usuario)
        {
            InitializeComponent();
            this.nombreUsuarioLogueado = usuario;
            lblNombreUsuario.Text = "¡Bienvenido, " + this.nombreUsuarioLogueado + "!";
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            Tiempo++;
            TimeSpan tiempo = TimeSpan.FromSeconds(Tiempo);
            lblTiempo.Text = tiempo.ToString(@"hh\:mm\:ss");
        }
        private void frmAuditoria_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer.Stop();
            if (sesionActual != -1)
            {
                bdUsuarios usuarios = new bdUsuarios();
                DateTime fechaFin = DateTime.Now;
                bool guardadoExitoso = usuarios.ActualizarFinSesion(sesionActual, fechaFin, Tiempo);
                if (!guardadoExitoso)
                {
                    MessageBox.Show("Hubo un error al guardar el tiempo de sesión.", "Error de Guardado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            this.Hide();
            if (rbAdministrador.Checked)
            {
                frmStock Administrador = new frmStock();
                Administrador.Show();
            }
            else if (rbUsuario.Checked)
            {
                frmUsuario usuarios = new frmUsuario();
                usuarios.Show();
            }
        }

        private void rbAdministrador_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAdministrador.Checked)
            {

                btnSiguiente.Enabled = true;
               

            }
            else
            {
                btnSiguiente.Enabled = false;
                rbUsuario.Enabled = false;
            }
        }

        private void rbUsuario_CheckedChanged(object sender, EventArgs e)
        {
            if (rbUsuario.Checked)
            {
                btnSiguiente.Enabled = true;
            }
            else
            {
                btnSiguiente.Enabled = false;
                rbAdministrador.Enabled = false;
            }

        }
    }
}
