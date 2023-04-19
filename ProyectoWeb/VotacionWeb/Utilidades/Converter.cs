using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ProyectoWeb.Utilidades
{
    public class Converter
    {
        public static string toBase64String(string path)
        {

            string Base64string = "";
            try
            {
                byte[] bytes = File.ReadAllBytes(path);
                Base64string = Convert.ToBase64String(bytes);
            }
            catch
            {

            }
            return Base64string;
        }
    }
}