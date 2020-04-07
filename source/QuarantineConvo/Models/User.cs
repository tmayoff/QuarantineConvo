using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QuarantineConvo.Models {
    public class User {
        
        [Required]
        [StringLength(50)]
        [Key]
        public string Username { get; set; }
        
        [Required]
        [StringLength(50)]
        [RegularExpression("^[a-zA-Z]*$")]
        [DisplayName("First name")]
        public string FirstName { get; set; }
        
        [Required]
        [StringLength(50)]
        [RegularExpression("^[a-zA-Z]*$")]
        [DisplayName("Last name")]
        public string LastName { get; set; }

        [RegularExpression("[A-z]*[0-9]*@[A-z]+.[A-z]+")]
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        
        [StringLength(50, MinimumLength = 6)]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool isAdmin { get; set; }
    }
}
