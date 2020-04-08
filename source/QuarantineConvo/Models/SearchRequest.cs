using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuarantineConvo.Models
{
    public class SearchRequest
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public long Interests { get; set; }
    }
}
