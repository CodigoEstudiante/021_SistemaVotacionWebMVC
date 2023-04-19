using CapaModelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Votante
    {

        public static List<Votante> Obtener()
        {
            List<Votante> Lista = new List<Votante>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand("usp_ObtenerVotantes", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        Lista.Add(new Votante()
                        {
                            IdVotante = Convert.ToInt32(dr["IdVotante"].ToString()),
                            DocumentoIdentidad = dr["DocumentoIdentidad"].ToString(),
                            Nombres = dr["Nombres"].ToString(),
                            Apellidos = dr["Apellidos"].ToString(),
                            Activo = Convert.ToBoolean(dr["Activo"]),
                            EmitioVotacion = Convert.ToBoolean(dr["EmitioVotacion"]),
                            FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"].ToString()),
                            oEleccion = new Eleccion()
                            {
                                IdEleccion = Convert.ToInt32(dr["IdEleccion"].ToString()),
                            },
                            oUsuario = new Usuario()
                            {
                                IdUsuario = Convert.ToInt32(dr["IdUsuario"].ToString()),
                                Correo = dr["Correo"].ToString()
                            }
                        });
                    }
                    dr.Close();

                    return Lista;

                }
                catch (Exception ex)
                {
                    Lista = null;
                    return Lista;
                }
            }
        }


        public static List<Votante> CargarVotantes(DataTable tabla)
        {
            List<Votante> Lista = new List<Votante>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand("usp_cargarVotantes", oConexion);
                cmd.Parameters.Add("EstructuraCarga", SqlDbType.Structured).Value = tabla;
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        Lista.Add(new Votante()
                        {
                            Estado = Convert.ToBoolean(dr["Estado"]),
                            Mensaje = dr["Mensaje"].ToString(),
                            DocumentoIdentidad = dr["DocumentoIdentidad"].ToString(),
                            Nombres = dr["Nombres"].ToString(),
                            Apellidos = dr["Apellidos"].ToString()
                        });
                    }
                    dr.Close();

                    return Lista;

                }
                catch (Exception ex)
                {
                    Lista = null;
                    return Lista;
                }
            }
        }



        public static bool Modificar(Votante objeto)
        {

            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_ModificarVotante", oConexion);
                    cmd.Parameters.AddWithValue("IdVotante", objeto.IdVotante);
                    cmd.Parameters.AddWithValue("DocumentoIdentidad", objeto.DocumentoIdentidad);
                    cmd.Parameters.AddWithValue("Nombres", objeto.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", objeto.Apellidos);
                    cmd.Parameters.AddWithValue("Activo", objeto.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oConexion.Open();
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);

                }
                catch (Exception ex)
                {
                    respuesta = false;
                }

            }

            return respuesta;

        }

    }
}
