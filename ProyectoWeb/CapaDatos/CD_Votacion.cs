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
    public class CD_Votacion
    {
        public static List<Resultado> ObtenerResultados(int ideleccion)
        {
            List<Resultado> Lista = new List<Resultado>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand("usp_ObtenerResultados", oConexion);
                cmd.Parameters.AddWithValue("IdEleccion", ideleccion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        Lista.Add(new Resultado()
                        {
                            Votos = Convert.ToInt32(dr["Votos"].ToString()),
                            NombresCompleto = dr["NombresCompleto"].ToString(),
                            RutaImagen = dr["RutaImagen"].ToString(),
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


        public static bool Registrar(Votacion objeto)
        {

            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_RegistrarVotacion", oConexion);
                    cmd.Parameters.AddWithValue("IdEleccion", objeto.IdEleccion);
                    cmd.Parameters.AddWithValue("DocumentoIdentidad", objeto.DocumentoIdentidad);
                    cmd.Parameters.AddWithValue("IdCandidato", objeto.IdCandidato);
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
