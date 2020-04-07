using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuarantineConvo.Models {
    public class ClientConnection {

        [Key]
        public int ID { get; set; }

        public string UserName { get; set; }

        public string UserID { get; set; }

        public string ConnectionID { get; set; }
    }
}
