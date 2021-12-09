using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using API_Reist.Models;

namespace API_Reist.Controllers
{
    public class LocalController : Controller
    {
        Local local = new Local();

        [HttpGet]
        public Local Buscar(string nome)
        {
            return local.Buscar(nome);
        }
    }
}
