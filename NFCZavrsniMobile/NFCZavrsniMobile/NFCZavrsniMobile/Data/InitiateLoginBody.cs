using System;
using System.Collections.Generic;
using System.Text;

namespace NFCZavrsniMobile.Data
{
    public class InitiateLogInBody
    {
        public string PhoneID { get; set; }
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Creates body of http post request
        /// </summary>
        public InitiateLogInBody(string id, string number)
        {
            PhoneID = id;
            PhoneNumber = number;
        }
    }

}
