using CapaEntidades.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion.Formularios.FormsEmpleados
{
    public partial class EmpleadoNominaSmall : UserControl
    {
        public EmpleadoNominaSmall()
        {
            InitializeComponent();
            this.btnPagar.Click += BtnPagar_Click;
        }

        public event EventHandler OnBtnPagarClick;

        private void BtnPagar_Click(object sender, EventArgs e)
        {
            OnBtnPagarClick?.Invoke(sender, e);
        }

        private void AsignarDatos(EmpleadoNominaBinding empleadoNomina)
        {
            this.txtNombre.Text = empleadoNomina.Empleado.Nombre_empleado + " " + empleadoNomina.Empleado.Telefono_empleado;
            this.txtTotalPropinas.Text = empleadoNomina.Propinas.ToString("N2");
            this.txtSalario.Text = empleadoNomina.Salario.ToString("N2");
            this.txtOtrosIngresos.Text = empleadoNomina.Otros_ingresos.ToString("N2");
            this.txtEgresos.Text = empleadoNomina.Egresos.ToString("N2");
            this.lblTotal.Text = empleadoNomina.Total_nomina.ToString("C");
            this.lblEstado.Text = empleadoNomina.Estado_nomina;
        }

        EmpleadoNominaBinding _eEmpleadoNomina;

        public EmpleadoNominaBinding EmpleadoNominaBinding
        { 
            get => _eEmpleadoNomina;
            set
            {
                _eEmpleadoNomina = value;
                this.AsignarDatos(value);
            }
        }
    }
}
