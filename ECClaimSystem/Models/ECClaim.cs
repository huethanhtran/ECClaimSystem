using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECClaimSystem.Models
{
    public class ECClaim
    {
        public long ClaimId { get; set; }
        public User User { get; set; }
      
        public string Summary { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field can not be empty")]
        public string Situation { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field can not be empty")]
        public string EffectSituation { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field can not be empty")]
        [DataType(DataType.Date, ErrorMessage = "It must be date")]
        public DateTime? CircumstanceStartDate { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "This field can not be empty")]
        [DataType(DataType.Date, ErrorMessage ="It must be date")]
        public DateTime? CircumstanceEndDate { get; set; }
        public int OutCome { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public DateTime? FinalClosureDate { get; set; }
        public int? ClaimStatus { get; set; }
        public int? DecisionStatus { get; set; }
        public long? ProcessedUser { get; set; }
        public bool HasEvidence { get; set; }
        public bool Active { get; set; }
        
        public DateTime? DecisionDate { get; set; }
    }
}
