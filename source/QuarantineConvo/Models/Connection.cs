using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace QuarantineConvo.Models {

    public class Connection {

        [Key]
        public int ID { get; set; }

        public string user1 { get; set; }

        public string user2 { get; set; }

        public bool active { get; set; }
    }
}