using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using API_Reist.Models;

namespace API_Reist.Controllers
{
    public class PassagemController : Controller
    {
        Passagem passagem = new Passagem();

        [HttpGet]
        [ActionName("BuscarIda")]
        public IEnumerable ListIda(string cidadeOrigem, string cidadeDestino, string data, int classe)
        {
            return passagem.BuscarPassagensIda(cidadeOrigem, cidadeDestino, data, classe);
        }

        [HttpGet]
        [ActionName("BuscarIdaVolta")]
        public IEnumerable ListIdaVolta(string cidadeOrigem, string cidadeDestino, string dataIda, string dataVolta, int classe)
        {
            return passagem.BuscarPassagensIdaVolta(cidadeOrigem, cidadeDestino, dataIda, dataVolta,  classe);
        }
    }
}
