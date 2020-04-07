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
        private List<SearchRequest> searchRequests = new List<SearchRequest>();

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

        public IActionResult Conversation(Connection connection)
        {
            return View("Index", connection);
        }

        public IActionResult FindConnection(string message)
        {
            var interests = db.Interest.ToList();

            return View(interests);
        }

        [HttpPost]
        public ActionResult Search(List<string> interestCheckboxes)
        {
            string currentUser    = User.Identity.Name;
            long currentInterests = GetInterests(interestCheckboxes);

            SearchRequest request = new SearchRequest()
            {
                Username = currentUser,
                Interests = currentInterests
            };

            db.SearchRequest.Add(request);
            db.SaveChanges();

            searchRequests.Add(request);

            SearchRequest foundUser = db.SearchRequest.FirstOrDefault(r => (r.Interests & currentInterests) != 0 && r.Username != currentUser);

            if (null == foundUser)
            {
                return RedirectToAction("FindConnection");
            }

            else
            {
                Connection connection = new Connection()
                {
                    user1 = currentUser,
                    user2 = foundUser.Username,
                    active = true
                };

                db.Connection.Add(connection);
                db.SearchRequest.RemoveRange(db.SearchRequest.Where(r => r.Username == currentUser));
                db.SearchRequest.RemoveRange(db.SearchRequest.Where(r => r.Username == foundUser.Username));
                db.SaveChanges();

                return RedirectToAction("Conversation", new { connection = connection });
            }
        }

        private long GetInterests(List<string> interestCheckboxes)
        {
            long interests = 0;

            foreach (Interest interest in db.Interest)
            {
                interests <<= 1;

                if (interestCheckboxes.Contains(interest.Name))
                    interests += 1;
            }

            return interests;
        }
    }
}