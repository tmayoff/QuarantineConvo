using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuarantineConvo.Models {
    public class User : IdentityUser {
        
        [Required]
        [StringLength(50)]
        [RegularExpression("^[a-zA-Z]*$")]
        [DisplayName("First name")]
        [PersonalData]
        public string FirstName { get; set; }
        
        [Required]
        [StringLength(50)]
        [RegularExpression("^[a-zA-Z]*$")]
        [DisplayName("Last name")]
        [PersonalData]
        public string LastName { get; set; }

        [PersonalData]
        public bool isAdmin { get; set; }
    }
}
