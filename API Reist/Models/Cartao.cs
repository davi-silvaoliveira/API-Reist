using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Reist.Models
{
    public class Cartao
    {
        public int id { get; set; }
        public long numero { get; set; }
        public string validade { get; set; }
        public string codigoSeguranca { get; set; }
        public string tipo { get; set; }
        public string bandeira { get; set; }
    }
}
