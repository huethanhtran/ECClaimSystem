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
        void DeleteECClaim(long ClaimId);
        ECClaim GetECClaimById(long ClaimId);
        void UpdateECClaim(ECClaim ec);
        List<Faculty> GetAllFaculty();
        List<ECClaim> GetECClaimsByFaculty(int facultyId);
        List<ECEvidence> GetEvidencesByECClaim(long ClaimId);
        void AcceptECClaim(ECClaim ec);
        void UpdateDecision(int decisionStatus, long claimId);
        string SendMail(User u, long claimId);
        string ChangeActiveOfECClaim(long claimId, bool value);
        List<User> GetAllUser();
        string ChangeActiveStatusUser(long userId, bool value);
        User GetUserById(long userId);
        int UpdateUser(Models.User user);
        int InsertUser(Models.User user);
        List<Setting> GetAllSetting();
        void UpdateOrInsertSetting(int? id, string key, string value);
        Setting GetSettingById(int id);
        string ChangeActiveSetting(int id, bool value);
        List<Setting> SearchSetting(string searchkey);
    }
}
