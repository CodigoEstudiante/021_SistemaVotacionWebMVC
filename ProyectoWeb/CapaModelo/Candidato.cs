using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo
{
    public class Candidato
    {
        public int IdCandidato { get; set; }
        public string NombresCompleto { get; set; }
        public string Mensaje { get; set; }
        public string RutaImagen { get; set; }
        public string ImagenBase64 { get; set; }
        public string Extension { get; set; }
        public Eleccion oEleccion { get; set; }
        public Usuario oUsuario { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; }

    }
}
