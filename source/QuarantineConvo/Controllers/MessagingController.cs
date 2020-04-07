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

        public IActionResult FindConnection()
        {
            var interests = db.Interest.ToList();

            return View(interests);
        }

        [HttpPost]
        public IActionResult Search(List<string> interestCheckboxes)
        {
            string currentUser    = User.Identity.Name;
            long currentInterests = GetInterests(interestCheckboxes);

            SearchRequest request = new SearchRequest()
            {
                Username = currentUser,
                Interests = currentInterests
            };

            searchRequests.Add(request);

            string foundUser = GetCommonInterest(currentUser, currentInterests);

            if (string.IsNullOrEmpty(foundUser))
                return FindConnection();

            Connection connection = new Connection()
            {
                user1  = currentUser,
                user2  = foundUser,
                active = true
            };

            db.Connection.Add(connection);
            db.SaveChanges();

            searchRequests.Remove(request);

            return Conversation(connection);
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

        private string GetCommonInterest(string searchingUser, long searchingInterests)
        {
            bool stillSearching = true;
            int timeOutCounter  = 0;
            int timeOut         = 1000000;

            SearchRequest userFound = null;


            while (stillSearching)
            {
                //timeOutCounter++;

                if (timeOutCounter >= timeOut)
                    return string.Empty;

                userFound = searchRequests.FirstOrDefault(r => (r.Interests & searchingInterests) != 0 && r.Username != searchingUser);

                if (userFound != null)
                    stillSearching = false;
            }

            return userFound.Username;
        }
    }
}