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
        Cliente client = new Cliente();

        [HttpGet]
        [ActionName("Listar")]
        public IEnumerable ListAll()
        {
            //return cliente.ListarClientes();
            try
            {
                return client.ListarClientes();
            }
            catch
            {
                RedirectToAction("Index", "Home");
                return null;
            }
        }

        [HttpPost]
        [ActionName("Cadastrar")]
        public bool Insert([FromBody] Cliente cliente)
        {
            //return cliente.Insert();
            try
            {
                cliente.Insert();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
