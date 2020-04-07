using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using QuarantineConvo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuarantineConvo.Data;

namespace QuarantineConvo.Models {

    public class ConnectionHub : Hub {

        QuarantineConvoContext db;

        public ConnectionHub(IServiceProvider sp) {
            db = sp.GetRequiredService(typeof(QuarantineConvoContext)) as QuarantineConvoContext;
        }

        public async Task SendMessage(string connectionString, string message) {
            int connectionID = int.Parse(connectionString);

            // TODO Save message to database
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
            if (connection.user1 == user)
                toUser = connection.user2;
            else
                toUser = connection.user1;

            await Clients.User(toUser).SendAsync("ReceiveMessage", message);
        }
    }
}