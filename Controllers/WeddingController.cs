using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Wedding.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;


namespace Wedding.Controllers
{
    public class WeddingController : Controller
    {
        private WeddingContext db;

        public WeddingController(WeddingContext context)
        {
            db = context;
        }

        [HttpGet("/Wedding")]
        public IActionResult WeddingDisplay()
        {
            return View("All");
        }

    }
}
