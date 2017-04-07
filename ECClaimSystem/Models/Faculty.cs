using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECClaimSystem.Models
{
    public class Faculty
    {
        public int FacultyId { get; set; }
        [Required(AllowEmptyStrings =false, ErrorMessage ="This field can not be empty")]
        public string FacultyCode { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field can not be empty")]
        public string FacultyName { get; set; }
        public bool Active { get; set; }
    }
}