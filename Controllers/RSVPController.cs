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


namespace RSVP.Controllers
{
    public class RSVPController : Controller
    {
        private WeddingContext db;

        public RSVPController(WeddingContext context)
        {
            db = context;
        }
    }
}
