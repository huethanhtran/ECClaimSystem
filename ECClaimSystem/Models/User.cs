using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECClaimSystem.Models
{
    public class User
    {
        public long UserId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field can not be empty")]
        public string AccountId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field can not be empty")]
        public string Password { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field can not be empty")]
        public string UserFullName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field can not be empty")]
        public DateTime BirthDay { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field can not be empty")]
        public string Address { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field can not be empty")]
        public int Gender { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field can not be empty")]
        public string Phone { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field can not be empty")]
        public string Email { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field can not be empty")]
        public Faculty Faculty { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field can not be empty")]
        public Role Role { get; set; }
        public bool Active { get; set; }


    }
}