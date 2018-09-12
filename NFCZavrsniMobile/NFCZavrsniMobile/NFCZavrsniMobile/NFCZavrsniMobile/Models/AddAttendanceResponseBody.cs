using System;
using System.Collections.Generic;
using System.Text;

namespace NFCZavrsniMobile.Models
{
    public class AddAttendanceResponseBody
    {
        public long ID { get; set; }

        public string TagInfo { get; set; }

        public string EmployeeInfo { get; set; }

        public AddAttendanceResponseBody()
        {
        }

        public AddAttendanceResponseBody(long id, string tagInfo, string employeeInfo)
        {
            ID = id;
            TagInfo = tagInfo;
            EmployeeInfo = employeeInfo;
        }
    }

   
}
