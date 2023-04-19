using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo
{
    public class Eleccion
    {
        public int IdEleccion { get; set; }
        public string Descripcion { get; set; }
        public string Cargo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public Usuario oUsuario { get; set; }
        public bool Activo { get; set; }
    }
}
