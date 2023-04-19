using CapaDatos;
using CapaModelo;
using ProyectoWeb.Utilidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VotacionWeb.Controllers
{
    public class VotacionController : Controller
    {
        private static Votante SesionVotante;
        // GET: Votacion
        public ActionResult Ingreso()
        {
            return View();
        }

        public ActionResult Elecciones() {

            if(Session["Votante"] == null)
                return RedirectToAction("Ingreso", "Votacion");

            SesionVotante = (Votante)Session["Votante"];
            return View();
        }

        public ActionResult Candidatos()
        {
            if (Session["Votante"] == null)
                return RedirectToAction("Ingreso", "Votacion");

            SesionVotante = (Votante)Session["Votante"];
            return View();
        }

        [HttpGet]
        public JsonResult ObtenerElecciones() {

            List<Votante> votantes = CD_Votante.Obtener().Where(v => v.DocumentoIdentidad == SesionVotante.DocumentoIdentidad).ToList();
            List<Eleccion> elecciones = CD_Eleccion.Obtener().Where(e => e.Activo == true).ToList();

            List<Eleccion> oLista;
  
            if (votantes != null && elecciones != null)
            {
                oLista = (from v in votantes
                          join e in elecciones on v.oEleccion.IdEleccion equals e.IdEleccion
                          where v.EmitioVotacion == false
                          select new Eleccion()
                          {
                              IdEleccion = e.IdEleccion,
                              Descripcion = e.Descripcion,
                              Cargo = e.Cargo,
                              FechaRegistro = e.FechaRegistro,
                              oUsuario = e.oUsuario,
                              Activo = e.Activo
                          }).ToList();

            }
            else {

                oLista = new List<Eleccion>();
            }


            //List<Eleccion> oLista = CD_Eleccion.Obtener().Where(x => x.IdEleccion == SesionVotante.oEleccion.IdEleccion && x.Activo == true).ToList();

            return Json(oLista, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerCandidatos(int ideleccion)
        {

            List<Candidato> oLista = CD_Candidato.Obtener().Where(x => x.oEleccion.IdEleccion == ideleccion).ToList();

            oLista = (from row in oLista
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
                          Extension = Path.GetExtension(row.RutaImagen).Replace(".", "")
                      }).ToList();

            return Json(oLista, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult TerminarVotacion(int ideleccion, int idcandidato)
        {
            Votacion objeto = new Votacion()
            {
                IdEleccion = ideleccion,
                IdCandidato = idcandidato,
                DocumentoIdentidad = SesionVotante.DocumentoIdentidad
            };

            bool respuesta = CD_Votacion.Registrar(objeto);

            Session["Votante"] = null;
            return Json(new { resultado = respuesta },JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Ingreso(string documentoidentidad)
        {

            List<Votante> votantes = CD_Votante.Obtener().Where(v => v.DocumentoIdentidad == documentoidentidad).ToList();

            if (votantes == null)
            {
                ViewBag.Mensaje = "No emite voto";
                return View();
            }
            else {
                if (votantes.Where(v => v.EmitioVotacion == false).Count() == 0) {
                    ViewBag.Mensaje = "Usted ya emitió su voto";
                    return View();
                }

            }

            Session["Votante"] = votantes[0];

            return RedirectToAction("Elecciones", "Votacion");
        }


    }
}