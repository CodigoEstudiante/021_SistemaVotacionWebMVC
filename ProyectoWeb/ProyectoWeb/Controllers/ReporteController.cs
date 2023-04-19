using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoWeb.Controllers
{
    public class ReporteController : Controller
    {
        // GET: Reporte
        public ActionResult Votantes()
        {
            return View();
        }

        public ActionResult Candidatos() {

            return View();
        }


    }
}