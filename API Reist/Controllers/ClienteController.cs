using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using API_Reist.Models;

namespace API_Reist.Controllers
{
    public class ClienteController : Controller
    {
        Cliente cliente = new Cliente();

        [HttpGet]
        [ActionName("Listar")]
        public IEnumerable ListAll()
        {
            //return cliente.ListarClientes();
            try
            {
                return cliente.ListarClientes();
            }
            catch
            {
                RedirectToAction("Index", "Home");
                return null;
            }
        }
    }
}
