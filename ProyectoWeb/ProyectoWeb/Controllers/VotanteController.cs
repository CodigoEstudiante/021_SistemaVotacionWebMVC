using CapaDatos;
using CapaModelo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoWeb.Controllers
{
    public class VotanteController : Controller
    {
        private static Usuario SesionUsuario;
        // GET: Votante
        public ActionResult Index()
        {
            SesionUsuario = (Usuario)Session["Usuario"];
            return View();
        }

        [HttpGet]
        public JsonResult Obtener(int ideleccion)
        {
            List<Votante> olista = CD_Votante.Obtener().Where(x => x.oEleccion.IdEleccion == ideleccion).ToList();

            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult CargarExcel(string ideleccion, HttpPostedFileBase excelArchivo)
        {
            List<Votante> olista = new List<Votante>();

            try
            {
                //CREAMOS LA ESTRUCTURA DE LA TABLA
                DataTable tabla = new DataTable();
                tabla.Columns.Add("DocumentoIdentidad", typeof(string));
                tabla.Columns.Add("Nombres", typeof(string));
                tabla.Columns.Add("Apellidos", typeof(string));
                tabla.Columns.Add("IdEleccion", typeof(string));
                tabla.Columns.Add("IdUsuarioRegistro", typeof(string));


                //GUARDAMOS EL EXCEL EN EL PROYECTO LOCAL PARA LEERLO DESPUÉS
                string rutaGuardado = Server.MapPath("~/Temporal/");
                if (!Directory.Exists(rutaGuardado))
                    Directory.CreateDirectory(rutaGuardado);

                rutaGuardado = rutaGuardado  + SesionUsuario.IdUsuario + DateTime.Now.Minute.ToString() + Path.GetExtension(excelArchivo.FileName);
                excelArchivo.SaveAs(rutaGuardado);


                //LEEMOS EL ARCHIVO EXCEL GUARDADO ANTERIORMENTE
                IWorkbook miExcel = null;
                FileStream fs = new FileStream(rutaGuardado, FileMode.Open, FileAccess.Read);

                if (Path.GetExtension(rutaGuardado) == ".xlsx")
                    miExcel = new XSSFWorkbook(fs);
                else
                    miExcel = new HSSFWorkbook(fs);

                ISheet PrimeraHoja = miExcel.GetSheetAt(0);

                if (PrimeraHoja != null) {
                    int cantidad_filas = PrimeraHoja.LastRowNum;

                    for (int i = 1; i<= cantidad_filas;i++) {

                        IRow fila = PrimeraHoja.GetRow(i);

                        tabla.Rows.Add(
                            fila.GetCell(0, MissingCellPolicy.RETURN_NULL_AND_BLANK) != null ? fila.GetCell(0).StringCellValue : "",
                            fila.GetCell(1, MissingCellPolicy.RETURN_NULL_AND_BLANK) != null ? fila.GetCell(1).StringCellValue : "",
                            fila.GetCell(2, MissingCellPolicy.RETURN_NULL_AND_BLANK) != null ? fila.GetCell(2).StringCellValue : "",
                            ideleccion,
                            SesionUsuario.IdUsuario.ToString()
                            );
                    }

                }

                olista = CD_Votante.CargarVotantes(tabla);

                fs.Dispose();
                fs.Close();

                System.IO.File.Delete(rutaGuardado);

            }
            catch {

                olista = new List<Votante>();
            }

            

            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Guardar(Votante objeto)
        {
            bool respuesta = false;

            if (objeto.IdVotante != 0)
            {
                respuesta = CD_Votante.Modificar(objeto);
            }

            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }


    }
}