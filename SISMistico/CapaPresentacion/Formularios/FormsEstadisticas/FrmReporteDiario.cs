using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion.Formularios.FormsEstadisticas
{
    public partial class FrmReporteDiario : Form
    {
        public FrmReporteDiario()
        {
            InitializeComponent();
            this.Load += FrmReporteDiario_Load;
        }

        private void FrmReporteDiario_Load(object sender, EventArgs e)
        {
            this.reportViewer1 = new ReportViewer();
            this.Controls.Add(this.reportViewer1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource =
                "CapaPresentacion.ReportesFacturas.NominaEmpleado.rptNominaEmpleado.rdlc";
        }

        public string InformacionEmpleado { get; set; }
        public string CantidadPedidos { get; set; }
        public string ResumenResultados { get; set; }
        public string InformacionEgresos { get; set; }
        public string IdentificacionTurno { get; set; }
        public string FechaHora { get; set; }

        private ReportViewer reportViewer1;
        ControladorImpresion objImpresion = new ControladorImpresion();

        public void ObtenerReporte()
        {
            this.reportViewer1 = new ReportViewer
            {
                Dock = DockStyle.Fill,
                Location = new Point(0, 0),
            };
            this.Controls.Add(this.reportViewer1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource =
            "CapaPresentacion.Formularios.FormsEstadisticas.rptReporteDiario.rdlc";

            ReportParameter[] reportParameters = new ReportParameter[6];
            reportParameters[0] = new ReportParameter("InformacionEmpleado", InformacionEmpleado);
            reportParameters[1] = new ReportParameter("CantidadPedidos", CantidadPedidos);
            reportParameters[2] = new ReportParameter("ResumenResultados", ResumenResultados);
            reportParameters[3] = new ReportParameter("InformacionEgresos", InformacionEgresos);
            reportParameters[4] = new ReportParameter("IdentificacionTurno", IdentificacionTurno);
            reportParameters[5] = new ReportParameter("FechaHora", FechaHora);
            this.reportViewer1.LocalReport.SetParameters(reportParameters);
            this.reportViewer1.RefreshReport();
        }

        public void ImprimirFactura(int Repetir)
        {
            try
            {
                ReportParameter[] reportParameters = new ReportParameter[6];
                reportParameters[0] = new ReportParameter("InformacionEmpleado", InformacionEmpleado);
                reportParameters[1] = new ReportParameter("CantidadPedidos", CantidadPedidos);
                reportParameters[2] = new ReportParameter("ResumenResultados", ResumenResultados);
                reportParameters[3] = new ReportParameter("InformacionEgresos", InformacionEgresos);
                reportParameters[4] = new ReportParameter("IdentificacionTurno", IdentificacionTurno);
                reportParameters[5] = new ReportParameter("FechaHora", FechaHora);
                this.reportViewer1.LocalReport.SetParameters(reportParameters);
                this.reportViewer1.RefreshReport();

                int contador = 0;
                while (contador != Repetir)
                {
                    objImpresion.Imprimir(reportViewer1.LocalReport);
                    objImpresion.Dispose();

                    contador += 1;
                }
            }
            catch (Exception ex)
            {
                Mensajes.MensajeErrorForm(ex.Message);
            }
        }

    }
}
