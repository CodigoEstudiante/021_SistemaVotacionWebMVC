using CapaDatos;
using CapaModelo;
using ProyectoWeb.Utilidades;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoWeb.Controllers
{
    public class DashBoardController : Controller
    {
        // GET: DashBoard
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult ObtenerResultados(int ideleccion)
        {
            List<Resultado> olista = CD_Votacion.ObtenerResultados(ideleccion);

            olista = (from row in olista
                      select new Resultado()
                      {
                          Votos = row.Votos,
                          NombresCompleto = row.NombresCompleto,
                          RutaImagen = row.RutaImagen,
                          Imagen64 = Converter.toBase64String(row.RutaImagen),
                          Extension = Path.GetExtension(row.RutaImagen).Replace(".", "")
                      }).ToList();

            return Json( olista , JsonRequestBehavior.AllowGet);
        }

        public ActionResult Salir()
        {
            Session["Usuario"] = null;
            return RedirectToAction("Index", "Login");
        }

    }
}