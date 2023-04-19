using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaModelo
{
    public class Votacion
    {

        public int IdVotacion { get; set; }
        public int IdEleccion { get; set; }
        public int IdVotante { get; set; }
        public string DocumentoIdentidad { get; set; }
        public int IdCandidato { get; set; }
    }
}
