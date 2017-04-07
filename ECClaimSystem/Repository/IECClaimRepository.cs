using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECClaimSystem.Repository
{
    public interface IECClaimRepository
    {
        User Login(string account, string password);
        long InsertECClaim(ECClaim ec);
        Setting GetSettingByKey(string key);
        int InsertECClaimEvidence(ECEvidence evidence);
        List<ECClaim> GetListECClaimWithSpecificUser(User user);
        void DeleteECClaim(int ClaimId);
        ECClaim GetECClaimById(int ClaimId);
        void UpdateECClaim(ECClaim ec);
        List<Faculty> GetAllFaculty();
        List<ECClaim> GetECClaimsByFaculty(int facultyId);
    }
}
