using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECClaimSystem.Models.Elements
{
    public class ConstantsKey
    {
        public enum Role
        {
            Student = 1,
            ECManager = 2,
            ECCoordinator = 3,
            Administrator = 4
        }
        public enum ClaimStatus
        {
           Pending = 1, 
           Processing = 2,
           Processed = 3,
           Rejected = 4
        }
        public enum DecisionStatus
        {
           Pending = 1,
           Success = 2,
           Fail = 3
        }
        public enum EvidenceType
        {
            Pdf = 1,
            Image = 2
        }
        public enum SortType
        {
           ClosureDate = 1,
           Faculty = 2
        }
        public enum Gender
        {
            Male = 1,
            Female = 2
        }
        public enum ReportType
        {
           NotEvidence = 1,
            DecisionLate = 2
        }
    }
}
