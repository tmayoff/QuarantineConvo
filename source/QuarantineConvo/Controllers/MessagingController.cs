using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using QuarantineConvo.Data;
using QuarantineConvo.Models;

namespace QuarantineConvo.Controllers {
    public class MessagingController : Controller {

        private readonly QuarantineConvoIdentityContext identityContext;
        private readonly QuarantineConvoContext db;
        private List<SearchRequest> searchRequests = new List<SearchRequest>();
        private IHubContext<ConnectionHub> hubContext;

        public MessagingController(QuarantineConvoContext context, IHubContext<ConnectionHub> hubcontext, QuarantineConvoIdentityContext _identityContext) {
            db = context;
            hubContext = hubcontext;
            identityContext = _identityContext;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index(int connectionId) {
            Connection connection = db.Connection.FirstOrDefault(c => c.ID == connectionId);
            if (connection == null || connection.user1 != User.Identity.Name || connection.user2 != User.Identity.Name)
                RedirectToAction("Connections");

            string oUser = connection.user1 == User.Identity.Name ? connection.user2 : connection.user1;
            User user = identityContext.Users.FirstOrDefault(u => u.Email == oUser);

            ViewData["otherUser"] = user.DisplayName;

            List<Message> messages = db.Message.Where(m => m.Connection.ID == connection.ID).ToList();
            ViewData["messages"] = messages;
            return View(connection);
        }

        [Authorize]
        public IActionResult FindConnection(string message) {
            var interests = db.Interest.ToList();
            return View(interests);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Search(List<string> interestCheckboxes) {
            string currentUser = User.Identity.Name;
            long currentInterests = GetInterests(interestCheckboxes);

            SearchRequest request = new SearchRequest() {
                Username = currentUser,
                Interests = currentInterests
            };

            db.SearchRequest.Add(request);
            db.SaveChanges();

            searchRequests.Add(request);

            SearchRequest foundUser = db.SearchRequest.FirstOrDefault(r => (r.Interests & currentInterests) != 0 && r.Username != currentUser);

            if (null == foundUser) {
                return RedirectToAction("FindConnection");
            }

            else {
                long commonInterests = foundUser.Interests & currentInterests;

                Connection theConnection = db.Connection.FirstOrDefault(c => c.user1 == currentUser && c.user2 == foundUser.Username);

                if (theConnection == null) {
                    theConnection = new Connection() {
                        user1 = currentUser,
                        user2 = foundUser.Username,
                        interests = commonInterests,
                        active = true
                    };

                    db.Connection.Add(theConnection);
                }

                else {
                    theConnection.interests = commonInterests;
                    theConnection.active = true;

                    db.Connection.Update(theConnection);
                }

                db.SearchRequest.RemoveRange(db.SearchRequest.Where(r => r.Username == currentUser));
                db.SearchRequest.RemoveRange(db.SearchRequest.Where(r => r.Username == foundUser.Username));
                db.SaveChanges();

                theConnection = db.Connection.FirstOrDefault(c => c.user1 == currentUser && c.user2 == foundUser.Username);

                await SendNewConnection(theConnection.ID.ToString());

                return RedirectToAction("Index", new { connectionId = theConnection.ID });
            }
        }

        private long GetInterests(List<string> interestCheckboxes) {
            long interests = 0;

            foreach (Interest interest in db.Interest) {
                if (interestCheckboxes.Contains(interest.Name))
                    interests += 1 << (int)interest.Position;
            }

            return interests;
        }

        private string GetInterestNames(long interestsBitMap) {
            string interests = string.Empty;

            foreach (Interest interest in db.Interest) {
                long bitCompare = 1 << (int)interest.Position;

                if ((interestsBitMap & bitCompare) != 0) {
                    if (!string.IsNullOrWhiteSpace(interests))
                        interests += ", ";

                    interests += interest.Name;
                }
            }

            return interests;
        }

        public async Task SendNewConnection(string connectionString) {
            int connectionID = int.Parse(connectionString);
            Connection connection = db.Connection.FirstOrDefault(conn => conn.ID == connectionID);

            string interests = GetInterestNames(connection.interests);

            ClientConnection clientConnection_1 = db.ClientConnection.FirstOrDefault(c => c.UserName == connection.user1);
            ClientConnection clientConnection_2 = db.ClientConnection.FirstOrDefault(c => c.UserName == connection.user2);

            string message_1 = "New connection with user: " + connection.user2 + " about: " + interests;
            string message_2 = "New connection with user: " + connection.user1 + " about: " + interests;

            await hubContext.Clients.User(clientConnection_1.UserID).SendAsync("ReceiveNotification", message_1);
            await hubContext.Clients.User(clientConnection_2.UserID).SendAsync("ReceiveNotification", message_2);
        }

        public IActionResult Connections() {
            List<Connection> con = db.Connection.Where(c => User.Identity.Name == c.user1 || User.Identity.Name == c.user2).ToList();

            List<DisplayNameConnection> displayNameConnections = new List<DisplayNameConnection>();
            foreach(Connection c in con) {
                // Get the other user's display name
                string oUser = c.user1 == User.Identity.Name ? c.user2 : c.user1;
                // Get their user object
                User user = identityContext.Users.FirstOrDefault(u => u.Email == oUser);


                DisplayNameConnection displayNameConnection = new DisplayNameConnection() {
                    OtherUser = user,
                    Connection = c
                };

                displayNameConnections.Add(displayNameConnection);
            }

            return View(displayNameConnections);
        }
    }
}
