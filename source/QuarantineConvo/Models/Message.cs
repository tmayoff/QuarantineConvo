using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuarantineConvo.Models {
    public class Message {

        [Key]
        public int ID;

        public DateTime timeStamp;

        public Connection connection;

        public User SentBy;

        public string Msg;
    }
}