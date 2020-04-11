using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuarantineConvo.Models {
    public class ConnectionList {

        public User OtherUser;

        public Connection Connection;

        public bool ContainsUnread;

        public string LastMessage;
    }
}
