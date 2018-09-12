namespace NFCZavrsniMobile.Data
{
    public class LogInBody
    {
        public string Grant_type { get; set; } = "password";
        public string PhoneID { get; set; }
        public string PhoneNumber { get; set; }
        public string Token { get; set; }

        public LogInBody(string id, string number, string token)
        {
            PhoneID = id;
            PhoneNumber = number;
            Token = token;
        }
    }
}
