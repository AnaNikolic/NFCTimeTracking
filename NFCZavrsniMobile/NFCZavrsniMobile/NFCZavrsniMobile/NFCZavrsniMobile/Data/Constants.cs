namespace NFCZavrsniMobile.Data
{
    class Constants
    {
        public static readonly string RestURLBearerToken = "https://nfczavrsniservice.azurewebsites.net/token";
        public static readonly string RestURLLogout = "https://nfczavrsniservice.azurewebsites.net/api/Account/Logout";
        public static readonly string RestURLInitiateLogin = "https://nfczavrsniservice.azurewebsites.net/api/Account/InitiateLogin";
        public static readonly string RestURLAddAttendance = "https://nfczavrsniservice.azurewebsites.net/api/Attendances/AddAttendance";
        public static readonly string RestURLVerifyAttendance = "https://nfczavrsniservice.azurewebsites.net/api/Attendances/VerifyAttendance";
    }
}