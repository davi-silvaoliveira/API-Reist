using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Reist.Models
{
    public class Viagem
    {
        public int id { get; set; }
        public int quantidade_pessoas { get; set; }
        public int quantidade_quartos { get; set; }
        public string checkin { get; set; }
        public string checkout { get; set; }
        public Quarto quarto { get; set; }
        public Passagem ida { get; set; }
        public IdaVolta idaVolta { get; set; }
    }
}
