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

        public ConnectionHub(IServiceProvider sp) {
            db = sp.GetRequiredService(typeof(QuarantineConvoContext)) as QuarantineConvoContext;
        }

        public async Task SendMessage(string connectionString, string message) {
            int connectionID = int.Parse(connectionString);

            string user = Context.User.Identity.Name;
            Connection connection = db.Connection.FirstOrDefault(conn => conn.ID == connectionID);
            Message msg = new Message() {
                Connection = connection,
                Msg = message,
                SentBy = user,
                TimeStamp = DateTime.UtcNow
            };
            db.Message.Add(msg);
            db.SaveChanges();

            string toUser;
            if (user == connection.user1)
                toUser = connection.user2;
            else
                toUser = connection.user1;

            ClientConnection clientConnection = db.ClientConnection.FirstOrDefault(c => c.UserName == toUser);

            await Clients.User(clientConnection.UserID).SendAsync("ReceiveMessage", message);
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