using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        public IActionResult Index() {

            string currentUser = User.Identity.Name;
            Connection c = db.Connection.Where(c => c.user1 == currentUser || c.user2 == currentUser).FirstOrDefault();

            List<Message> messages = db.Message.Where(m => m.Connection.ID == c.ID).ToList();
            ViewData["messages"] = messages;
            return View(c);
        }
    }
}