using System;
using System.Collections.Generic;
using System.Text;

namespace NFCZavrsniMobile.Data
{
    public class InitiateLogInBody
    {
        public string PhoneID { get; set; }
        public string PhoneNumber { get; set; }

        public InitiateLogInBody(string id, string number)
        {
            PhoneID = id;
            PhoneNumber = number;
        }
    }

}
