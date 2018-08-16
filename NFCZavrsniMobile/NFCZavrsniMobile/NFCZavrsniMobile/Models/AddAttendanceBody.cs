using System;
using System.Collections.Generic;
using System.Text;

namespace NFCZavrsniMobile.Models
{
    public class AddAttendanceBody
    {
        public string SerialNumber { get; set; }
        public string NFCContentRead { get; set; }
        public string GPSLoction { get; set; }

        public AddAttendanceBody(string serialNumber, string nfcContentRead)
        {
            SerialNumber = serialNumber;
            NFCContentRead = nfcContentRead;
            GPSLoction = null;
        }

        public AddAttendanceBody()
        {
        }
    }

}
