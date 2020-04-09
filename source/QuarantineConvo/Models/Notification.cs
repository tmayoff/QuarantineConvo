using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuarantineConvo.Models {
    public class Notification {

        public string Title { get; set; }

        public string Message { get; set; }

        public bool Viewed { get; set; }
    }
}
