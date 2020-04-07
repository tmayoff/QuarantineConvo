using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuarantineConvo.Models {

    public class ConnectionHub : Hub {

        public async Task SendMessage(string user, string message) {
            // TODO Save message to database

            //HttpContext.Session.GetString(UsersController.SessionKeyName, "tmayoff");
            user = Context.User.Identity.Name;
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}