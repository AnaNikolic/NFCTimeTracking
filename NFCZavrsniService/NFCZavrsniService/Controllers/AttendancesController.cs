using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.AspNet.Identity.Owin;
using NFCZavrsniService.Models;
using NFCZavrsniService.Models.Entity;

namespace NFCZavrsniService.Controllers
{
    [Authorize]
    public class AttendancesController : ApiController
    {
        private ModelNFCZavrsniService db = new ModelNFCZavrsniService();

        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public AttendancesController(ApplicationUserManager applicationUserManager)
        {
            UserManager = applicationUserManager;
        }
        
        public AttendancesController()
        {
        }

        // GET: api/Attendances
        public IQueryable<Attendance> GetAttendance()
        {
            var user = UserManager.Users.Where(x => x.Email == User.Identity.Name).First();
            return db.Attendance.Where(x => x.Employee == db.Employee.Where(y => y.PhoneNumber == user.PhoneNumber).First().ID);
        }

        // GET: api/Attendances/5
        [ResponseType(typeof(Attendance))]
        public async Task<IHttpActionResult> GetAttendance(long id)
        {
            Attendance attendance = await db.Attendance.FindAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }

            return Ok(attendance);
        }

        // PUT: api/Attendances/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutAttendance(long id, Attendance attendance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != attendance.ID)
            {
                return BadRequest();
            }

            db.Entry(attendance).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttendanceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
        
        // POST: api/Attendances
        [HttpPost]
        [ActionName("AddAttendance")]
        [Route("api/Attendances/AddAttendance", Name = "AddAttendanceRoute")]
        [ResponseType(typeof(AttendanceResponseModel))]
        public async Task<IHttpActionResult> AddAttendance(AttendanceModel attendanceModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = UserManager.Users.Where(x => x.Email == User.Identity.Name).First();
            var employee = db.Employee.Where(y => y.Email == user.Email).First();
            var tag = db.Tag.Where(x => x.SerialNumber == attendanceModel.SerialNumber).First();
            if (tag.TagContent != attendanceModel.NFCContentRead)
            {
                tag.TagContent = attendanceModel.NFCContentUploaded;
                await db.SaveChangesAsync();
                return BadRequest("Tag content does not match.");
            }
            if (employee.Working != true)
            {
                tag.TagContent = attendanceModel.NFCContentUploaded;
                await db.SaveChangesAsync();
                return BadRequest("Not allowed, employee not working.");
            }
            Attendance attendance = new Attendance
            {
                Employee = employee.ID,
                Tag = tag.ID,
                DateTime = DateTime.Now,
                NFCContentRead = attendanceModel.NFCContentRead,
                Confirmed = true,
                NFCContentUploaded = attendanceModel.NFCContentUploaded
            };
            tag.TagContent = attendanceModel.NFCContentUploaded;
            db.Attendance.Add(attendance);
            await db.SaveChangesAsync();
            AttendanceResponseModel attendanceResponseModel = new AttendanceResponseModel
            {
                ID = attendance.ID,
                TagInfo = tag.Client1.Name + "/" + tag.Location1.Name + "/" + tag.TypeOfAttendance1.Name,
                EmployeeInfo = employee.Email
            };
            return Ok(attendanceResponseModel);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }
            }
            base.Dispose(disposing);
        }

        private bool AttendanceExists(long id)
        {
            return db.Attendance.Count(e => e.ID == id) > 0;
        }
    }
}