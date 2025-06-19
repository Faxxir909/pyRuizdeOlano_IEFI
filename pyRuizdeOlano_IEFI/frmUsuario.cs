using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pyRuizdeOlano_IEFI
{
    public partial class frmUsuario : Form
    {
        OleDbConnection conectar = new OleDbConnection();
        public frmUsuario()
        {
            conectar.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:Productos1.accdb;";
            InitializeComponent();
        }

        private void btnListar_Click(object sender, EventArgs e)
        {
            conectar.Open();
            OleDbCommand comando = new OleDbCommand();
            comando.Connection = conectar;
            comando.CommandText = "SELECT * FROM Productos";
            OleDbDataReader Leer = comando.ExecuteReader();
            while (Leer.Read())
            {
                int Grilla = dgvGrilla_Usuario.Rows.Add();
                dgvGrilla_Usuario.Rows[Grilla].Cells[0].Value = Leer["Codigo"].ToString();
                dgvGrilla_Usuario.Rows[Grilla].Cells[1].Value = Leer["Nombre"].ToString();
                dgvGrilla_Usuario.Rows[Grilla].Cells[2].Value = Leer["Descripcion"].ToString();
                dgvGrilla_Usuario.Rows[Grilla].Cells[3].Value = Leer["Precio"].ToString();
                dgvGrilla_Usuario.Rows[Grilla].Cells[4].Value = Leer["Stock"].ToString();
                dgvGrilla_Usuario.Rows[Grilla].Cells[5].Value = Leer["Categoria"].ToString();
                dgvGrilla_Usuario.ReadOnly = true;
            }

        }

        private void frmUsuario_Load(object sender, EventArgs e)
        {
            
        }
    }
}
