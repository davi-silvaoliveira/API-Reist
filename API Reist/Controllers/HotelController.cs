using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using API_Reist.Models;

namespace API_Reist.Controllers
{
    public class HotelController : Controller
    {
        Hotel hotel = new Hotel();

        [HttpGet]
        [ActionName("Buscar")]
        public IEnumerable ListBy(string estado)
        {
            return hotel.BuscarHotel(estado);
        }
    }
}
