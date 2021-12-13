using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using API_Reist.Models;

namespace API_Reist.Controllers
{
    public class PacoteController : Controller
    {
        Pacote pacote = new Pacote();

        [HttpGet]
        [ActionName("BuscarPacote")]
        public IEnumerable ListIdaVolta(string cidadeOrigem, string cidadeDestino, string dataIda, string dataVolta, int classe)
        {
            return pacote.BuscarPacotes(cidadeOrigem, cidadeDestino, dataIda, dataVolta, classe);
        }
    }
}
