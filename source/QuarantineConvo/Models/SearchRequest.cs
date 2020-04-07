using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuarantineConvo.Models
{
    public class SearchRequest
    {
        public string Username { get; set; }

        public long Interests { get; set; }
    }
}
