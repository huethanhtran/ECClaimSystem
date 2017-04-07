using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECClaimSystem.Models
{
  public class Role
    {
        
        public int RoleId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage ="This field can not be empty")]
        public string RoleName { get; set; }
        public bool Active { get; set; }
    }
}
