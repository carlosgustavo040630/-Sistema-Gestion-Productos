
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace SistemaGestionProductos
{
    public partial class Form1 : Form
    {
        private ComboBox cmbOrdenar;
        private Button btnOrdenar;
        private DataView vistaProductos;
        private ProductosDAL productosDAL = new ProductosDAL();
        private TextBox txtCodigo;
        private TextBox txtNombre;
        private TextBox txtDescripcion;
        private TextBox txtPrecio;
        private TextBox txtStock;
        private Button btnLimpiarBusqueda;

        private DataGridView dgvProductos;
        private TextBox txtBuscar;
        private Button btnBuscar;

        private Button btnGuardar;
        private Button btnModificar;
        private Button btnEliminar;
        private Button btnLimpiar;

        public Form1()
        {
            InitializeComponent();
            ConstruirInterfaz();
        }

        private void ConstruirInterfaz()
        {
            this.Controls.Clear();

            this.Text = "Sistema de Gestión de Productos";
            this.Size = new Size(1050, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(245, 247, 250);

            // Encabezado
            Label lblTitulo = new Label();
            lblTitulo.Text = "GESTIÓN DE PRODUCTOS";
            lblTitulo.Font = new Font("Segoe UI", 20, FontStyle.Bold);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.BackColor = Color.FromArgb(35, 47, 62);
            lblTitulo.Location = new Point(0, 0);
            lblTitulo.Size = new Size(1050, 75);
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;

            this.Controls.Add(lblTitulo);

            // Panel de información
            GroupBox grupoDatos = new GroupBox();
            grupoDatos.Text = "Información del producto";
            grupoDatos.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            grupoDatos.Location = new Point(30, 100);
            grupoDatos.Size = new Size(970, 220);

            this.Controls.Add(grupoDatos);

            // Campos
            CrearEtiqueta(grupoDatos, "Código:", 25, 40);
            txtCodigo = CrearCampo(grupoDatos, 110, 35, 250);

            CrearEtiqueta(grupoDatos, "Nombre:", 25, 90);
            txtNombre = CrearCampo(grupoDatos, 110, 85, 250);

            CrearEtiqueta(grupoDatos, "Descripción:", 480, 40);
            txtDescripcion = CrearCampo(grupoDatos, 590, 35, 320);

            CrearEtiqueta(grupoDatos, "Precio:", 480, 90);
            txtPrecio = CrearCampo(grupoDatos, 590, 85, 140);

            CrearEtiqueta(grupoDatos, "Stock:", 750, 90);
            txtStock = CrearCampo(grupoDatos, 810, 85, 100);

            // Botones
            btnGuardar = CrearBoton("Guardar", 110, 155, Color.FromArgb(39, 174, 96));
            btnModificar = CrearBoton("Modificar", 270, 155, Color.FromArgb(41, 128, 185));
            btnEliminar = CrearBoton("Eliminar", 430, 155, Color.FromArgb(192, 57, 43));
            btnLimpiar = CrearBoton("Limpiar", 590, 155, Color.FromArgb(127, 140, 141));

            grupoDatos.Controls.Add(btnGuardar);
            grupoDatos.Controls.Add(btnModificar);
            grupoDatos.Controls.Add(btnEliminar);
            grupoDatos.Controls.Add(btnLimpiar);
            btnGuardar.Click += BtnGuardar_Click;

            // Tabla de productos
            Label lblListado = new Label();
            // Buscador de productos
            txtBuscar = new TextBox();
            txtBuscar.Location = new Point(30, 340);
            txtBuscar.Size = new Size(250, 28);
            txtBuscar.Font = new Font("Segoe UI", 10);

            btnBuscar = CrearBoton("Buscar", 300, 337, Color.FromArgb(41, 128, 185));
            btnBuscar.Click += BtnBuscar_Click;

            this.Controls.Add(txtBuscar);
            this.Controls.Add(btnBuscar);
            // Ordenamiento de productos
            cmbOrdenar = new ComboBox();
            cmbOrdenar.Location = new Point(420, 340);
            cmbOrdenar.Size = new Size(180, 28);
            cmbOrdenar.Font = new Font("Segoe UI", 10);
            cmbOrdenar.DropDownStyle = ComboBoxStyle.DropDownList;

            cmbOrdenar.Items.Add("Nombre A-Z");
            cmbOrdenar.Items.Add("Nombre Z-A");
            cmbOrdenar.Items.Add("Precio menor a mayor");
            cmbOrdenar.Items.Add("Precio mayor a menor");

            cmbOrdenar.SelectedIndex = 0;

            btnOrdenar = CrearBoton("Ordenar", 620, 337, Color.FromArgb(39, 174, 96));
            btnOrdenar.Click += BtnOrdenar_Click;

            this.Controls.Add(cmbOrdenar);
            this.Controls.Add(btnOrdenar);
            // Botón para limpiar búsqueda
            btnLimpiarBusqueda = CrearBoton("Limpiar búsqueda", 790, 337, Color.FromArgb(127, 140, 141));
            btnLimpiarBusqueda.Click += BtnLimpiarBusqueda_Click;

            this.Controls.Add(btnLimpiarBusqueda);
            lblListado.Text = "Listado de productos";
            lblListado.Font = new Font("Segoe UI", 13, FontStyle.Bold);
            lblListado.Location = new Point(30, 380);
            lblListado.Size = new Size(300, 30);

            this.Controls.Add(lblListado);

            dgvProductos = new DataGridView();
            dgvProductos.Location = new Point(30, 420);
            dgvProductos.Size = new Size(970, 200);
            dgvProductos.BackgroundColor = Color.White;
            dgvProductos.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
            dgvProductos.AllowUserToAddRows = false;
            dgvProductos.ReadOnly = true;
            dgvProductos.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            dgvProductos.MultiSelect = false;

            this.Controls.Add(dgvProductos);
            CargarProductos();

            btnModificar.Click += BtnModificar_Click;
            dgvProductos.CellClick += DgvProductos_CellClick;
            btnLimpiar.Click += (sender, e) => LimpiarCampos();
            btnEliminar.Click += BtnEliminar_Click;
        }
        private bool ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text) ||
                string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtPrecio.Text) ||
                string.IsNullOrWhiteSpace(txtStock.Text))
            {
                MessageBox.Show("Completa los campos obligatorios.");
                return false;
            }

            // NUEVAS VALIDACIONES
            if (txtCodigo.Text.Trim().Length < 3)
            {
                MessageBox.Show("El código debe tener al menos 3 caracteres.");
                return false;
            }

            if (txtNombre.Text.Trim().Length < 3)
            {
                MessageBox.Show("El nombre debe tener al menos 3 caracteres.");
                return false;
            }

            if (!decimal.TryParse(txtPrecio.Text, out decimal precio) || precio < 0)
            {
                MessageBox.Show("Introduce un precio válido y no negativo.");
                return false;
            }

            if (!int.TryParse(txtStock.Text, out int stock) || stock < 0)
            {
                MessageBox.Show("Introduce una cantidad de stock válida y no negativa.");
                return false;
            }

            return true;
        }
        private void DgvProductos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow fila = dgvProductos.Rows[e.RowIndex];

                txtCodigo.Text = fila.Cells["Codigo"].Value.ToString();
                txtNombre.Text = fila.Cells["Nombre"].Value.ToString();
                txtDescripcion.Text = fila.Cells["Descripcion"].Value?.ToString();
                txtPrecio.Text = fila.Cells["Precio"].Value.ToString();
                txtStock.Text = fila.Cells["Stock"].Value.ToString();
            }
        }
        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProductos.CurrentRow == null)
                {
                    MessageBox.Show("Selecciona un producto de la tabla.");
                    return;
                }

                int idProducto = Convert.ToInt32(
                    dgvProductos.CurrentRow.Cells["IdProducto"].Value);

                DialogResult resultado = MessageBox.Show(
                    "¿Estás seguro de que deseas eliminar este producto?",
                    "Confirmar eliminación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (resultado == DialogResult.Yes)
                {
                    productosDAL.EliminarProducto(idProducto);

                    MessageBox.Show("Producto eliminado correctamente.");

                    CargarProductos();
                    LimpiarCampos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message);
            }
        }
        private void BtnLimpiarBusqueda_Click(object sender, EventArgs e)
        {
            try
            {
                txtBuscar.Clear();

                vistaProductos = productosDAL.ConsultarProductos().DefaultView;
                vistaProductos.Sort = "";

                dgvProductos.DataSource = vistaProductos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al limpiar la búsqueda: " + ex.Message);
            }
        }
        private void BtnModificar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos())
            {
                return;
            }
            try
            {
                if (dgvProductos.CurrentRow == null)
                {
                    MessageBox.Show("Selecciona un producto de la tabla.");
                    return;
                }

                int idProducto = Convert.ToInt32(
                    dgvProductos.CurrentRow.Cells["IdProducto"].Value);

                string codigo = txtCodigo.Text;
                string nombre = txtNombre.Text;
                string descripcion = txtDescripcion.Text;
                decimal precio = Convert.ToDecimal(txtPrecio.Text);
                int stock = Convert.ToInt32(txtStock.Text);

                productosDAL.ModificarProducto(
                    idProducto, codigo, nombre, descripcion, precio, stock);

                MessageBox.Show("Producto modificado correctamente.");

                CargarProductos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar: " + ex.Message);
            }
        }

        private void LimpiarCampos()
        {
            txtCodigo.Clear();
            txtNombre.Clear();
            txtDescripcion.Clear();
            txtPrecio.Clear();
            txtStock.Clear();

            dgvProductos.ClearSelection();
        }

        private void CargarProductos()
        {
            try
            {
                dgvProductos.DataSource = productosDAL.ConsultarProductos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Error al cargar los productos: " + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            

        }
        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string busqueda = txtBuscar.Text.Trim();

                DataTable tabla = productosDAL.ConsultarProductos();
                vistaProductos = tabla.DefaultView;

                if (!string.IsNullOrWhiteSpace(busqueda))
                {
                    vistaProductos.RowFilter = string.Format(
                        "Codigo LIKE '%{0}%' OR Nombre LIKE '%{0}%'",
                        busqueda.Replace("'", "''")
                    );
                }

                dgvProductos.DataSource = vistaProductos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar productos: " + ex.Message);
            }
        }
        private void BtnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarCampos())
            {
                return;
            }
            try
            {
                string codigo = txtCodigo.Text;
                string nombre = txtNombre.Text;
                string descripcion = txtDescripcion.Text;
                decimal precio = Convert.ToDecimal(txtPrecio.Text);
                int stock = Convert.ToInt32(txtStock.Text);

                productosDAL.RegistrarProducto(
                    codigo, nombre, descripcion, precio, stock);

                MessageBox.Show("Producto registrado correctamente.");

                CargarProductos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al registrar: " + ex.Message);
            }
        }
        private void BtnOrdenar_Click(object sender, EventArgs e)
        {
            try
            {
                if (vistaProductos == null)
                {
                    DataTable tabla = productosDAL.ConsultarProductos();
                    vistaProductos = tabla.DefaultView;
                }

                switch (cmbOrdenar.SelectedIndex)
                {
                    case 0:
                        vistaProductos.Sort = "Nombre ASC";
                        break;

                    case 1:
                        vistaProductos.Sort = "Nombre DESC";
                        break;

                    case 2:
                        vistaProductos.Sort = "Precio ASC";
                        break;

                    case 3:
                        vistaProductos.Sort = "Precio DESC";
                        break;
                }

                dgvProductos.DataSource = vistaProductos;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al ordenar productos: " + ex.Message);
            }
        }

        private void CrearEtiqueta(Control contenedor, string texto, int x, int y)
        {
            Label etiqueta = new Label();
            etiqueta.Text = texto;
            etiqueta.Location = new Point(x, y);
            etiqueta.Size = new Size(100, 25);
            etiqueta.Font = new Font("Segoe UI", 9);

            contenedor.Controls.Add(etiqueta);
        }

        private TextBox CrearCampo(Control contenedor, int x, int y, int ancho)
        {
            TextBox campo = new TextBox();
            campo.Location = new Point(x, y);
            campo.Size = new Size(ancho, 28);
            campo.Font = new Font("Segoe UI", 10);

            contenedor.Controls.Add(campo);

            return campo;
        }

        private Button CrearBoton(string texto, int x, int y, Color color)
        {
            Button boton = new Button();
            boton.Text = texto;
            boton.Location = new Point(x, y);
            boton.Size = new Size(130, 35);
            boton.BackColor = color;
            boton.ForeColor = Color.White;
            boton.FlatStyle = FlatStyle.Flat;
            boton.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            boton.Cursor = Cursors.Hand;

            return boton;
        }
    }
}
