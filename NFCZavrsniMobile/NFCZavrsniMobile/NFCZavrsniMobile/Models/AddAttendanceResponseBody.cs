using System;
using System.Collections.Generic;
using System.Text;

namespace NFCZavrsniMobile.Models
{
    public class AddAttendanceResponseBody
    {
        public string ConfirmationToken { get; set; }
        public string NfcContentUploaded { get; set; }
        public long Id { get; set; }

        public AddAttendanceResponseBody()
        {
        }

        public AddAttendanceResponseBody(string confirmationToken, string nfcContentUploaded, long id)
        {
            ConfirmationToken = confirmationToken;
            NfcContentUploaded = nfcContentUploaded;
            Id = id;
        }
    }

   
}
