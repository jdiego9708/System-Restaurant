namespace CapaEntidades.Models
{
    using System;
    using System.Data;

    public class Turno
    {
        public Turno()
        {

        }

        public Turno(DataRow row)
        {
            try
            {
                this.Id_turno = Convert.ToInt32(row["Id_turno"]);
                this.Fecha_turno = Convert.ToDateTime(row["Fecha_turno"]);
                this.Valor_inicial = Convert.ToDecimal(row["Valor_inicial"]);
                this.Total_turno = Convert.ToDecimal(row["Total_turno"]);
                this.Estado_turno = Convert.ToString(row["Estado_turno"]);
            }
            catch (Exception ex)
            {
                OnError?.Invoke(ex.Message, null);
            }
        }

        public int Id_turno { get; set; }

        public DateTime Fecha_turno { get; set; }

        public decimal Valor_inicial { get; set; }

        public decimal Total_turno { get; set; }

        public string Estado_turno { get; set; }

        public event EventHandler OnError;
    }
}
