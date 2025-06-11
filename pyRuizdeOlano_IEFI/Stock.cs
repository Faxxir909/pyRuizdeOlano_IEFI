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

namespace pryRuiz_de_Olano
{
    public partial class frmStock : Form
    {
        OleDbConnection conectar = new OleDbConnection();
        public frmStock()
        {
            conectar.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:Productos1.accdb;";
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conectar.Open();
            OleDbCommand comando = new OleDbCommand();
            comando.Connection = conectar;
            comando.CommandText = "SELECT * FROM Productos";
            OleDbDataReader Leer = comando.ExecuteReader();
            while (Leer.Read())
            {
                int Grilla = dgvTabla.Rows.Add();
                dgvTabla.Rows[Grilla].Cells[0].Value = Leer["Codigo"].ToString();
                dgvTabla.Rows[Grilla].Cells[1].Value = Leer["Nombre"].ToString();
                dgvTabla.Rows[Grilla].Cells[2].Value = Leer["Descripcion"].ToString();
                dgvTabla.Rows[Grilla].Cells[3].Value = Leer["Precio"].ToString();
                dgvTabla.Rows[Grilla].Cells[4].Value = Leer["Stock"].ToString();
                dgvTabla.Rows[Grilla].Cells[5].Value = Leer["Categoria"].ToString();
                dgvTabla.ReadOnly=true;
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvTabla.SelectedRows.Count > 0)
            {
                DataGridViewRow filaSeleccionada = dgvTabla.SelectedRows[0];
                if (filaSeleccionada.Cells[0].Value != null)
                {
                    string idAEliminar = filaSeleccionada.Cells[0].Value.ToString();
                    if (MessageBox.Show($"¿Seguro que desea eliminar el registro con Código: {idAEliminar}?", "Confirmar Eliminación", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {

                    }
                    try
                    {
                        
                        string consultaEliminar = "DELETE FROM Productos WHERE Codigo = @Codigo";
                        OleDbCommand comandoEliminar = new OleDbCommand(consultaEliminar, conectar);
                        comandoEliminar.Parameters.AddWithValue("@Codigo", idAEliminar);
                        int filasAfectadas = comandoEliminar.ExecuteNonQuery();
                        if (filasAfectadas > 0)
                        {
                            MessageBox.Show("Registro eliminado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);


                            dgvTabla.Rows.Remove(filaSeleccionada);


                        }
                        else
                        {
                            MessageBox.Show("No se pudo eliminar el registro.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex) 
                    {
                        MessageBox.Show($"Error al eliminar el registro: {ex.Message}", "Error de base de datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        if (conectar.State == ConnectionState.Open)
                        {
                            conectar.Close();
                        }
                    }
                    
                }
                else
                {
                    MessageBox.Show("No se puede determinar el Código del registro a eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una fila para eliminar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    
    

        private void btnCargar_Click(object sender, EventArgs e)
        {

            string nuevoProducto = txtProductos.Text;
            string nuevaDescripcion = txtDescripcion.Text;
            string nuevoPrecioTexto = txtPrecio.Text;
            string nuevoStockTexto = txtStock.Text;
            string nuevaCategoria = cmbCategoria.Text;

            if (string.IsNullOrWhiteSpace(nuevoProducto) || string.IsNullOrWhiteSpace(nuevaDescripcion) ||
            string.IsNullOrWhiteSpace(nuevoPrecioTexto) || string.IsNullOrWhiteSpace(nuevoStockTexto) ||
            string.IsNullOrWhiteSpace(nuevaCategoria))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!decimal.TryParse(nuevoPrecioTexto, out decimal nuevoPrecio))
            {
                MessageBox.Show("El precio debe ser un número válido.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!int.TryParse(nuevoStockTexto, out int nuevoStock))
            {
                MessageBox.Show("El stock debe ser un número entero válido.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                
                string consultaInsertar = "INSERT INTO Productos (Nombre, Descripcion, Precio, Stock, Categoria) VALUES (@Nombre, @Descripcion, @Precio, @Stock, @Categoria)";
                OleDbCommand comandoInsertar = new OleDbCommand(consultaInsertar, conectar);
                comandoInsertar.Parameters.AddWithValue("@Nombre", nuevoProducto);
                comandoInsertar.Parameters.AddWithValue("@Descripcion", nuevaDescripcion);
                comandoInsertar.Parameters.AddWithValue("@Precio", nuevoPrecio);
                comandoInsertar.Parameters.AddWithValue("@Stock", nuevoStock);
                comandoInsertar.Parameters.AddWithValue("@Categoria", nuevaCategoria);
                int filasAfectadas = comandoInsertar.ExecuteNonQuery();
                if (filasAfectadas > 0)
                {
                    MessageBox.Show("Producto guardado en la base de datos.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                     int i = dgvTabla.Rows.Add();
                     dgvTabla.Rows[i].Cells[1].Value = nuevoProducto; 
                     dgvTabla.Rows[i].Cells[2].Value = nuevaDescripcion; 
                     dgvTabla.Rows[i].Cells[3].Value = nuevoPrecio;    
                     dgvTabla.Rows[i].Cells[5].Value = nuevaCategoria;
                        txtProductos.Clear();
                     txtDescripcion.Clear();
                       txtPrecio.Clear();
                    txtStock.Clear();
                    cmbCategoria.SelectedIndex = -1;
                    conectar.Close();
                    Form1_Load(this, EventArgs.Empty);
                }
                else
                {
                    MessageBox.Show("No se pudo guardar el producto en la base de datos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (OleDbException ex)
            {
                MessageBox.Show("Error al guardar el producto: " + ex.Message, "Error de base de datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (conectar.State == ConnectionState.Open)
                {
                    conectar.Close();
                }
            }









        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvTabla.SelectedRows.Count > 0)
            {
                DataGridViewRow filaSeleccionada = dgvTabla.SelectedRows[0];
                if (filaSeleccionada.Cells[0].Value != null)
                {
                    string codigoAActualizar = filaSeleccionada.Cells[0].Value.ToString();
                    string nuevoProducto = txtProductos.Text;
                    string nuevaDescripcion = txtDescripcion.Text;
                    string nuevoPrecioTexto = txtPrecio.Text;
                    string nuevoStockTexto = txtStock.Text;
                    string nuevaCategoria = cmbCategoria.Text;
                    if (string.IsNullOrWhiteSpace(nuevoProducto) || string.IsNullOrWhiteSpace(nuevaDescripcion) ||
                        string.IsNullOrWhiteSpace(nuevoPrecioTexto) || string.IsNullOrWhiteSpace(nuevoStockTexto) ||
                        string.IsNullOrWhiteSpace(nuevaCategoria))
                    {
                        MessageBox.Show("Por favor, complete todos los campos.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (!decimal.TryParse(nuevoPrecioTexto, out decimal nuevoPrecio))
                    {
                        MessageBox.Show("El precio debe ser un número válido.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (!int.TryParse(nuevoStockTexto, out int nuevoStock))
                    {
                        MessageBox.Show("El stock debe ser un número entero válido.", "Error de validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    try
                    {
                        string consultaActualizar = "UPDATE Productos SET Nombre = @Nombre, Descripcion = @Descripcion, Precio = @Precio, Stock = @Stock, Categoria = @Categoria WHERE Codigo = @Codigo";
                        OleDbCommand comandoActualizar = new OleDbCommand(consultaActualizar, conectar);
                        comandoActualizar.Parameters.AddWithValue("@Nombre", nuevoProducto);
                        comandoActualizar.Parameters.AddWithValue("@Descripcion", nuevaDescripcion);
                        comandoActualizar.Parameters.AddWithValue("@Precio", nuevoPrecio);
                        comandoActualizar.Parameters.AddWithValue("@Stock", nuevoStock);
                        comandoActualizar.Parameters.AddWithValue("@Categoria", nuevaCategoria);
                        comandoActualizar.Parameters.AddWithValue("@Codigo", codigoAActualizar);
                        int filasAfectadas = comandoActualizar.ExecuteNonQuery();
                        if (filasAfectadas > 0)
                        {
                            filaSeleccionada.Cells[1].Value = nuevoProducto;
                            filaSeleccionada.Cells[2].Value = nuevaDescripcion;
                            filaSeleccionada.Cells[3].Value = nuevoPrecio;
                            filaSeleccionada.Cells[4].Value = nuevoStock;
                            filaSeleccionada.Cells[5].Value = nuevaCategoria;
                        }
                        else
                        {
                            MessageBox.Show("No se pudo modificar el producto.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("Error al modificar el producto: " + ex.Message, "Error de base de datos", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    finally
                    {
                        if (conectar.State == ConnectionState.Open)
                        {
                            conectar.Close();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No se puede determinar el código del producto a actualizar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
                
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una fila para modificar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
