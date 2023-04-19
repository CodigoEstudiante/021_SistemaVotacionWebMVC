using System.Web;
using System.Web.Mvc;
using ProyectoWeb.Filters;

namespace ProyectoWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new VerificarSesion());
        }
    }
}
