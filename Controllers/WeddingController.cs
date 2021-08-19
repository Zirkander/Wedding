using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeddingProj.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace WeddingProj.Controllers
{
    public class WeddingController : Controller
    {
        private WeddingContext db;

        public WeddingController(WeddingContext context)
        {
            db = context;
        }
        private int? uid
        {
            get
            {
                return HttpContext.Session.GetInt32("UserID");
            }
        }

        private bool isLoggedIn
        {
            get
            {
                return uid != null;
            }
        }

        [HttpGet("/Wedding")]
        public IActionResult WeddingDisplay()
        {
            //pass in weddding list
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            //Wedding.Models.Wedding is because my project and namespace is the same name (Don't do this in the future.)
            List<Wedding> allWeddings = db.Weddings
            .Include(c => c.RSVP)
            .ThenInclude(u => u.User)
            .ToList();
            return View("All", allWeddings);
        }

        [HttpGet("/NewWedding")]
        public IActionResult CreateWedding()
        {
            return View("NewWedding");
        }

        [HttpPost("/NewWedding")]
        public IActionResult NewWedding(Wedding newWedding)
        {
            if (ModelState.IsValid == false)
            {
                return View("NewWedding");
            }
            if (newWedding.WeddingDate < DateTime.Now)
            {
                ModelState.AddModelError("Date", "Must be in the future!");
                return View("NewWedding");
            }

            db.Add(newWedding);
            db.SaveChanges();
            return RedirectToAction("WeddingDisplay");
        }

        [HttpPost("/Wedding/{weddingId}/Rsvp")]
        public IActionResult Rsvp(int weddingId)
        {
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }

            UserWeddingRSVP RSVP = db.RSVP
            .FirstOrDefault(r => r.UserId == (int)uid && r.WeddingId == weddingId);

            if (RSVP == null)
            {
                UserWeddingRSVP rsvp = new UserWeddingRSVP()
                {
                    WeddingId = weddingId,
                    UserId = (int)uid
                };
                db.RSVP.Add(rsvp);
            }
            else
            {
                db.RSVP.Remove(RSVP);
            }

            db.SaveChanges();
            return RedirectToAction("WeddingDisplay");
        }
    }
}
