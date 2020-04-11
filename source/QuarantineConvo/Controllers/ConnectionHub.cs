using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using QuarantineConvo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QuarantineConvo.Models {

    public class ConnectionHub : Hub {

        QuarantineConvoContext db;
        QuarantineConvoIdentityContext IdentityContext;

        public ConnectionHub(IServiceProvider sp) {
            db = sp.GetRequiredService(typeof(QuarantineConvoContext)) as QuarantineConvoContext;
            IdentityContext = sp.GetRequiredService(typeof(QuarantineConvoIdentityContext)) as QuarantineConvoIdentityContext;
        }

        public async Task SendMessage(string connectionString, string message) {
            if (connectionString == "") return;

            string user = Context.User.Identity.Name;
            Connection connection = db.Connection.FirstOrDefault(conn => conn.ID == Guid.Parse(connectionString));
            Message msg = new Message() {
                Connection = connection,
                Msg = message,
                SentBy = user,
                TimeStamp = DateTime.UtcNow,
                Read = false
            };
            db.Message.Add(msg);
            db.SaveChanges();

            string toUser;
            if (user == connection.user1)
                toUser = connection.user2;
            else
                toUser = connection.user1;

            ClientConnection clientConnection = db.ClientConnection.FirstOrDefault(c => c.UserName == toUser);
            await Clients.User(clientConnection.UserID).SendAsync("ReceiveMessage", message, msg.ID);
        }

        public async Task ReadAllMessages(string connectionString) {
            if (string.IsNullOrEmpty(connectionString)) return;

            foreach(Message msg in db.Message.Where(m => m.Connection.ID == Guid.Parse(connectionString))) {
                msg.Read = true;
                db.Message.Update(msg);
            }

            db.SaveChanges();
        }

        public async Task ReadMessage(int messageID) {
            Message msg = db.Message.First(m => m.ID == messageID);
            msg.Read = true;
            db.Message.Update(msg);
            await db.SaveChangesAsync();
        }

        public async Task SendNotification(string toUserID, string message) {
            // Add to the database


            await Clients.User(toUserID).SendAsync("ReceiveNotification", message);
        }

        public string GetDisplayNameFromEmail(string email) {
            User u = IdentityContext.Users.FirstOrDefault(u => u.Email == email);
            if (u == null) return "";

            return u.DisplayName;
        }

        public override Task OnConnectedAsync() {

            ClientConnection clientConnection = db.ClientConnection.FirstOrDefault(c => c.UserName == Context.User.Identity.Name);
            if (clientConnection == null) {
                clientConnection = new ClientConnection() {
                    UserName = Context.User.Identity.Name,
                    UserID = Context.UserIdentifier,
                    ConnectionID = Context.ConnectionId
                };

                db.ClientConnection.Add(clientConnection);
            }
            else {
                clientConnection.UserName = Context.User.Identity.Name;
                clientConnection.UserID = Context.UserIdentifier;
                clientConnection.ConnectionID = Context.ConnectionId;

                db.ClientConnection.Update(clientConnection);
            }

            db.SaveChanges();

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception) {
            //var us = userConnections.Remove(userConnections.FirstOrDefault(u => u.UserName == Context.User.Identity.Name && u.ConnectionID == Context.ConnectionId));
            return base.OnDisconnectedAsync(exception);
        }
    }
}