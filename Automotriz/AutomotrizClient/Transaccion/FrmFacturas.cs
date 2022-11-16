using AutomotrizClient.Http;
using Libreria.Datos;
using Libreria.Dominio;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace AutomotrizClient
{
    public partial class FrmFacturas : Form
    {
        private HelperDB helper;
        private Factura oFactura;

        public FrmFacturas()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.ResizeRedraw, true);
            helper = new HelperDB();
            oFactura = new Factura();
        }

        #region AGREGADOS ESTETICOS
        private const int cGrip = 16;      // Grip size
        private const int cCaption = 32;   // Caption bar height

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
        #endregion

        private void Frm_Facturas_Load_1(object sender, EventArgs e)
        {
            ProximaFactura();//OK
            CargarEmpleados();//OK
            CargarClientes();//OK
            CargarPlanes();//OK

            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");//OK

            lblPlan.Select();
            cboProductos.Enabled = false;
            txtNroFactura.Hide();
            btnBuscar.Hide();
        }

        #region SOPORTE WEBAPI
        private async void ProximaFactura()
        {
            string url = "http://localhost:5046/proxima_factura";
            var res = await ClientSingleton.GetInstance().GetAsync(url);
            var prox = JsonConvert.DeserializeObject<int>(res);
            lblNroFactura.Text = "Nº Factura:   " + prox.ToString();
        }

        private async void CargarAutopartes()
        {
            string url = "http://localhost:5046/autopartes";
            var res = await ClientSingleton.GetInstance().GetAsync(url);
            var lst = JsonConvert.DeserializeObject<List<Producto>>(res);
            cboProductos.DataSource = lst;
            //cboProductos.ValueMember = "NroSerie";
            cboProductos.ValueMember = "CodProducto";
            cboProductos.DisplayMember = "Descripcion";
        }

        private async void CargarAutomoviles()
        {
            string url = "http://localhost:5046/automoviles";
            var res = await ClientSingleton.GetInstance().GetAsync(url);
            var lst = JsonConvert.DeserializeObject<List<Producto>>(res);
            cboProductos.DataSource = lst;
            //cboProductos.ValueMember = "Patente";
            cboProductos.ValueMember = "CodProducto";
            cboProductos.DisplayMember = "Descripcion";
        }

        private async void CargarPlanes()
        {
            string url = "http://localhost:5046/autoplanes";
            var res = await ClientSingleton.GetInstance().GetAsync(url);
            var lst = JsonConvert.DeserializeObject<List<Autoplan>>(res);
            cboPlan.DataSource = lst;
            cboPlan.ValueMember = "CodPlan";
            cboPlan.DisplayMember = "NomPlan";
        }

        private async void CargarEmpleados()
        {
            string url = "http://localhost:5046/empleados";
            var res = await ClientSingleton.GetInstance().GetAsync(url);
            var lst = JsonConvert.DeserializeObject<List<Empleado>>(res);
            cboEmpleado.DataSource = lst;
            cboEmpleado.ValueMember = "Legajo";
            cboEmpleado.DisplayMember = "Nombre";
        }

        public async void CargarClientes()
        {
            string url = "http://localhost:5046/clientes";
            var res = await ClientSingleton.GetInstance().GetAsync(url);
            var lst = JsonConvert.DeserializeObject<List<Cliente>>(res);
            cboClientes.DataSource = lst;
            cboClientes.ValueMember = "NroFactura";
            cboClientes.DisplayMember = "Nombre";
        }
        #endregion

        private void BtnConsultar_Click(object sender, EventArgs e)
        {
            txtNroFactura.Show();
            dgvDetalles1.Hide();
            dgvDetalles1.Show();
            btnBuscar.Show();
        }

        private void BtnNuevo_Click(object sender, EventArgs e)
        {
            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");
            cboClientes.SelectedIndex = -1;
            cboEmpleado.SelectedIndex = -1;
            cboPlan.SelectedIndex = -1;
            cboProductos.Text = "Seleccione el Producto";
            lblPlan.Select();
            cboProductos.Enabled = false;
            txtNroFactura.Hide();
            btnBuscar.Hide();
            dgvDetalles1.Show();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }


        private void btnAgregar_Click(object sender, EventArgs e)
        {
            #region VALIDACIONES
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

            foreach (DataGridViewRow row in dgvDetalles1.Rows)
            {
                if (row.Cells["colProd"].Value.ToString().Equals(cboProductos.Text))
                {
                    MessageBox.Show("PRODUCTO: " + cboProductos.Text + " ya se encuentra como detalle!", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }
            #endregion

            DetalleFactura det;
            DataGridViewRow fila = new DataGridViewRow();
            fila.CreateCells(dgvDetalles1);

            if (rbAutomovil.Checked) //aca lo mismo que dije en DetallesFacturas, imagino que deberia de cambiar los campos de Patente y NroSerie por Cod_Producto.
            {
                //det = new DetalleFactura();
                //det.Auto = new Automovil();
                //det.Auto.Patente = (int)cboProductos.SelectedValue;
                //det.Cantidad = Convert.ToInt32(txtCantidad.Text);

                //fila.Cells[0].Value = det.Auto.Patente.ToString();

                det = new DetalleFactura();
                //det.Producto = new Producto();
                det.CodProducto = (int)cboProductos.SelectedValue;

                fila.Cells[0].Value = det.CodProducto.ToString();
            }
            else
            {
                //det = new DetalleFactura();
                //det.AutoP = new Autoparte();
                //det.AutoP.NroSerie = (int)cboProductos.SelectedValue;
                //det.Cantidad = Convert.ToInt32(txtCantidad.Text);

                //fila.Cells[0].Value = det.AutoP.NroSerie.ToString();

                det = new DetalleFactura();
                //det.Producto = new Producto();
                det.CodProducto = (int)cboProductos.SelectedValue;
                //det.Producto.Descripcion = cboProductos.Text;
                
                fila.Cells[0].Value = det.CodProducto.ToString();
            }

            det.Cantidad = Convert.ToInt32(txtCantidad.Text);
            det.Precio = Convert.ToDouble(txtPrecio.Text);
            fila.Cells[1].Value = cboProductos.Text;
            fila.Cells[2].Value = det.Cantidad.ToString();
            fila.Cells[3].Value = det.Precio.ToString();
            dgvDetalles1.Rows.Add(fila);

            oFactura.AgregarDetalle(det);
            CalcularTotal();
        }


        private async Task GuardarFacturaAsync()
        {
            //datos de la factura
            oFactura.CodEmpleado = cboEmpleado.SelectedIndex + 1;
            oFactura.NomCliente = cboClientes.Text;
            oFactura.CodPlan = cboPlan.SelectedIndex + 1;
            oFactura.CodTipoCliente = 1;
            oFactura.Cuit = 20418473593;


            string bodyContent = JsonConvert.SerializeObject(oFactura);

            string url = "http://localhost:5046/facturas";
            var res = await ClientSingleton.GetInstance().PostAsync(url, bodyContent);

            if (res.Equals("true"))
            {
                MessageBox.Show("Factura registrada", "Informe", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Dispose();
            }
            else
            {
                MessageBox.Show("No se pudo registrar la factura", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (cboEmpleado.SelectedIndex == -1)
            {
                MessageBox.Show("Debe ingresar un empleado!", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (cboClientes.SelectedIndex == -1)
            {
                MessageBox.Show("Debe ingresar un cliente!", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (dgvDetalles1.Rows.Count == 0)
            {
                MessageBox.Show("Debe ingresar al menos detalle!", "Control", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            GuardarFacturaAsync();
        }

        private void CalcularTotal()
        {
            double total = oFactura.CalcularTotal();
            txtFinal.Text = total.ToString();
        }

        private void dgvDetalles_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetalles1.CurrentCell.ColumnIndex == 5)
            {
                oFactura.QuitarDetalle(dgvDetalles1.CurrentRow.Index);
                dgvDetalles1.Rows.Remove(dgvDetalles1.CurrentRow);
                CalcularTotal();
            }
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {/*

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
        */
        }

        private void rbAutomovil_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAutomovil.Checked)
            {
                cboProductos.Enabled = true;
                CargarAutomoviles();
            }
            else
            {
                cboProductos.Enabled = true;
                cboPlan.Enabled = false;
                cboPlan.SelectedIndex = -1;
                CargarAutopartes();
            }
        }
    }
}