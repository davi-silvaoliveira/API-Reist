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
        public IEnumerable ListBy()
        {
            return passagem.ListarPassagens();
        }
    }
}
