using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECClaimSystem.Models
{
    public class ECEvidence
    {
        public int EvidenceId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field can not be empty")]
        public string EvidenceName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field can not be empty")]
        public long ClaimId { get; set; }
        public int? Type { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool Active { get; set; }
    }
}
