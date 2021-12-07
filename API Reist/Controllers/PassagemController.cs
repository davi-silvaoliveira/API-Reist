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
        [ActionName("Buscar")]
        public IEnumerable ListBy(string cidadeOrigem, string cidadeDestino, string data, int classe)
        {
            return passagem.BuscarPassagensIda(cidadeOrigem, cidadeDestino, data, classe);
        }
    }
}
