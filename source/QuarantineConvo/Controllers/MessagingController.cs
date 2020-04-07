using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuarantineConvo.Data;
using QuarantineConvo.Models;

namespace QuarantineConvo.Controllers
{
    public class MessagingController : Controller
    {

        private readonly QuarantineConvoContext db;

        public MessagingController(QuarantineConvoContext context)
        {
            db = context;
        }

        public IActionResult Index() {
            //HttpContext.Session.SetString("_, "tmayoff");
            string currentUser = "tmayoff";

            ViewData["currentUser"] = currentUser;
            Connection c = db.Connection.Where(c => c.user1 == currentUser || c.user2 == currentUser).FirstOrDefault();
            return View(c);
        }

        public IActionResult FindConnection()
        {
            var interests = db.Interest.ToList();

            return View(interests);
        }

        public IActionResult Search()
        {
            return Index();
        }

    }
}