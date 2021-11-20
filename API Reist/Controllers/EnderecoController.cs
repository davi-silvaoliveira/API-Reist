using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using API_Reist.Models;

namespace API_Reist.Controllers
{
    public class EnderecoController : Controller
    {
        Endereco endereco = new Endereco();

        [HttpGet]
        [ActionName("Listar")]
        public IEnumerable ListAll()
        {
            //return endereco.ListarEnderecos();
            try
            {
                return endereco.ListarEnderecos();
            }
            catch
            {
                RedirectToAction("Index", "Home");
                return null;
            }
        }
    }
}
