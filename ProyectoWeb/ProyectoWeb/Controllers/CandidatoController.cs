using CapaDatos;
using CapaModelo;
using Newtonsoft.Json;
using ProyectoWeb.Utilidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ProyectoWeb.Controllers
{
    public class CandidatoController : Controller
    {
        private static Usuario SesionUsuario;
        // GET: Candidato
        public ActionResult Index()
        {
            SesionUsuario = (Usuario)Session["Usuario"];
            return View();
        }

        [HttpGet]
        public JsonResult Obtener(int ideleccion)
        {
            List<Candidato> olista = CD_Candidato.Obtener().Where(x => x.oEleccion.IdEleccion == ideleccion).ToList();

            olista = (from row in olista
                      select new Candidato()
                      {
                          IdCandidato = row.IdCandidato,
                          NombresCompleto = row.NombresCompleto,
                          Mensaje = row.Mensaje,
                          RutaImagen = row.RutaImagen,
                          oEleccion = row.oEleccion,
                          oUsuario = row.oUsuario,
                          FechaRegistro = row.FechaRegistro,
                          Activo = row.Activo,
                          ImagenBase64 = Converter.toBase64String(row.RutaImagen),
                          Extension = Path.GetExtension(row.RutaImagen).Replace(".","")
                      }).ToList();

            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Guardar(string objeto , HttpPostedFileBase imagenArchivo)
        {

            Response oresponse = new Response() { resultado = true, mensaje = "" };

            try
            {
                Candidato oCandidato = new Candidato();
                oCandidato = JsonConvert.DeserializeObject<Candidato>(objeto);

                string GuardarEnRuta = (CD_Configuracion.Obtener().Where(x => x.IdConfiguracion == 1).FirstOrDefault()).Valor;
                

                if (!Directory.Exists(GuardarEnRuta))
                    Directory.CreateDirectory(GuardarEnRuta);


                if (oCandidato.IdCandidato == 0)
                {
                    oCandidato.oUsuario = SesionUsuario;

                    int idCandidato_respuesta = CD_Candidato.Registrar(oCandidato);
                    
                    oCandidato.IdCandidato = idCandidato_respuesta;

                    oresponse.resultado = oCandidato.IdCandidato == 0 ? false : true;

                }
                else
                {
                    oresponse.resultado = CD_Candidato.Modificar(oCandidato);
                }


                //GUARDAMOS IMAGEN SI NO ES NULL Y SI IDCANDIDATO ES DIFERENTE DE CERO
                if (imagenArchivo != null && oCandidato.IdCandidato != 0)
                {
                    string extension = Path.GetExtension(imagenArchivo.FileName);
                    GuardarEnRuta = GuardarEnRuta + oCandidato.IdCandidato.ToString() + extension;
                    oCandidato.RutaImagen = GuardarEnRuta;

                    imagenArchivo.SaveAs(GuardarEnRuta);

                    oresponse.resultado = CD_Candidato.ActualizarRutaImagen(oCandidato);

                }


            }
            catch(Exception e) {
                oresponse.resultado = false;
                oresponse.mensaje = e.Message;
            }



            return Json(oresponse, JsonRequestBehavior.AllowGet);
        }


    }
}