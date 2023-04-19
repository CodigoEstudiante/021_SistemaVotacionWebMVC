using System.Web;
using System.Web.Optimization;

namespace ProyectoWeb
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // para la producción, use la herramienta de compilación disponible en https://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css"
                      ));


            bundles.Add(new StyleBundle("~/Content/PluginsCSS").Include(

                    //FUENTE FONTAWESOME
                   "~/Content/Plugins/fontawesome-free-5.15.2/css/all.min.css",

                   //SWEET ALERT
                   "~/Content/Plugins/sweetalert2/css/sweetalert.css",


                    //DATATABLE
                    "~/content/plugins/datatables/css/jquery.datatables.min.css",
                    "~/Content/Plugins/datatables/css/responsive.dataTables.min.css"

                    


                   ));

            bundles.Add(new StyleBundle("~/Content/PluginsJS").Include(

                    //FUENTE FONTAWESOME,
                    "~/Content/Plugins/fontawesome-free-5.15.2/js/all.min.js",


                   //SWEET ALERT
                   "~/Content/Plugins/sweetalert2/js/sweetalert.js",

                   //DATATABLE JS
                   "~/Content/Plugins/datatables/js/jquery.dataTables.min.js",
                   "~/Content/Plugins/datatables/js/dataTables.responsive.min.js",

                   //LOADING OVERLAY
                   "~/Content/Plugins/jquery-loading-overlay/loadingoverlay.min.js",

                   //ECHARTS
                   "~/Content/Plugins/Echarts/js/echarts.js",

                   "~/Scripts/jquery.validate.min.js"
                    ));

        }
    }
}
