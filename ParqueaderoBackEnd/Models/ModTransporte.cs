using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParqueaderoBackEnd.Models
{
    public class ModTransporte
    {
        public string Placa { get; set; }
        public string Transporte { get; set; }
        public string Estado { get; set; }
        public string Hora { get; set; }
        public string Minutos { get; set; }
    }
}
