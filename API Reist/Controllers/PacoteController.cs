using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Reist.Controllers
{
    public class PacoteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
