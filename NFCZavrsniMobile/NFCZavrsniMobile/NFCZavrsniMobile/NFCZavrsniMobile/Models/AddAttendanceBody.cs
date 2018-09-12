using System;
using System.Collections.Generic;
using System.Text;

namespace NFCZavrsniMobile.Models
{
    public class AddAttendanceBody
    {
        public string SerialNumber { get; set; }
        public string NFCContentRead { get; set; }
        public string NFCContentUploaded { get; set; }

        public AddAttendanceBody(string serialNumber, string nfcContentRead, string nfcContentUploaded)
        {
            SerialNumber = serialNumber;
            NFCContentRead = nfcContentRead;
            NFCContentUploaded = nfcContentUploaded;
        }

        public AddAttendanceBody()
        {
        }
    }

}
