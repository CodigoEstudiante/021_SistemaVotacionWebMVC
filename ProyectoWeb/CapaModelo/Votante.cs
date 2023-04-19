using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo
{
    public class Votante
    {
        public int IdVotante { get; set; }
        public string DocumentoIdentidad { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public Eleccion oEleccion { get; set; }
        public Usuario oUsuario { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Estado { get; set; }
        public string Mensaje { get; set; }
        public bool EmitioVotacion { get; set; }
    }
}
