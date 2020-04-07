using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuarantineConvo.Data;
using QuarantineConvo.Models;

namespace QuarantineConvo.Controllers {
    public class MessagingController : Controller {

        QuarantineConvoContext db;

        public MessagingController (QuarantineConvoContext _db) {
            db = _db;
        }

        public IActionResult Index() {
            //HttpContext.Session.SetString("_, "tmayoff");
            string currentUser = "tmayoff";

            ViewData["currentUser"] = currentUser;
            Connection c = db.Connection.Where(c => c.user1 == currentUser || c.user2 == currentUser).FirstOrDefault();
            return View(c);
        }
    }
}