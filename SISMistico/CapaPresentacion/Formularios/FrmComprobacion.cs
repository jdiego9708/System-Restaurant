using CapaNegocio;
using System;
using System.Data;
using System.Windows.Forms;

namespace CapaPresentacion.Formularios
{
    public partial class FrmComprobacion : Form
    {
        public FrmComprobacion()
        {
            InitializeComponent();
            this.txtCodigo.KeyPress += TxtCodigo_KeyPress;
            this.txtCodigo.TextChanged += TxtCodigo_TextChanged;
            this.Load += FrmComprobacion_Load;
        }

        private string _cargo_empleado;
        private int _id_empleado;
        private string _nombre_empleado;

        public string Cargo_empleado { get => _cargo_empleado; set => _cargo_empleado = value; }
        public int Id_empleado { get => _id_empleado; set => _id_empleado = value; }
        public string Nombre_empleado { get => _nombre_empleado; set => _nombre_empleado = value; }

        private void FrmComprobacion_Load(object sender, EventArgs e)
        {
            this.txtCodigo.Focus();
        }

        private void TxtCodigo_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            try
            {
                if (txt.TextLength == 4)
                {
                    string rpta;
                    DataTable tabla =
                        NEmpleados.Login("CODIGO MAESTRO", txt.Text, "", out rpta);
                    if (rpta.Equals("OK"))
                    {
                        this.Id_empleado = Convert.ToInt32(tabla.Rows[0]["Id_empleado"]);
                        this.Nombre_empleado = Convert.ToString(tabla.Rows[0]["Nombre_empleado"]);
                        this.Cargo_empleado = Convert.ToString(tabla.Rows[0]["Cargo_empleado"]);

                        this.DialogResult = DialogResult.OK;
                        this.Tag = tabla;
                        this.Close();
                    }
                    else if (rpta.Equals(""))
                    {
                        Mensajes.MensajeInformacion("El código no corresponde a ninguno de nuestros empleados", "Entendido");
                    }
                    else
                    {
                        throw new Exception(rpta);
                    }
                }
            }
            catch (Exception ex)
            {
                Mensajes.MensajeErrorForm(ex.Message);
            }
        }

        private void TxtCodigo_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else if (Char.IsControl(e.KeyChar))
            {
                e.Handled = false;
                if ((int)e.KeyChar == (int)Keys.Enter)
                {
                    string rpta;
                    DataTable tabla =
                        NEmpleados.Login("CODIGO MAESTRO", this.txtCodigo.Text, "", out rpta);
                    if (rpta.Equals("OK"))
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Tag = tabla;
                        this.Close();
                    }
                    else if (rpta.Equals(""))
                    {
                        Mensajes.MensajeInformacion("El código no corresponde a ninguno de nuestros empleados", "Entendido");
                    }
                    else
                    {
                        throw new Exception(rpta);
                    }
                }
                else if ((int)e.KeyChar == (int)Keys.Escape)
                {
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
            else if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = true;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
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
