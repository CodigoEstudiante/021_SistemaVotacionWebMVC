using CapaDatos;
using CapaModelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoWeb.Controllers
{
    public class EleccionController : Controller
    {
        private static Usuario SesionUsuario;

        // GET: Eleccion
        public ActionResult Index()
        {
            SesionUsuario = (Usuario)Session["Usuario"];

            return View();
        }

        public JsonResult Obtener()
        {
            List<Eleccion> olista = CD_Eleccion.Obtener();
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Guardar(Eleccion objeto)
        {
            bool respuesta = false;

            if (objeto.IdEleccion == 0)
            {
                objeto.oUsuario = SesionUsuario;

                respuesta = CD_Eleccion.Registrar(objeto);
            }
            else
            {
                respuesta = CD_Eleccion.Modificar(objeto);
            }


            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }

    }
}