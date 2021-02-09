using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Show_List.Base
{
    public enum LangDirection
    {
        LTR,
        RTL
    }
    public static class LangCode
    {
        public static string English = "en-US";
        public static string Arabic = "ar-SA";
    }

    public class CurrentUserContext
    {

        public int UserID { get; set; }
        public string UserName { get; set; }

        public string UserPWD { get; set; }

        public int CurrCountryID { get; set; }
        public int CurrCityID { get; set; }
        public int NationalityCountryID { get; set; }

        public string CurrCountryName { get; set; }
        public string CurrCityName { get; set; }
        public string NationalityCountryName { get; set; }

        public string Gender { get; set; }

        public DateTime DOB { get; set; }

        public string MobileNo { get; set; }

        public string Email_ID { get; set; }

        public string Email_ID_Verified { get; set; }

        public string NICIqamaNumber { get; set; }

        public string UserLang { get; set; }


        public LangDirection LangDir { get; set; }

        public string PreviousPage { get; set; }

        public string FullName_AR { get; set; }
        public string FullName_EN { get; set; }

        public string RoleIDsCommaSep { get; set; }

        public string TeacherID { get; set; }
        public string StudentID { get; set; }

        public byte[] UserProfilePic { get; set; }

        #region PortalRegistration

        public string UserNIC { get; set; }

        public string CodeVarify { get; set; }
        public string UserRegistrationMinutes { get; set; }

        public string UserRegistrationSeconds { get; set; }

        public string UserRegistrationCode { get; set; }

        public string UserRole { get; set; }

        public bool ChangePassword { get; set; }

        public DateTime LastClick { get; set; }

        #endregion

        public string PageTitle { get; set; }

        public static implicit operator CurrentUserContext(ThirdPartyUserContext v)
        {
            throw new NotImplementedException();
        }
    }


    public class ThirdPartyUserContext
    {
        public string ProviderName { get; set; } //= "GOOGLE";
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Locale { get; set; }
        public string Name { get; set; }
        public string ProviderUserId { get; set; }

    }
}
