using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ECClaimSystem.Models
{
    public class Setting
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings =false, ErrorMessage ="This field can not be empty")]
        public string Key { get; set; }
        public string Value { get; set; }
        public bool Active { get; set; }
    }
}