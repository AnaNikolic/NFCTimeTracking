using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NFCZavrsniWeb.Models.Entity;

namespace NFCZavrsniWeb.Controllers
{
    [NoDirectAccess]
    [Authorize(Roles = "SystemAdministrator, ClientAdministrator")]
    public class AttendancesController : Controller
    {
        private ModelNFCZavrsniWeb db = new ModelNFCZavrsniWeb();

        // GET: Attendances
        public async Task<ActionResult> Index()
        {
            IQueryable<Attendance> attendance;
            if (User.IsInRole("SystemAdministrator"))
            {
                attendance = db.Attendance.Include(a => a.Employee1).Include(a => a.Tag1);
            }
            else
            {
                int client = db.Employee.Where(c => c.Email == User.Identity.Name).First().Client;
                attendance = db.Attendance.Where(x => x.Employee == (db.Employee.Where(y => y.Client == client)).FirstOrDefault().ID)
                    .Include(a => a.Employee1).Include(a => a.Tag1);
            }
            return View(await attendance.ToListAsync());

        }

        // GET: Attendances/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = await db.Attendance.FindAsync(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            return View(attendance);
        }

        // GET: Attendances/Create
        public ActionResult Create()
        {
            ViewBag.Employee = new SelectList(db.Employee, "ID", "OIB");
            ViewBag.Tag = new SelectList(db.Tag, "ID", "TagContent");
            return View();
        }

        // POST: Attendances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Employee,Tag,DateTime,NFCContentRead,NFCContentUploaded,GPSLoction,Confirmed")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                db.Attendance.Add(attendance);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Employee = new SelectList(db.Employee, "ID", "OIB", attendance.Employee);
            ViewBag.Tag = new SelectList(db.Tag, "ID", "TagContent", attendance.Tag);
            return View(attendance);
        }

        // GET: Attendances/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = await db.Attendance.FindAsync(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            ViewBag.Employee = new SelectList(db.Employee, "ID", "EmployeeDetail", attendance.Employee);
            ViewBag.Tag = new SelectList(db.Tag, "ID", "TagDetail", attendance.Tag);
            return View(attendance);
        }

        // POST: Attendances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Employee,Tag,DateTime,NFCContentRead,NFCContentUploaded,GPSLoction,Confirmed")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attendance).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Employee = new SelectList(db.Employee, "ID", "OIB", attendance.Employee);
            ViewBag.Tag = new SelectList(db.Tag, "ID", "TagContent", attendance.Tag);
            return View(attendance);
        }

        // GET: Attendances/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attendance attendance = await db.Attendance.FindAsync(id);
            if (attendance == null)
            {
                return HttpNotFound();
            }
            return View(attendance);
        }

        // POST: Attendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Attendance attendance = await db.Attendance.FindAsync(id);
            db.Attendance.Remove(attendance);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
