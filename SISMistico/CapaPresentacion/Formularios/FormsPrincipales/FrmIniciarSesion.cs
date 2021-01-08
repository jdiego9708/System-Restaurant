using CapaNegocio;
using System;
using System.Configuration;
using System.Data;
using System.ServiceProcess;
using System.Windows.Forms;

namespace CapaPresentacion.Formularios.FormsPrincipales
{
    public partial class FrmIniciarSesion : Form
    {
        public FrmIniciarSesion()
        {
            InitializeComponent();
            this.Load += FrmIniciarSesion_Load;
            this.ListaEmpleados.SelectedIndexChanged += ListaEmpleados_SelectedIndexChanged;
            this.btnCerrar.Click += BtnCerrar_Click;
            this.btnIngresar.Click += BtnIngresar_Click;
        }

        private void BtnIngresar_Click(object sender, EventArgs e)
        {
            try
            {
                string rpta;
                if (this.ListaEmpleados.Text != "" & this.txtPass.Texto != null)
                {
                    if (ListaEmpleados.Text.Equals("NINGUNO"))
                    {
                        if (this.txtPass.Texto.Equals("administrador"))
                        {
                            DatosInicioSesion datos = DatosInicioSesion.GetInstancia();
                            datos.Id_empleado = Convert.ToInt32(0);
                            datos.Nombre_empleado = Convert.ToString("Administrador");
                            datos.Cargo_empleado = "ADMINISTRADOR";

                            FrmPrincipal frmPrincipal = new FrmPrincipal();
                            frmPrincipal.WindowState = FormWindowState.Maximized;
                            frmPrincipal.Show();

                            this.Hide();
                        }
                        else if (this.txtPass.Texto.Equals("configadmin"))
                        {
                            FrmAdministracionAvanzada frm = new FrmAdministracionAvanzada();
                            frm.StartPosition = FormStartPosition.CenterScreen;
                            frm.ShowDialog();
                        }
                    }
                    else
                    {
                        DataTable tabla = NEmpleados.Login("LOGIN",
                        Convert.ToString(this.ListaEmpleados.SelectedValue), this.txtPass.Texto, out rpta);
                        if (rpta.Equals("OK"))
                        {
                            DatosInicioSesion datos = DatosInicioSesion.GetInstancia();
                            datos.Id_empleado = Convert.ToInt32(tabla.Rows[0]["Id_empleado"]);
                            datos.Nombre_empleado = Convert.ToString(tabla.Rows[0]["Nombre_empleado"]);
                            datos.Cargo_empleado = Convert.ToString(tabla.Rows[0]["Cargo_empleado"]);

                            FrmPrincipal frmPrincipal = new FrmPrincipal();
                            frmPrincipal.WindowState = FormWindowState.Maximized;
                            frmPrincipal.Show();

                            this.Hide();
                        }
                        else if (rpta.Equals(""))
                        {
                            Mensajes.MensajeInformacion("No se encontró el usuario, intentelo de nuevo", "Entendido");
                        }
                        else
                        {
                            throw new Exception(rpta);
                        }
                    }
                }
                else if (this.ListaEmpleados.Text.Equals(""))
                {
                    if (this.txtPass.Texto.Equals("configadmin"))
                    {
                        FrmAdministracionAvanzada frm = new FrmAdministracionAvanzada();
                        frm.StartPosition = FormStartPosition.CenterScreen;
                        frm.ShowDialog();
                    }
                }
                else
                {
                    Mensajes.MensajeErrorForm("La contraseña es obligatoria");
                }
            }
            catch (Exception ex)
            {
                Mensajes.MensajeErrorCompleto(this.Name, "BtnIngresar_Click",
                    "Hubo un error al ingresar", ex.Message);
            }
        }

        private void BtnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ListaEmpleados_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            int id_empleado;
            if (int.TryParse(Convert.ToString(cb.SelectedValue), out id_empleado))
            {
                this.txtPass.txtBusqueda.Focus();
            }
        }

        private void FrmIniciarSesion_Load(object sender, EventArgs e)
        {
            this.txtPass.txtBusqueda.TextAlign = HorizontalAlignment.Center;
            this.txtPass.Texto_inicial = "Contraseña";
            this.txtPass.txtBusqueda.UseSystemPasswordChar = true;

            string rpta;
            DataTable tablaEmpleados =
                NEmpleados.BuscarEmpleados("COMPLETO", "", out rpta);
            if (tablaEmpleados != null)
            {
                this.ListaEmpleados.DataSource = tablaEmpleados;
                this.ListaEmpleados.ValueMember = "Id_empleado";
                this.ListaEmpleados.DisplayMember = "Nombre_empleado";

                this.ListaEmpleados.SelectedValue = 0;
            }
            else
            {
                Mensajes.MensajePregunta("Hubo un error conectando con el servidor, desea intentar de nuevo?",
                    "Intentar de nuevo", "Cerrar", out DialogResult dialog);
                if (dialog == DialogResult.Yes)
                {
                    string servicio = Convert.ToString(ConfigurationManager.AppSettings["nameServiceStarter"]);
                    ServiceController sc = new ServiceController(servicio);
                    try
                    {
                        if (sc != null && sc.Status == ServiceControllerStatus.Stopped)
                        {
                            sc.Start();
                        }
                        sc.WaitForStatus(ServiceControllerStatus.Running);
                        sc.Close();
                    }
                    catch (Exception ex)
                    {
                        Mensajes.MensajeErrorCompleto(this.Name,"Iniciar servicio",
                            "Error al iniciar el servicio: ", ex.Message);
                    }
                }

                Mensajes.MensajeErrorCompleto(this.Name, "FrmIniciarSesion_Load",
                    "Hubo un error al conectarse con el servidor",
                    "Hubo un error al conectarse con el servidor, por favor intentelo de nuevo o envíe un ticket " +
                    "al administrador del sistema, detalles: " + rpta);
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter || keyData == Keys.Insert)
            {
                try
                {
                    string rpta;
                    if (this.ListaEmpleados.Text != "" & this.txtPass.Texto != null)
                    {
                        if (ListaEmpleados.Text.Equals("NINGUNO"))
                        {
                            if (this.txtPass.Texto.Equals("administrador"))
                            {
                                DatosInicioSesion datos = DatosInicioSesion.GetInstancia();
                                datos.Id_empleado = Convert.ToInt32(0);
                                datos.Nombre_empleado = Convert.ToString("Administrador");
                                datos.Cargo_empleado = "ADMINISTRADOR";

                                FrmPrincipal frmPrincipal = new FrmPrincipal();
                                frmPrincipal.WindowState = FormWindowState.Maximized;
                                frmPrincipal.Show();

                                this.Hide();
                            }
                            else if (this.txtPass.Texto.Equals("configadmin"))
                            {
                                FrmAdministracionAvanzada frm = new FrmAdministracionAvanzada();
                                frm.StartPosition = FormStartPosition.CenterScreen;
                                frm.ShowDialog();
                            }
                        }
                        else
                        {
                            DataTable tabla = NEmpleados.Login("LOGIN",
                            Convert.ToString(this.ListaEmpleados.SelectedValue), this.txtPass.Texto, out rpta);
                            if (rpta.Equals("OK"))
                            {
                                DatosInicioSesion datos = DatosInicioSesion.GetInstancia();
                                datos.Id_empleado = Convert.ToInt32(tabla.Rows[0]["Id_empleado"]);
                                datos.Nombre_empleado = Convert.ToString(tabla.Rows[0]["Nombre_empleado"]);
                                datos.Cargo_empleado = Convert.ToString(tabla.Rows[0]["Cargo_empleado"]);

                                FrmPrincipal frmPrincipal = new FrmPrincipal();
                                frmPrincipal.WindowState = FormWindowState.Maximized;
                                frmPrincipal.Show();

                                this.Hide();
                            }
                            else if (rpta.Equals(""))
                            {
                                Mensajes.MensajeInformacion("No se encontró el usuario, intentelo de nuevo", "Entendido");
                            }
                            else
                            {
                                throw new Exception(rpta);
                            }
                        }
                    }
                    else if (this.ListaEmpleados.Text.Equals(""))
                    {
                        if (this.txtPass.Texto.Equals("configadmin"))
                        {
                            FrmAdministracionAvanzada frm = new FrmAdministracionAvanzada();
                            frm.StartPosition = FormStartPosition.CenterScreen;
                            frm.ShowDialog();
                        }
                    }
                    else
                    {
                        Mensajes.MensajeErrorForm("La contraseña es obligatoria");
                    }

                }
                catch (Exception ex)
                {
                    Mensajes.MensajeErrorForm(ex.Message);
                }
            }
            else if (keyData == Keys.Escape)
            {
                this.Close();
            }
            else
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }
            return true;
        }
    }
}
