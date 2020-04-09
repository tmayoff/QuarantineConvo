using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuarantineConvo.Models {
    public class Message {

        [Key]
        public int ID { get; set; }

        public DateTime TimeStamp { get; set; }

        public Connection Connection { get; set; }

        public string SentBy { get; set; }

        public string Msg { get; set; }

        public bool Read { get; set; }
    }
}