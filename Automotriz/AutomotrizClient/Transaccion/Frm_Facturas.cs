using CarpinteriaApp.datos;
using CarpinteriaApp.dominio;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace CarpinteriaApp.formularios
{
    public partial class Frm_Facturas : Form
    {
        private HelperDB helper;
        private Factura nuevo;
        public Frm_Facturas()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            helper = new HelperDB();
            nuevo = new Factura();
            

        }

        private const int cGrip = 16;      // Grip size
        private const int cCaption = 32;   // Caption bar height;

        protected override void OnPaint(PaintEventArgs e)
        {
            Rectangle rc = new Rectangle(this.ClientSize.Width - cGrip, this.ClientSize.Height - cGrip, cGrip, cGrip);
            ControlPaint.DrawSizeGrip(e.Graphics, this.BackColor, rc);
            rc = new Rectangle(0, 0, this.ClientSize.Width, cCaption);
            e.Graphics.FillRectangle(Brushes.White, rc);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x84)
            {  // Trap WM_NCHITTEST
                Point pos = new Point(m.LParam.ToInt32());
                pos = this.PointToClient(pos);
                if (pos.Y < cCaption)
                {
                    m.Result = (IntPtr)2;  // HTCAPTION
                    return;
                }
                if (pos.X >= this.ClientSize.Width - cGrip && pos.Y >= this.ClientSize.Height - cGrip)
                {
                    m.Result = (IntPtr)17; // HTBOTTOMRIGHT
                    return;
                }
            }
            base.WndProc(ref m);
        }

        private void Frm_Facturas_Load_1(object sender, EventArgs e)
        {

            UltimaFactura();
            CargarClientes();
            CargarPlanes();
            CargarEmpleados();
            CargarTipoProductos();
            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            cboClientes.SelectedIndex = -1;
            cbxEmpleado.SelectedIndex = -1;
            cbxPlan.SelectedIndex = -1;
            cboTipoProducto.Text = "Seleccione el Tipo de Producto";
            cboProductos.Text = "Seleccione el Producto";
            lblPlan.Select();
            cboProductos.Enabled = false;
            txtNroFactura.Hide();
            dgvDetalles1.Hide();
            btnBuscar.Hide();
        }


        private void UltimaFactura()
        {
            int prox = helper.UltimaFactura();
            lblNroFactura.Text = "Nº Factura:   " + prox.ToString();
        }


        private void btnAgregar_Click(object sender, EventArgs e)
        {

            if (cboProductos.Text.Equals(String.Empty))
            {
                MessageBox.Show("Debe seleccionar un PRODUCTO!", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (txtCantidad.Text == "" || !int.TryParse(txtCantidad.Text, out _))
            {
                MessageBox.Show("Debe ingresar una cantidad válida!", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            foreach (DataGridViewRow row in dgvDetalles.Rows)
            {
                if (row.Cells["colProd"].Value.ToString().Equals(cboProductos.Text))
                {
                    MessageBox.Show("PRODUCTO: " + cboProductos.Text + " ya se encuentra como detalle!", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;

                }
            }
            DataRowView item = (DataRowView)cboProductos.SelectedItem;

            int prod = Convert.ToInt32(item.Row.ItemArray[0]);
            string nom = item.Row.ItemArray[1].ToString();
            double pre = Convert.ToDouble(item.Row.ItemArray[5]);
            Producto p = new Producto(prod, nom, pre);
            int cantidad = Convert.ToInt32(txtCantidad.Text);

            DetalleFactura detalle = new DetalleFactura(p, cantidad);
            nuevo.AgregarDetalle(detalle);
            Console.WriteLine(nuevo);
            dgvDetalles.Rows.Add(new object[] { item.Row.ItemArray[0], item.Row.ItemArray[1], item.Row.ItemArray[5], txtCantidad.Text, pre * cantidad });

            CalcularTotal();
        }

        private void CargarAutopartes()
        {
            DataTable table = helper.ConsultaSQL("SP_CONSULTAR_AUTOPARTES");
            if (table != null)
            {
                cboProductos.DataSource = table;
                cboProductos.DisplayMember = "descripcion";
                cboProductos.ValueMember = "cod_producto";
            }
        }

        private void CargarAutomoviles()
        {
            DataTable table = helper.ConsultaSQL("SP_CONSULTAR_AUTOMOVILES");
            if (table != null)
            {
                cboProductos.DataSource = table;
                cboProductos.DisplayMember = "descripcion";
                cboProductos.ValueMember = "cod_producto";
            }
        }

        private void CargarClientes()
        {
            DataTable table = helper.ConsultaSQL("SP_CONSULTAR_CLIENTES");
            if (table != null)
            {
                cboClientes.DataSource = table;
                cboClientes.DisplayMember = "cliente";
                cboClientes.ValueMember = "cod_cliente";
            }
        }

        private void CargarPlanes()
        {
            DataTable table = helper.ConsultaSQL("SP_CONSULTAR_PLANES");
            if (table != null)
            {
                cbxPlan.DataSource = table;
                cbxPlan.DisplayMember = "nom_plan";
                cbxPlan.ValueMember = "cod_plan";
            }
        }

        private void CargarEmpleados()
        {
            DataTable table = helper.ConsultaSQL("SP_CONSULTAR_EMPLEADOS");
            if (table != null)
            {
                cbxEmpleado.DataSource = table;
                cbxEmpleado.DisplayMember = "empleado";
                cbxEmpleado.ValueMember = "cod_empleado";
            }
        }

        private void CargarTipoProductos()
        {
            DataTable table = helper.ConsultaSQL("SP_CONSULTAR_TIPOSPROD");
            if (table != null)
            {
                cboTipoProducto.DataSource = table;
                cboTipoProducto.DisplayMember = "descripcion";
                cboTipoProducto.ValueMember = "cod_tipo_producto";
            }
        }

        private void cboTipoProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboProductos.Enabled = true;
            if (cboTipoProducto.SelectedIndex == 0)
            {
                cbxPlan.SelectedIndex = -1;
                cbxPlan.Enabled = false;
                CargarAutopartes();
            }
            if(cboTipoProducto.SelectedIndex == 1)
            {
                cbxPlan.SelectedIndex = -1;
                cbxPlan.Enabled = true;
                CargarAutomoviles();
            }
        }

        private void GuardarFactura()
        {
            nuevo.Cliente = cboClientes.SelectedIndex + 1;
            nuevo.Plan = cbxPlan.SelectedIndex + 1;
            nuevo.Empleado = cbxEmpleado.SelectedIndex + 1;

            if (helper.ConfirmarFactura(nuevo))
            {
                MessageBox.Show("Factura Cargada!", "Informe", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } else
            {
                MessageBox.Show("ERROR. No se pudo cargar la factura!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            cboClientes.SelectedIndex = -1;
            cbxEmpleado.SelectedIndex = -1;
            cbxPlan.SelectedIndex = -1;
            cboTipoProducto.Text = "Seleccione el Tipo de Producto";
            cboProductos.Text = "Seleccione el Producto";
            lblPlan.Select();
            cboProductos.Enabled = false;
            dgvDetalles.Rows.Clear();

            for (int i = 0; i < nuevo.Detalles.Count; i++)
            {
                nuevo.QuitarDetalle(0);
            }

        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (cbxEmpleado.SelectedIndex == -1)
            {
                MessageBox.Show("Debe ingresar un empleado!", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (cboClientes.SelectedIndex == -1)
            {
                MessageBox.Show("Debe ingresar un cliente!", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (dgvDetalles.Rows.Count == 0)
            {
                MessageBox.Show("Debe ingresar al menos detalle!", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            GuardarFactura();
        }

        private void CalcularTotal()
        {
            double total = nuevo.CalcularTotal();
            txtFinal.Text = total.ToString();

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void dgvDetalles_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetalles.CurrentCell.ColumnIndex == 5)
            {
                nuevo.QuitarDetalle(dgvDetalles.CurrentRow.Index);
                dgvDetalles.Rows.Remove(dgvDetalles.CurrentRow);
                CalcularTotal();
            }
        }

        private void lblConsultar_Click(object sender, EventArgs e)
        {
            txtNroFactura.Show();
            dgvDetalles.Hide();
            dgvDetalles1.Show();
            btnBuscar.Show();
        }

        private void lblNuevo_Click(object sender, EventArgs e)
        {

            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            cboClientes.SelectedIndex = -1;
            cbxEmpleado.SelectedIndex = -1;
            cbxPlan.SelectedIndex = -1;
            cboTipoProducto.Text = "Seleccione el Tipo de Producto";
            cboProductos.Text = "Seleccione el Producto";
            lblPlan.Select();
            cboProductos.Enabled = false;
            txtNroFactura.Hide();
            dgvDetalles1.Hide();
            btnBuscar.Hide();
            dgvDetalles.Show();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {

            cboClientes.SelectedIndex = -1;
            cbxEmpleado.SelectedIndex = -1;
            cbxPlan.SelectedIndex = -1;

            int nrofac = Convert.ToInt32(txtNroFactura.Text);
            try
            {
                string cliente = helper.ConsultaNombre(nrofac);
                cboClientes.Text = cliente;
            } catch (Exception)
            {
                MessageBox.Show("No se encontro ese numero de factura!", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            string empleado = helper.ConsultaEmpleado(nrofac);
            cbxEmpleado.Text = empleado;

            string plan = helper.ConsultaPlan(nrofac);
            cbxPlan.Text = plan;

            dgvDetalles1.DataSource = null;
            dgvDetalles1.Rows.Clear();
            dgvDetalles1.Refresh();

            DataTable table = helper.ConsultaDGV(nrofac);
            if (table != null)
            {
                dgvDetalles1.DataSource = table;
            }

        }
    }
}
