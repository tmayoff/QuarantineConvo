using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuarantineConvo.Models {
    public class Message {

        public DateTime timeStamp;

        public Connection connection;

        public User SentBy;

        public string Msg;
    }
}