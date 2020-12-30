using CapaNegocio;
using CapaPresentacion.Formularios.FormsPedido.Bebidas;
using CapaPresentacion.Formularios.FormsPedido.Platos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Windows.Forms;

namespace CapaPresentacion.Formularios.FormsPedido
{
    public partial class FrmPedido : Form
    {
        public FrmPedido()
        {
            InitializeComponent();

            this.ContextMenuDatosPedido = new ContextMenuDatosPedido();
            this.PoperContainer = new PoperContainer(ContextMenuDatosPedido);

            this.btnQuitar.Click += BtnQuitar_Click;
            this.btnPlatos.Click += BtnPlatos_Click;
            this.btnBebidas.Click += BtnBebidas_Click;
            this.btnAgregar.Click += BtnAgregar_Click;
            this.dgvProductos.DataSourceChanged += DgvProductos_DataSourceChanged;
            this.Load += FrmPedido_Load;
            this.btnInformacionPedido.MouseDown += btnInformacionPedido_MouseDown;
            this.ContextMenuDatosPedido.btnTerminarPedido.Click += BtnTerminarPedido_Click;
        }

        private void Comprobacion()
        {
            FrmComprobacion frm = new FrmComprobacion();
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.FormClosed += Frm_FormClosed;
            frm.ShowDialog();
        }

        private void Frm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form frm = (Form)sender;
            if (frm.DialogResult == DialogResult.OK)
            {
                if (frm.Tag != null)
                {

                    DataTable table = (DataTable)frm.Tag;
                    if (table != null)
                    {
                        this.Id_empleado = Convert.ToInt32(table.Rows[0]["Id_empleado"]);
                        this.Cargo_empleado = Convert.ToString(table.Rows[0]["Cargo_empleado"]);
                        this.Nombre_empleado = Convert.ToString(table.Rows[0]["Nombre_empleado"]);
                    }
                }
            }
            else
            {
                this.Close();
            }
        }

        public void ObtenerTablaPedido(DataTable dt)
        {
            this.lblMistico.Text = "Editar el pedido de la mesa " + this.Numero_mesa;
            this.tablaspedido.Id_pedido = this.Id_pedido;
            this.tablaspedido.IsEditar = true;
            this.tablaspedido.CrearTablas(dt, this.Numero_mesa);
            this.ContextMenuDatosPedido.IsEditar = true;
            this.dgvProductos =
                ConfiguracionDatagridview.ConfigurationGrid(this.dgvProductos);
            this.dgvProductos.DataSource = this.tablaspedido.TablaVista;
            this.dgvProductos.Focus();
        }

        private TablasPedidoModificado tablaspedido = new TablasPedidoModificado();

        private List<string> Variables()
        {
            return new List<string>
            {
                Convert.ToString(this.Id_mesa), Convert.ToString(this.Id_empleado),
                Convert.ToString(this.ContextMenuDatosPedido.txtCliente.Tag), "0"
            };
        }

        private bool Comprobaciones()
        {
            bool result = true;
            if (this.dgvProductos.Rows.Count < 1)
            {
                Mensajes.MensajeErrorForm("Debe seleccionar un plato o bebida");
                result = false;
            }

            if (this.tablaspedido.ReturnTablaDetalle() == null)
            {
                Mensajes.MensajeErrorForm("Debe seleccionar un plato o bebida");
                result = false;
            }

            if (this.tablaspedido.ReturnTablaDetalle().Rows.Count < 1)
            {
                Mensajes.MensajeErrorForm("Debe seleccionar un plato o bebida");
                result = false;
            }

            if (!Editar)
            {
                if (Convert.ToString(this.ContextMenuDatosPedido.txtCliente.Tag).Equals("0"))
                {
                    DialogResult dialogResult;
                    Mensajes.MensajePregunta("No ha seleccionado un cliente, ¿desea continuar?",
                        "Continuar", "Cancelar", out dialogResult);
                    if (dialogResult == DialogResult.Yes)
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            return result;
        }

        private void BtnTerminarPedido_Click(object sender, EventArgs e)
        {
            Thread hilo = new Thread(() => Mensajes.MensajeEspera("Facturando y terminando"));
            try
            {
                hilo.Start();
                this.Focus();
                string rpta = "";
                int id_pedido;
                if (this.Editar)
                {
                    if (this.Comprobaciones())
                    {
                        DataTable TablaDetallePedido = this.tablaspedido.ReturnTablaDetalle();
                        if (TablaDetallePedido.Rows.Count > 0)
                        {
                            this.Close();
                        }
                        else
                        {
                            Mensajes.MensajeErrorForm("Debe seleccionar como mínimo un plato o bebida");
                        }
                    }
                }
                else
                {
                    if (this.Comprobaciones())
                    {
                        DataTable TablaDetallePedido = this.tablaspedido.ReturnTablaDetalle();
                        if (TablaDetallePedido.Rows.Count > 0)
                        {
                            rpta =
                                NPedido.InsertarPedido(this.Variables(), out id_pedido,
                                TablaDetallePedido);
                            if (rpta.Equals("OK"))
                            {
                                FrmObservarMesas FrmObservarMesas = FrmObservarMesas.GetInstancia();
                                FrmObservarMesas.ObtenerPedido(id_pedido, this.Numero_mesa, "OCUPADA");

                                FrmComandas comandas = new FrmComandas();
                                comandas.Id_pedido = id_pedido;
                                comandas.AsignarTablas();

                                bool plato = false;
                                bool bebida = false;
                                int imprimir = 0;
                                foreach (DataRow row in TablaDetallePedido.Rows)
                                {
                                    if (row["Tipo"].Equals("PLATO"))
                                    {
                                        plato = true;
                                    }
                                    else
                                    {
                                        bebida = true;
                                    }
                                    if (plato && bebida)
                                    {
                                        break;
                                    }
                                }

                                if (plato && bebida)
                                {
                                    imprimir = 2;
                                }
                                else
                                {
                                    imprimir = 1;
                                }

                                comandas.ImprimirFactura(imprimir);
                                this.Close();
                            }
                            else
                            {
                                throw new Exception(rpta);
                            }
                        }
                        else
                        {
                            Mensajes.MensajeErrorForm("Debe seleccionar como mínimo un plato o bebida");
                        }
                    }
                }
                hilo.Abort();
            }
            catch (Exception ex)
            {
                hilo.Abort();
                Mensajes.MensajeErrorCompleto(this.Name, "BtnTerminarPedido_Click",
                    "Hubo un error al terminar un pedido", ex.Message);
            }
        }

        private void SumarPrecios()
        {
            int total_parcial = 0;
            DataTable Tabla = (DataTable)this.dgvProductos.DataSource;
            if (Tabla != null)
            {
                foreach (DataRow row in Tabla.Rows)
                {
                    total_parcial += Convert.ToInt32(row["Total"]);
                }
            }
            this.Total_parcial = total_parcial;
            this.ContextMenuDatosPedido.Total_parcial = this.Total_parcial;
        }

        private void btnInformacionPedido_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this.Editar)
                {
                    Thread hilo = new Thread(() => Mensajes.MensajeEspera("Facturando y terminando"));
                    FrmComandas comandas = new FrmComandas();
                    comandas.Id_pedido = this.Id_pedido;
                    comandas.AsignarTablas(this.tablaspedido.ReturnTablaTemp());
                    bool plato = false;
                    bool bebida = false;
                    int imprimir = 0;
                    foreach (DataRow row in this.tablaspedido.ReturnTablaTemp().Rows)
                    {
                        if (row["Tipo"].Equals("PLATO"))
                        {
                            plato = true;
                        }
                        else
                        {
                            bebida = true;
                        }
                        if (plato && bebida)
                        {
                            break;
                        }
                    }

                    if (plato && bebida)
                    {
                        imprimir = 2;
                    }
                    else
                    {
                        imprimir = 1;
                    }
                    comandas.ImprimirFactura(imprimir);
                    hilo.Abort();
                    this.Close();
                }
                else
                {
                    this.SumarPrecios();
                    this.ContextMenuDatosPedido.ObtenerPrecio();
                    PoperContainer.Show(btnInformacionPedido);
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //
            // Si el control DataGridView no tiene el foco, 
            // se abandonamos el procedimiento, llamando al metodo base
            //
            if ((!dgvProductos.Focused))
                return base.ProcessCmdKey(ref msg, keyData);

            //
            // Si la tecla presionada es distinta al ENTER, 
            // se abandonamos el procedimiento, llamando al metodo base
            //
            if (keyData == Keys.Delete || keyData == Keys.Back)
            {
                if (this.dgvProductos.DataSource != null)
                {
                    if (this.dgvProductos.Rows.Count > 0)
                    {
                        string rpta = "OK";
                        int id_tipo =
                            Convert.ToInt32(this.dgvProductos.CurrentRow.Cells["Id_tipo"].Value);

                        if (id_tipo != 0)
                        {
                            rpta =
                                this.tablaspedido.EliminarTipo(id_tipo);
                            if (!rpta.Equals("OK"))
                            {
                                Mensajes.MensajeErrorForm("No se pudo quitar el tipo de la lista");
                            }
                        }
                    }
                }
            }
            else
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }
            return true;
        }

        public void ObtenerTipo()
        {
            try
            {
                string tipo = "";
                string rpta = "";
                string observaciones = "";
                List<string> datos = new List<string>();

                if (this.panel2.Tag.Equals("FrmPedidoBebidas"))
                {
                    datos = this.FrmPedidoBebidas.AsignarLista(out rpta);
                    tipo = "BEBIDAS";
                }
                else
                {
                    datos = this.FrmPedidoPlatos.AsignarLista(out rpta);
                    tipo = "PLATO";
                }

                //if (datos != null)
                //    this.dgvProductos.DataSource =
                //        tablaspedido.AgregarTipo(datos, out rpta, tipo, observaciones);

                if (!rpta.Equals("OK"))
                {
                    throw new Exception(rpta);
                }

            }
            catch (Exception ex)
            {
                Mensajes.MensajeErrorCompleto(this.Name, "",
                    "Hubo un error al agregar el plato", ex.Message);
            }
        }

        private void BtnQuitar_Click(object sender, EventArgs e)
        {
            this.Comprobacion();
            if (this.Cargo_empleado.Equals("ADMINISTRADOR"))
            {
                string rpta = "OK";
                this.dgvProductos.Focus();
                if (this.dgvProductos.DataSource != null &&
                    this.dgvProductos.Enabled && this.dgvProductos.Focused &&
                    this.dgvProductos.Rows.Count > 0)
                {
                    int id_tipo =
                        Convert.ToInt32(this.dgvProductos.CurrentRow.Cells["Id_tipo"].Value);

                    if (id_tipo != 0)
                    {
                        rpta =
                            this.tablaspedido.EliminarTipo(id_tipo);
                        if (!rpta.Equals("OK"))
                        {
                            Mensajes.MensajeErrorForm("No se pudo quitar el tipo de la lista");
                        }
                    }
                }
            }
            else
            {
                Mensajes.MensajeInformacion("No tiene permisos suficientes para realizar esta acción", "Entendido");
            }
        }

        private void FrmPedido_Load(object sender, EventArgs e)
        {
            if (!this.Editar)
            {
                this.dgvProductos =
                ConfiguracionDatagridview.ConfigurationGrid(this.dgvProductos);
                this.dgvProductos.Focus();
                this.tablaspedido.CrearTablas(this.Numero_mesa);
                this.lblMistico.Text = "Agregar al pedido de la mesa " + this.Numero_mesa;

                this.Comprobacion();
            }
        }

        private void DgvProductos_DataSourceChanged(object sender, EventArgs e)
        {
            try
            {
                bool[] columns_visible =
                {
                    false, true, true
                };
                this.dgvProductos =
                    DatagridString.ChangeColumnsVisible(this.dgvProductos, columns_visible);
            }
            catch (Exception ex)
            {
                Mensajes.MensajeErrorCompleto(this.Name, "DgvProductos_DataSourceChanged",
                    "Hubo un error al cambiar las propiedades de la columna en el dgv", ex.Message);
            }
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            //string mensaje_respuesta = "";
            try
            {
                if (this.panel2.Controls.Count > 0)
                {
                    string tipo = "";
                    string rpta = "";
                    string observaciones = "";
                    List<string> datos = new List<string>();

                    if (Convert.ToString(this.panel2.Tag).Equals("FrmPedidoBebidas"))
                    {
                        datos = this.FrmPedidoBebidas.AsignarLista(out rpta);
                        tipo = "BEBIDA";
                    }
                    else
                    {
                        datos = this.FrmPedidoPlatos.AsignarLista(out rpta);
                        tipo = "PLATO";
                    }

                    if (rpta.Equals("OK"))
                    {
                        //this.dgvProductos.DataSource =
                        //    tablaspedido.AgregarTipo(datos, out rpta, tipo, observaciones);
                        if (!rpta.Equals("OK"))
                        {
                            throw new Exception(rpta);
                        }
                    }
                    else
                    {
                        if (!rpta.Equals("No hay filas seleccionadas"))
                            throw new Exception(rpta);
                    }
                }
            }
            catch (Exception ex)
            {
                Mensajes.MensajeErrorCompleto(this.Name, "",
                    "Hubo un error al agregar el plato", ex.Message);
            }
        }

        private Form ComprobarExistencia(Form form)
        {
            Form frmDevolver = null;
            foreach (Form frm in this.panel2.Controls)
            {
                if (frm.Name.Equals(form.Name))
                {
                    frmDevolver = frm;
                    break;
                }

            }
            return frmDevolver;
        }

        private void BtnPlatos_Click(object sender, EventArgs e)
        {
            try
            {
                this.panel2.Controls.Clear();

                FrmPedidoPlatos.FrmPedido = this;
                FrmPedidoPlatos.Text = "PLATOS";
                FrmPedidoPlatos.TopLevel = false;
                FrmPedidoPlatos.FormBorderStyle = FormBorderStyle.None;
                FrmPedidoPlatos.Dock = DockStyle.Fill;
                Form FormComprobado = this.ComprobarExistencia(FrmPedidoPlatos);
                if (FormComprobado != null)
                {
                    FrmPedidoPlatos.WindowState = FormWindowState.Normal;
                    FrmPedidoPlatos.Activate();
                }
                else
                {
                    this.panel2.Controls.Add(FrmPedidoPlatos);
                    FrmPedidoPlatos.Show();
                }
                FrmPedidoPlatos.BringToFront();
                FrmPedidoPlatos.Activate();
                this.panel2.Tag = FrmPedidoPlatos.Name;
            }
            catch (Exception ex)
            {
                Mensajes.MensajeErrorCompleto(this.Name, "BtnPlatos_Click",
                    "Hubo un error con el menu observar platos", ex.Message);
            }
        }

        private void BtnBebidas_Click(object sender, EventArgs e)
        {
            try
            {
                this.panel2.Controls.Clear();
                FrmPedidoBebidas.FrmPedido = this;
                FrmPedidoBebidas.Text = "BEBIDAS";
                FrmPedidoBebidas.TopLevel = false;
                FrmPedidoBebidas.FormBorderStyle = FormBorderStyle.None;
                FrmPedidoBebidas.Dock = DockStyle.Fill;
                Form FormComprobado = this.ComprobarExistencia(FrmPedidoBebidas);
                if (FormComprobado != null)
                {
                    FrmPedidoBebidas.WindowState = FormWindowState.Normal;
                    FrmPedidoBebidas.Activate();
                }
                else
                {
                    this.panel2.Controls.Add(FrmPedidoBebidas);
                    FrmPedidoBebidas.Show();
                }
                FrmPedidoBebidas.BringToFront();
                FrmPedidoBebidas.Activate();
                this.panel2.Tag = FrmPedidoBebidas.Name;
            }
            catch (Exception ex)
            {
                Mensajes.MensajeErrorCompleto(this.Name, "BtnBebidas_Click",
                    "Hubo un error con el menu observar bebidas", ex.Message);
            }
        }

        ContextMenuDatosPedido ContextMenuDatosPedido;
        PoperContainer PoperContainer;
        public FrmPedidoPlatos FrmPedidoPlatos = new FrmPedidoPlatos();
        public FrmPedidoBebidas FrmPedidoBebidas = new FrmPedidoBebidas();
        int numero_mesa;
        private int total_parcial;
        int id_empleado;
        int id_mesa;
        int id_pedido;
        private bool editar;
        private string cargo_empleado;
        private string nombre_empleado;

        public int Numero_mesa { get => numero_mesa; set => numero_mesa = value; }
        public int Total_parcial { get => total_parcial; set => total_parcial = value; }
        public int Id_empleado { get => id_empleado; set => id_empleado = value; }
        public int Id_mesa { get => id_mesa; set => id_mesa = value; }
        public bool Editar { get => editar; set => editar = value; }
        public int Id_pedido { get => id_pedido; set => id_pedido = value; }
        public string Cargo_empleado { get => cargo_empleado; set => cargo_empleado = value; }
        public string Nombre_empleado { get => nombre_empleado; set => nombre_empleado = value; }
    }
}
