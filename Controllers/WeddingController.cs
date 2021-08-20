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

            List<Wedding> allWeddings = db.Weddings.ToList();

            foreach (var wedding in allWeddings)
            {
                if (wedding.WeddingDate < DateTime.Now)
                {
                    db.Weddings.Remove(wedding);
                    db.SaveChanges();
                }
            }

            //Wedding.Models.Wedding is because my project and namespace is the same name (Don't do this in the future.)
            List<Wedding> newWeddings = db.Weddings
            .Include(c => c.RSVP)
            .ToList();
            return View("All", newWeddings);
        }

        [HttpGet("/NewWedding")]
        public IActionResult CreateWedding()
        {
            return View("NewWedding");
        }

        [HttpPost("/NewWedding")]
        public IActionResult NewWedding(Wedding newWedding)
        {
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid == false)
            {
                return View("NewWedding");
            }
            if (newWedding.WeddingDate < DateTime.Now)
            {
                ModelState.AddModelError("Date", "Must be in the future!");
                return View("NewWedding");
            }
            User NewWedCreater = db.Users.FirstOrDefault(u => u.UserID == (int)uid);
            newWedding.User = NewWedCreater;
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

        [HttpPost("Delete/{weddingId}/Delete")]
        public IActionResult Delete(int weddingId)
        {
            Wedding wedding = db.Weddings.FirstOrDefault(w => w.WeddingId == weddingId);

            if (wedding == null)
            {
                return RedirectToAction("WeddingDisplay");
            }

            db.Weddings.Remove(wedding);
            db.SaveChanges();
            return RedirectToAction("WeddingDisplay");
        }

        [HttpGet("/WeddingDetails/{weddingId}")]
        public IActionResult WeddingDetails(int weddingId)
        {
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }

            Wedding wedding = db.Weddings
            .Include(guest => guest.RSVP)
            // .Include(u => u.User)
            .ThenInclude(rsvp => rsvp.User)
            .FirstOrDefault(w => w.WeddingId == weddingId);
            Console.WriteLine(wedding);

            if (wedding == null)
            {
                return RedirectToAction("WeddingDisplay");
            }

            return View("WeddingDetails", wedding);

        }

        [HttpGet("/wedding/{weddingId}/edit")]
        public IActionResult Edit(int weddingId)
        {
            if (!isLoggedIn)
            {
                return RedirectToAction("Index", "Home");
            }
            Wedding wedding = db.Weddings.FirstOrDefault(w => w.WeddingId == weddingId);

            // The edit button will be hidden if you are not the author,
            // but the user could still type the URL in manually, so
            // prevent them from editing if they are not the author.
            if (wedding == null || wedding.UserId != uid)
            {
                return RedirectToAction("All");
            }

            return View("Edit", wedding);
        }

        [HttpPost("/weddings/{weddingId}/update")]
        public IActionResult Update(int weddingId, Wedding editedWedding)
        {
            if (ModelState.IsValid == false)
            {
                editedWedding.WeddingId = weddingId;
                // Send back to the page with the current form edited data to
                // display errors.
                return View("Edit", editedWedding);
            }

            Wedding dbWedding = db.Weddings.FirstOrDefault(w => w.WeddingId == weddingId);

            if (dbWedding == null)
            {
                return RedirectToAction("WeddingDisplay");
            }

            dbWedding.Wedder1Name = editedWedding.Wedder1Name;
            dbWedding.Wedder2Name = editedWedding.Wedder2Name;
            dbWedding.WeddingAddress = editedWedding.WeddingAddress;
            dbWedding.UpdatedAt = DateTime.Now;

            db.Weddings.Update(dbWedding);
            db.SaveChanges();

            /* 
            When redirecting to action that has params, you need to pass in a
            dict with keys that match param names and the value of the keys are
            the values for the params.
            */
            return RedirectToAction("Details", new { weddingId = weddingId });
        }

    }
}
