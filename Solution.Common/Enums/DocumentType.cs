using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Solution.Business.Enums
{
    public enum DocumentType
    {
        CompanyLogo = 1,
        ProfilePicture = 2,
        Other = 0,
    }
    public enum ClassTimeSlots
    {
        [Display(Name = "9:00 AM")]
        NineAM = 9,
        [Display(Name = "10:00 AM")]
        TenAM = 10,
        [Display(Name = "11:00 AM")]
        ElevenAM = 11,
        [Display(Name = "12:00 PM")]
        TwelvePM = 12,
        [Display(Name = "1:00 PM")]
        OnePM = 13,
        [Display(Name = "2:00 PM")]
        TwoPM = 14,
        [Display(Name = "3:00 PM")]
        ThreePM = 15
    }
    public enum UserTypes
    {
        Enterprise = 1,
        Company = 2,
        Admin = 3,
        User = 4,
    }
    public enum GenderList
    {
        Male,
        Female,
        Other
    }
    public enum Status
    {
        Pending,
        InProgress,
        Approved,
        Rejected,
    }
    public enum LoginResult
    {
        Success,
        UserNotFound,
        WrongPassword,
        LockedOut,
        NotAllowed,
        RequiresTwoFactor
    }
}
