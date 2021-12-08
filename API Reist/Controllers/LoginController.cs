using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using API_Reist.Models;

namespace API_Reist.Controllers
{
    public class LoginController : Controller
    {
        Cliente client = new Cliente();

        [HttpGet]
        [ActionName("Login")]
        public bool Login(string email, string senha)
        {
            return client.Autenticar(email, senha);
        }
    }
}
