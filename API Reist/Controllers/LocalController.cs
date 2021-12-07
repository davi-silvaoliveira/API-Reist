using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Reist.Controllers
{
    public class LocalController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
