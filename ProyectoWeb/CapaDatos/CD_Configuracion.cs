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
    public class CD_Configuracion
    {

        public static List<Configuracion> Obtener()
        {
            List<Configuracion> Lista = new List<Configuracion>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("usp_ObtenerConfiguraciones", oConexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        Lista.Add(new Configuracion()
                        {
                            IdConfiguracion = Convert.ToInt32(dr["IdConfiguracion"].ToString()),
                            Descripcion = dr["Descripcion"].ToString(),
                            Valor = dr["Valor"].ToString()
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

    }
}
