using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using API_Reist.Models;

namespace API_Reist.Controllers
{
    public class QuartoController : Controller
    {
        Quarto quarto = new Quarto();

        [HttpGet]
        [ActionName("Listar")]
        public IEnumerable ListBy(long id)
        {
            return quarto.ListarQuartos(id);
        }
    }
}
