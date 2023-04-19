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
    public class CD_Candidato
    {
        public static List<Candidato> Obtener()
        {
            List<Candidato> Lista = new List<Candidato>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand("usp_ObtenerCandidatos", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        Lista.Add(new Candidato()
                        {
                            IdCandidato = Convert.ToInt32(dr["IdCandidato"].ToString()),
                            NombresCompleto = dr["NombresCompleto"].ToString(),
                            Mensaje = dr["Mensaje"].ToString(),
                            RutaImagen = dr["RutaImagen"].ToString(),
                            Activo = Convert.ToBoolean(dr["Activo"]),
                            FechaRegistro = Convert.ToDateTime(dr["FechaRegistro"].ToString()),
                            oEleccion = new Eleccion() {
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


        public static int Registrar(Candidato objeto)
        {

            int respuesta = 0;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_RegistrarCandidato", oConexion);
                    cmd.Parameters.AddWithValue("NombresCompleto", objeto.NombresCompleto);
                    cmd.Parameters.AddWithValue("Mensaje", objeto.Mensaje);
                    cmd.Parameters.AddWithValue("IdEleccion", objeto.oEleccion.IdEleccion);
                    cmd.Parameters.AddWithValue("IdUsuarioRegistro", objeto.oUsuario.IdUsuario);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oConexion.Open();
                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(cmd.Parameters["Resultado"].Value);

                }
                catch (Exception ex)
                {
                    respuesta = 0;
                }

            }

            return respuesta;

        }

        public static bool Modificar(Candidato objeto)
        {

            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_ModificarCandidato", oConexion);
                    cmd.Parameters.AddWithValue("IdCandidato", objeto.IdCandidato);
                    cmd.Parameters.AddWithValue("NombresCompleto", objeto.NombresCompleto);
                    cmd.Parameters.AddWithValue("Mensaje", objeto.Mensaje);
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

        public static bool ActualizarRutaImagen(Candidato objeto)
        {

            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_ActualizarRutaImagenCandidato", oConexion);
                    cmd.Parameters.AddWithValue("IdCandidato", objeto.IdCandidato);
                    cmd.Parameters.AddWithValue("RutaImagen", objeto.RutaImagen);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oConexion.Open();
                    cmd.ExecuteNonQuery();

                    respuesta = true;

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
