namespace NFCZavrsniMobile.Data
{

    /// <summary>
    /// Tijelo http post zahtjeva web servisu prilikom login autentifikacije
    /// </summary>
    public class LogInBody
    {
        public string Grant_type { get; set; } = "password";
        public string PhoneID { get; set; }
        public string PhoneNumber { get; set; }
        public string Token { get; set; }


        /// <summary>
        /// Kreira Tijelo http post zahtjeva web servisu prilikom login autentifikacije. Parametri su korisničko ime i lozika korisnika.
        /// </summary>
        public LogInBody(string id, string number, string token)
        {
            PhoneID = id;
            PhoneNumber = number;
            Token = token;
        }
    }
}
