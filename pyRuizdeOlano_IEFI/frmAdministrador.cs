using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using pyRuizdeOlano_IEFI;

namespace pryRuiz_de_Olano
{
    public partial class frmAdministrador : Form
    {

        public frmAdministrador()
        {

            InitializeComponent();
        }




        clsAdministrador bd = new clsAdministrador();

        private void frmAdministrador_Load(object sender, EventArgs e)
        {
            CargarProductos();
        }
        private void CargarProductos()
        {
            try
            {
                dgvTabla.Rows.Clear();
                bd.Abrir();
                var comando = new OleDbCommand("SELECT * FROM Productos", bd.ObtenerConexion());
                using (var lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        int fila = dgvTabla.Rows.Add();
                        dgvTabla.Rows[fila].Cells[0].Value = lector["Codigo"];
                        dgvTabla.Rows[fila].Cells[1].Value = lector["Nombre"];
                        dgvTabla.Rows[fila].Cells[2].Value = lector["Descripcion"];
                        dgvTabla.Rows[fila].Cells[3].Value = lector["Precio"];
                        dgvTabla.Rows[fila].Cells[4].Value = lector["Stock"];
                        dgvTabla.Rows[fila].Cells[5].Value = lector["Categoria"];
                    }
                    dgvTabla.ReadOnly = true;
                }







            }

            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar productos: " + ex.Message);
            }
            finally
            {
                bd.Cerrar();
            }
        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos(out string nombre, out string descripcion, out decimal precio, out int stock, out string categoria))
                return;
            {
                try
                {
                    bd.Abrir();
                    var consulta = "INSERT INTO Productos (Nombre, Descripcion, Precio, Stock, Categoria) VALUES (?, ?, ?, ?, ?)";
                    var cmd = new OleDbCommand(consulta, bd.ObtenerConexion());
                    cmd.Parameters.AddWithValue("?", nombre);
                    cmd.Parameters.AddWithValue("?", descripcion);
                    cmd.Parameters.AddWithValue("?", precio);
                    cmd.Parameters.AddWithValue("?", stock);
                    cmd.Parameters.AddWithValue("?", categoria);
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Producto guardado correctamente.");
                        LimpiarCampos();
                        CargarProductos();
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Error al guardar: " + ex.Message);
                }
                finally
                {
                    bd.Cerrar();
                }
            }
        }
        private bool ValidarCampos(out string nombre, out string descripcion, out decimal precio, out int stock, out string categoria)
        {
            nombre = txtProductos.Text.Trim();
            descripcion = txtDescripcion.Text.Trim();
            categoria = cmbCategoria.Text.Trim();
            bool precioOk = decimal.TryParse(txtPrecio.Text.Trim(), out precio);
            bool stockOk = int.TryParse(txtStock.Text.Trim(), out stock);
            if (string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(descripcion) || string.IsNullOrWhiteSpace(categoria))
            {
                MessageBox.Show("Complete todos los campos.");
                return false;
            }
            if (!precioOk)
            {
                MessageBox.Show("Precio inválido.");
                return false;
            }
            if (!stockOk)
            {
                MessageBox.Show("Stock inválido.");
                return false;
            }
            return true;
        }
        private void LimpiarCampos()
        {
            txtProductos.Clear();
            txtDescripcion.Clear();
            txtPrecio.Clear();
            txtStock.Clear();
            cmbCategoria.SelectedIndex = -1;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvTabla.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un producto para eliminar.");
                return;
            }
            var fila = dgvTabla.SelectedRows[0];
            string codigo = fila.Cells[0].Value?.ToString();
            if (string.IsNullOrEmpty(codigo))
            {
                MessageBox.Show("Código no válido.");
                return;
            }
            if (MessageBox.Show($"¿Eliminar producto con código {codigo}?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    bd.Abrir();
                    var cmd = new OleDbCommand("DELETE FROM Productos WHERE Codigo = ?", bd.ObtenerConexion());
                    cmd.Parameters.AddWithValue("?", codigo);

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("Producto eliminado.");
                        CargarProductos();
                    }
                    else
                    {
                        MessageBox.Show("No se encontró el producto.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al eliminar: " + ex.Message);
                }
                finally
                {
                    bd.Cerrar();
                }
            }
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvTabla.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un producto para modificar.");
                return;
            }
            var fila = dgvTabla.SelectedRows[0];
            string codigo = fila.Cells[0].Value?.ToString();
            if (string.IsNullOrEmpty(codigo))
            {
                MessageBox.Show("Código no válido.");
                return;
            }
            if (!ValidarCampos(out string nombre, out string descripcion, out decimal precio, out int stock, out string categoria))
                return;
            try
            {
                bd.Abrir();
                var cmd = new OleDbCommand("UPDATE Productos SET Nombre=?, Descripcion=?, Precio=?, Stock=?, Categoria=? WHERE Codigo=?", bd.ObtenerConexion());
                cmd.Parameters.AddWithValue("?", nombre);
                cmd.Parameters.AddWithValue("?", descripcion);
                cmd.Parameters.AddWithValue("?", precio);
                cmd.Parameters.AddWithValue("?", stock);
                cmd.Parameters.AddWithValue("?", categoria);
                cmd.Parameters.AddWithValue("?", codigo);

                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Producto modificado.");
                    CargarProductos();
                }
                else
                {
                    MessageBox.Show("No se pudo modificar el producto.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar: " + ex.Message);
            }
            finally
            {

            }
        }
        private void frmAdministrador_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
