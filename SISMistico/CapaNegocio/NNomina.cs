using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CapaDatos;
using System.Data;
using CapaEntidades.Models;

namespace CapaNegocio
{
    public class NNomina
    {
        #region INSERTAR NOMINA EMPLEADO

        public static async Task<(string rpta, int id_nomina)> InsertarEmpleado(EmpleadoNominaBinding empleadoNomina)
        {
            DNomina DNomina = new DNomina();
            return await DNomina.InsertarNomina(empleadoNomina);
        }

        #endregion

        #region EDITAR NOMINA

        public static async Task<string> EditarNomina(int id_nomina, EmpleadoNominaBinding empleadoNomina)
        {
            DNomina DNomina = new DNomina();
            return await DNomina.EditarNomina(id_nomina, empleadoNomina);
        }
        #endregion

        #region BUSCAR EMPLEADOS

        public static async Task<(string rpta, DataTable dtNomina)> BuscarNomina(string tipo_busqueda, string texto_busqueda)
        {
            DNomina DNomina = new DNomina();
            return await DNomina.BuscarNomina(tipo_busqueda, texto_busqueda);
        }

        #endregion
    }
}
