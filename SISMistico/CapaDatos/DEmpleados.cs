using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class DEmpleados
    {
        #region MENSAJE SQL
        private void SqlCon_InfoMessage(object sender, SqlInfoMessageEventArgs e)
        {
            this.Mensaje_respuesta = e.Message;
        }
        #endregion

        #region VARIABLES
        private string mensaje_respuesta;
        public string Mensaje_respuesta
        {
            get
            {
                return mensaje_respuesta;
            }

            set
            {
                mensaje_respuesta = value;
            }
        }

        public string[] Variables
        {
            get
            {
                return _variables;
            }

            set
            {
                _variables = value;
            }
        }

        private string[] _variables;
        #endregion

        #region CONSTRUCTOR VACIO
        public DEmpleados() { }
        #endregion

        #region METODO INSERTAR EMPLEADOS
        public string InsertarEmpleado(DEmpleados DEmpleados)
        {
            int contador = 0;
            //asignamos a una cadena string la variable rpta y la iniciamos en vacía
            string rpta = "";
            SqlConnection SqlCon = new SqlConnection();
            SqlCon.InfoMessage += new SqlInfoMessageEventHandler(SqlCon_InfoMessage);
            SqlCon.FireInfoMessageEventOnUserErrors = true;
            //Capturador de errores
            try
            {
                SqlCon.ConnectionString = Conexion.Cn;
                SqlCon.Open();
                //establecer comando
                SqlCommand SqlCmd = new SqlCommand
                {
                    Connection = SqlCon,
                    CommandText = "sp_Insertar_empleado",
                    //Indicamos que es un procedimiento almacenado
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter Id_empleado = new SqlParameter
                {
                    ParameterName = "@Id_empleado",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };
                SqlCmd.Parameters.Add(Id_empleado);

                SqlParameter Nombre_empleado = new SqlParameter
                {
                    ParameterName = "@Nombre_empleado",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 500,
                    Value = DEmpleados.Variables[contador].Trim().ToUpper()
                };
                SqlCmd.Parameters.Add(Nombre_empleado);
                contador += 1;

                SqlParameter Telefono_empleado = new SqlParameter
                {
                    ParameterName = "@Telefono_empleado",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = DEmpleados.Variables[contador].Trim().ToUpper()
                };
                SqlCmd.Parameters.Add(Telefono_empleado);
                contador += 1;

                SqlParameter Correo_electronico = new SqlParameter
                {
                    ParameterName = "@Correo_electronico",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 500,
                    Value = DEmpleados.Variables[contador].Trim().ToUpper()
                };
                SqlCmd.Parameters.Add(Correo_electronico);
                contador += 1;

                SqlParameter Cargo_empleado = new SqlParameter
                {
                    ParameterName = "@Cargo_empleado",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = DEmpleados.Variables[contador].Trim().ToUpper()
                };
                SqlCmd.Parameters.Add(Cargo_empleado);
                contador += 1;

                SqlParameter Password = new SqlParameter
                {
                    ParameterName = "@Password",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = DEmpleados.Variables[contador].Trim()
                };
                SqlCmd.Parameters.Add(Password);

                //Ejecutamos nuestro comando
                //Se puede ejecutar este metodo pero ya tenemos el mensaje que devuelve sql
                rpta = SqlCmd.ExecuteNonQuery() >= 1 ? "OK" : "NO se Ingreso el Registro";

                if (rpta != "OK")
                {
                    rpta = this.Mensaje_respuesta;
                }
            }
            //Mostramos posible error que tengamos
            catch (SqlException ex)
            {
                rpta = ex.Message;
            }
            finally
            {
                //Si la cadena SqlCon esta abierta la cerramos
                if (SqlCon.State == ConnectionState.Open)
                    SqlCon.Close();
            }
            return rpta;
        }
        #endregion

        #region METODO EDITAR EMPLEADOS
        public string EditarEmpleado(DEmpleados DEmpleados)
        {
            int contador = 0;
            //asignamos a una cadena string la variable rpta y la iniciamos en vacía
            string rpta = "";
            SqlConnection SqlCon = new SqlConnection();
            SqlCon.InfoMessage += new SqlInfoMessageEventHandler(SqlCon_InfoMessage);
            SqlCon.FireInfoMessageEventOnUserErrors = true;
            //Capturador de errores
            try
            {
                SqlCon.ConnectionString = Conexion.Cn;
                SqlCon.Open();
                //establecer comando
                SqlCommand SqlCmd = new SqlCommand();
                SqlCmd.Connection = SqlCon;
                SqlCmd.CommandText = "sp_Editar_empleado";
                //Indicamos que es un procedimiento almacenado
                SqlCmd.CommandType = CommandType.StoredProcedure;

                SqlParameter Id_agendamiento = new SqlParameter();
                Id_agendamiento.ParameterName = "@Id_empleado";
                Id_agendamiento.SqlDbType = SqlDbType.Int;
                Id_agendamiento.Value = DEmpleados.Variables[contador].Trim();
                SqlCmd.Parameters.Add(Id_agendamiento);
                contador += 1;

                SqlParameter Nombre_empleado = new SqlParameter();
                Nombre_empleado.ParameterName = "@Nombre_empleado";
                Nombre_empleado.SqlDbType = SqlDbType.VarChar;
                Nombre_empleado.Size = 500;
                Nombre_empleado.Value = DEmpleados.Variables[contador].Trim().ToUpper();
                SqlCmd.Parameters.Add(Nombre_empleado);
                contador += 1;

                SqlParameter Telefono_empleado = new SqlParameter();
                Telefono_empleado.ParameterName = "@Telefono_empleado";
                Telefono_empleado.SqlDbType = SqlDbType.VarChar;
                Telefono_empleado.Size = 50;
                Telefono_empleado.Value = DEmpleados.Variables[contador].Trim().ToUpper();
                SqlCmd.Parameters.Add(Telefono_empleado);
                contador += 1;

                SqlParameter Correo_electronico = new SqlParameter();
                Correo_electronico.ParameterName = "@Correo_electronico";
                Correo_electronico.SqlDbType = SqlDbType.VarChar;
                Correo_electronico.Size = 500;
                Correo_electronico.Value = DEmpleados.Variables[contador].Trim().ToUpper();
                SqlCmd.Parameters.Add(Correo_electronico);
                contador += 1;

                SqlParameter Cargo_empleado = new SqlParameter();
                Cargo_empleado.ParameterName = "@Cargo_empleado";
                Cargo_empleado.SqlDbType = SqlDbType.VarChar;
                Cargo_empleado.Size = 50;
                Cargo_empleado.Value = DEmpleados.Variables[contador].Trim().ToUpper();
                SqlCmd.Parameters.Add(Cargo_empleado);
                contador += 1;

                SqlParameter Password = new SqlParameter();
                Password.ParameterName = "@Password";
                Password.SqlDbType = SqlDbType.VarChar;
                Password.Size = 50;
                Password.Value = DEmpleados.Variables[contador].Trim();
                SqlCmd.Parameters.Add(Password);

                //Ejecutamos nuestro comando
                //Se puede ejecutar este metodo pero ya tenemos el mensaje que devuelve sql
                rpta = SqlCmd.ExecuteNonQuery() >= 1 ? "OK" : "NO se Ingreso el Registro";

                if (rpta != "OK")
                {
                    rpta = this.Mensaje_respuesta;
                }
            }
            //Mostramos posible error que tengamos
            catch (SqlException ex)
            {
                rpta = ex.Message;
            }
            finally
            {
                //Si la cadena SqlCon esta abierta la cerramos
                if (SqlCon.State == ConnectionState.Open)
                    SqlCon.Close();
            }
            return rpta;
        }
        #endregion

        #region METODO BUSCAR EMPLEADOS
        public static DataTable BuscarEmpleados(string tipo_busqueda, string texto_busqueda, 
            out string rpta)
        {

            StringBuilder consulta = new StringBuilder();
            consulta.Append("SELECT * FROM Empleados ");
            if (tipo_busqueda.Equals("BUSQUEDA COMPLETA ACTIVO"))
            {
                consulta.Append("WHERE Nombre_empleado like '%" + texto_busqueda + "%' OR " +
                    "Telefono_empleado like '%" + texto_busqueda + "%' and " +
                    "Estado = 'ACTIVO' ");
            }
            else if (tipo_busqueda.Equals("COMPLETO ACTIVO"))
            {
                consulta.Append("WHERE Estado = 'ACTIVO' ");
            }
            else if (tipo_busqueda.Equals("BUSQUEDA COMPLETA INACTIVO"))
            {
                consulta.Append("WHERE Nombre_empleado like '%" + texto_busqueda + "%' OR " +
                    "Telefono_empleado like '%" + texto_busqueda + "%' and " +
                    "Estado = 'INACTIVO' ");
            }
            else if (tipo_busqueda.Equals("COMPLETO INACTIVO"))
            {
                consulta.Append("WHERE Estado = 'INACTIVO' ");
            }

            consulta.Append("ORDER BY Id_empleado DESC");
            return Conexion.EjecutarConsultaDt(Convert.ToString(consulta), out rpta);
        }

        #endregion

        #region METODO LOGIN
        public static DataTable Login(string tipo_busqueda, string texto_busqueda1, 
            string texto_busqueda2, out string rpta)
        {
            rpta = "OK";
            DataTable DtResultado = new DataTable("Login");
            SqlConnection SqlCon = new SqlConnection();
            try
            {
                SqlCon.ConnectionString = Conexion.Cn;
                SqlCommand Sqlcmd = new SqlCommand
                {
                    Connection = SqlCon,
                    CommandText = "sp_Login",
                    CommandType = CommandType.StoredProcedure
                };

                SqlParameter Tipo_busqueda = new SqlParameter
                {
                    ParameterName = "@Tipo_busqueda",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = tipo_busqueda.Trim().ToUpper()
                };
                Sqlcmd.Parameters.Add(Tipo_busqueda);

                SqlParameter Texto_busqueda1 = new SqlParameter
                {
                    ParameterName = "@Texto_busqueda1",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = texto_busqueda1.Trim().ToUpper()
                };
                Sqlcmd.Parameters.Add(Texto_busqueda1);

                SqlParameter Texto_busqueda2 = new SqlParameter
                {
                    ParameterName = "@Texto_busqueda2",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = texto_busqueda2.Trim().ToUpper()
                };
                Sqlcmd.Parameters.Add(Texto_busqueda2);

                SqlDataAdapter SqlData = new SqlDataAdapter(Sqlcmd);
                SqlData.Fill(DtResultado);

                if (DtResultado.Rows.Count < 1)
                {
                    DtResultado = null;
                    rpta = "";
                }
            }
            catch (SqlException ex)
            {
                DtResultado = null;
                rpta = ex.Message;
            }
            catch (Exception ex)
            {
                DtResultado = null;
                rpta = ex.Message;
            }

            return DtResultado;
        }

        #endregion

        #region METODO CLAVE MAESTRA
        public string ActualizarClaveMaestra(int id_empleado, int clave_maestra)
        {
            string consulta = "UPDATE Empleados SET Codigo_maestro = '" + clave_maestra + "' " +
                "WHERE Id_empleado = '" + id_empleado + "'";
            return Conexion.EjecutarConsultaCadena(consulta, false);
        }
        #endregion

        #region METODO CONTRASEÑA USUARIO
        public string ActualizarPassword(int id_empleado, string pass)
        {
            string consulta = "UPDATE Empleados SET Password = '" + pass + "' " +
                "WHERE Id_empleado = '" + id_empleado + "'";
            return Conexion.EjecutarConsultaCadena(consulta, false);
        }
        #endregion

        #region METODO INACTIVAR EMPLEADO
        public string InactivarEmpleado(int id_empleado)
        {
            string consulta = "UPDATE Empleados SET Estado = 'INACTIVO' " +
                "WHERE Id_empleado = '" + id_empleado + "'";
            return Conexion.EjecutarConsultaCadena(consulta, false);
        }
        #endregion

        #region METODO ACTIVAR EMPLEADO
        public string ActivarEmpleado(int id_empleado)
        {
            string consulta = "UPDATE Empleados SET Estado = 'ACTIVO' " +
                "WHERE Id_empleado = '" + id_empleado + "'";
            return Conexion.EjecutarConsultaCadena(consulta, false);
        }
        #endregion
    }
}
