﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using CapaEntidades.Models;

namespace CapaDatos
{
    public class DNomina
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
        #endregion

        #region CONSTRUCTOR VACIO
        public DNomina() { }
        #endregion

        #region METODO INSERTAR NOMINA
        public async Task<(string rpta, int id_empleado)> InsertarNomina(EmpleadoNominaBinding empleadoNomina)
        {
            int contador = 0;
            int id_nomina = 0;
            string consulta = "INSERT INTO Nomina_empleado VALUES (@Id_empleado, @Fecha_nomina, @Salario, " +
                "@Propinas, @Otros_ingresos, @Egresos, @Total_nomina, @Estado_nomina) " +
                "SET @Id_nomina_empleado = SCOPE_IDENTITY(); ";

            //asignamos a una cadena string la variable rpta y la iniciamos en vacía
            string rpta = "";
            SqlConnection SqlCon = new SqlConnection();
            SqlCon.InfoMessage += new SqlInfoMessageEventHandler(SqlCon_InfoMessage);
            SqlCon.FireInfoMessageEventOnUserErrors = true;
            //Capturador de errores
            try
            {
                SqlCon.ConnectionString = Conexion.Cn;
                await SqlCon.OpenAsync();
                //Establecer comando
                SqlCommand SqlCmd = new SqlCommand
                {
                    Connection = SqlCon,
                    CommandText = consulta,
                    CommandType = CommandType.Text,
                };

                SqlParameter Id_nomina_empleado = new SqlParameter
                {
                    ParameterName = "@Id_nomina_empleado",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output,
                };
                SqlCmd.Parameters.Add(Id_nomina_empleado);

                SqlParameter Id_empleado = new SqlParameter
                {
                    ParameterName = "@Id_empleado",
                    SqlDbType = SqlDbType.Int,
                    Value = empleadoNomina.Id_empleado,
                };
                SqlCmd.Parameters.Add(Id_empleado);

                SqlParameter Fecha_nomina = new SqlParameter
                {
                    ParameterName = "@Fecha_nomina",
                    SqlDbType = SqlDbType.Date,
                    Value = empleadoNomina.Fecha_nomina,
                };
                SqlCmd.Parameters.Add(Fecha_nomina);
                contador += 1;

                SqlParameter Salario = new SqlParameter
                {
                    ParameterName = "@Salario",
                    SqlDbType = SqlDbType.Decimal,
                    Value = empleadoNomina.Salario,
                };
                SqlCmd.Parameters.Add(Salario);
                contador += 1;

                SqlParameter Propinas = new SqlParameter
                {
                    ParameterName = "@Propinas",
                    SqlDbType = SqlDbType.Decimal,
                    Value = empleadoNomina.Propinas,
                };
                SqlCmd.Parameters.Add(Propinas);
                contador += 1;

                SqlParameter Otros_ingresos = new SqlParameter
                {
                    ParameterName = "@Otros_ingresos",
                    SqlDbType = SqlDbType.Decimal,
                    Value = empleadoNomina.Salario,
                };
                SqlCmd.Parameters.Add(Otros_ingresos);
                contador += 1;

                SqlParameter Egresos = new SqlParameter
                {
                    ParameterName = "@Egresos",
                    SqlDbType = SqlDbType.Decimal,
                    Value = empleadoNomina.Egresos,
                };
                SqlCmd.Parameters.Add(Egresos);
                contador += 1;

                SqlParameter Total_nomina = new SqlParameter
                {
                    ParameterName = "@Total_nomina",
                    SqlDbType = SqlDbType.Decimal,
                    Value = empleadoNomina.Total_nomina,
                };
                SqlCmd.Parameters.Add(Total_nomina);
                contador += 1;
                
                SqlParameter Estado_nomina = new SqlParameter
                {
                    ParameterName = "@Estado_nomina",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = empleadoNomina.Estado_nomina,
                };
                SqlCmd.Parameters.Add(Estado_nomina);

                //Ejecutamos nuestro comando
                //Se puede ejecutar este metodo pero ya tenemos el mensaje que devuelve sql
                rpta = SqlCmd.ExecuteNonQuery() >= 1 ? "OK" : "NO se Ingreso el Registro";

                if (!rpta.Equals("OK"))
                {
                    if (this.Mensaje_respuesta != null)
                    {
                        rpta = this.Mensaje_respuesta;
                    }
                }
                else
                {
                    id_nomina = Convert.ToInt32(SqlCmd.Parameters["@Id_nomina_empleado"].Value);
                }
            }
            //Mostramos posible error que tengamos
            catch (SqlException ex)
            {
                rpta = ex.Message;
            }
            catch(Exception ex)
            {
                rpta = ex.Message;
            }
            finally
            {
                //Si la cadena SqlCon esta abierta la cerramos
                if (SqlCon.State == ConnectionState.Open)
                    SqlCon.Close();
            }
            return (rpta, id_nomina);
        }
        #endregion

        #region METODO EDITAR NOMINA
        public async Task<string> EditarNomina(int id_nomina_empleado, EmpleadoNominaBinding empleadoNomina)
        {
            int contador = 0;
            string consulta = "UPDATE Nomina_empleado SET " +
                "Id_empleado = @Id_empleado, " +
                "Fecha_nomina = @Fecha_nomina, " +
                "Salario = @Salario, " +
                "Propinas = @Propinas, " +
                "Otros_ingresos = @Otros_ingresos, " +
                "Egresos = @Egresos, " +
                "Total_nomina = @Total_nomina, " +
                "Estado_nomina = @Estado_nomina " +
                "WHERE Id_nomina_empleado = @Id_nomina_empleado ";

            //asignamos a una cadena string la variable rpta y la iniciamos en vacía
            string rpta = "";
            SqlConnection SqlCon = new SqlConnection();
            SqlCon.InfoMessage += new SqlInfoMessageEventHandler(SqlCon_InfoMessage);
            SqlCon.FireInfoMessageEventOnUserErrors = true;
            //Capturador de errores
            try
            {
                SqlCon.ConnectionString = Conexion.Cn;
                await SqlCon.OpenAsync();
                //Establecer comando
                SqlCommand SqlCmd = new SqlCommand
                {
                    Connection = SqlCon,
                    CommandText = consulta,
                    CommandType = CommandType.Text,
                };

                SqlParameter Id_nomina_empleado = new SqlParameter
                {
                    ParameterName = "@Id_nomina_empleado",
                    SqlDbType = SqlDbType.Int,
                    Value = id_nomina_empleado,
                };
                SqlCmd.Parameters.Add(Id_nomina_empleado);

                SqlParameter Id_empleado = new SqlParameter
                {
                    ParameterName = "@Id_empleado",
                    SqlDbType = SqlDbType.Int,
                    Value = empleadoNomina.Id_empleado,
                };
                SqlCmd.Parameters.Add(Id_empleado);

                SqlParameter Fecha_nomina = new SqlParameter
                {
                    ParameterName = "@Fecha_nomina",
                    SqlDbType = SqlDbType.Date,
                    Value = empleadoNomina.Fecha_nomina,
                };
                SqlCmd.Parameters.Add(Fecha_nomina);
                contador += 1;

                SqlParameter Salario = new SqlParameter
                {
                    ParameterName = "@Salario",
                    SqlDbType = SqlDbType.Decimal,
                    Value = empleadoNomina.Salario,
                };
                SqlCmd.Parameters.Add(Salario);
                contador += 1;

                SqlParameter Propinas = new SqlParameter
                {
                    ParameterName = "@Propinas",
                    SqlDbType = SqlDbType.Decimal,
                    Value = empleadoNomina.Propinas,
                };
                SqlCmd.Parameters.Add(Propinas);
                contador += 1;

                SqlParameter Otros_ingresos = new SqlParameter
                {
                    ParameterName = "@Otros_ingresos",
                    SqlDbType = SqlDbType.Decimal,
                    Value = empleadoNomina.Salario,
                };
                SqlCmd.Parameters.Add(Otros_ingresos);
                contador += 1;

                SqlParameter Egresos = new SqlParameter
                {
                    ParameterName = "@Egresos",
                    SqlDbType = SqlDbType.Decimal,
                    Value = empleadoNomina.Egresos,
                };
                SqlCmd.Parameters.Add(Egresos);
                contador += 1;

                SqlParameter Total_nomina = new SqlParameter
                {
                    ParameterName = "@Total_nomina",
                    SqlDbType = SqlDbType.Decimal,
                    Value = empleadoNomina.Total_nomina,
                };
                SqlCmd.Parameters.Add(Total_nomina);
                contador += 1;

                SqlParameter Password = new SqlParameter
                {
                    ParameterName = "@Estado_nomina",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = empleadoNomina.Estado_nomina,
                };
                SqlCmd.Parameters.Add(Password);

                //Ejecutamos nuestro comando
                //Se puede ejecutar este metodo pero ya tenemos el mensaje que devuelve sql
                rpta = SqlCmd.ExecuteNonQuery() >= 1 ? "OK" : "NO se actualizó el registro";

                if (!rpta.Equals("OK"))
                {
                    if (this.Mensaje_respuesta != null)
                    {
                        rpta = this.Mensaje_respuesta;
                    }
                }
            }
            //Mostramos posible error que tengamos
            catch (SqlException ex)
            {
                rpta = ex.Message;
            }
            catch (Exception ex)
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

        #region METODO BUSCAR NOMINA
        public async Task<(string rpta, DataTable dtNominaEmpleado)> BuscarNomina(string tipo_busqueda, string texto_busqueda)
        {
            string rpta = "OK";
            DataTable dtNomina = new DataTable("NominaEmpleado");
            SqlConnection SqlCon = new SqlConnection();
            SqlCon.InfoMessage += new SqlInfoMessageEventHandler(SqlCon_InfoMessage);
            SqlCon.FireInfoMessageEventOnUserErrors = true;
            try
            {
                SqlCon.ConnectionString = Conexion.Cn;
                await SqlCon.OpenAsync();
                SqlCommand Sqlcmd = new SqlCommand
                {
                    Connection = SqlCon,
                    CommandText = "sp_Buscar_nomina",
                    CommandType = CommandType.StoredProcedure,
                };

                SqlParameter Tipo_busqueda = new SqlParameter
                {
                    ParameterName = "@Tipo_busqueda",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = tipo_busqueda.Trim(),
                };
                Sqlcmd.Parameters.Add(Tipo_busqueda);

                SqlParameter Texto_busqueda = new SqlParameter
                {
                    ParameterName = "@Texto_busqueda",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 50,
                    Value = texto_busqueda.Trim(),
                };
                Sqlcmd.Parameters.Add(Texto_busqueda);
            
                SqlDataAdapter SqlData = new SqlDataAdapter(Sqlcmd);
                await Task.Run(() => SqlData.Fill(dtNomina));                
            }
            catch (SqlException ex)
            {
                rpta = ex.Message;
            }
            catch (Exception ex)
            {
                rpta = ex.Message;
            }
            finally
            {
                if (SqlCon.State == ConnectionState.Open)
                    SqlCon.Close();
            }

            return (rpta, dtNomina);
        }

        #endregion
    }
}