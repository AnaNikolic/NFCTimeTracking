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
    public class TagsController : Controller
    {
        private ModelNFCZavrsniWeb db = new ModelNFCZavrsniWeb();

        // GET: Tags
        public async Task<ActionResult> Index()
        {
            var tag = db.Tag.Include(t => t.Client1).Include(t => t.Location1).Include(t => t.TypeOfAttendance1);
            return View(await tag.ToListAsync());
        }

        // GET: Tags/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag tag = await db.Tag.FindAsync(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View(tag);
        }

        // GET: Tags/Create
        public ActionResult Create()
        {
            ViewBag.Client = new SelectList(db.Client, "ID", "Name");
            ViewBag.Location = new SelectList(db.Location, "ID", "Name");
            ViewBag.TypeOfAttendance = new SelectList(db.TypeOfAttendance, "ID", "Name");
            return View();
        }

        // POST: Tags/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Location,TypeOfAttendance,Client,GPS,TagContent,SerialNumber")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                db.Tag.Add(tag);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Client = new SelectList(db.Client, "ID", "Name", tag.Client);
            ViewBag.Location = new SelectList(db.Location, "ID", "Name", tag.Location);
            ViewBag.TypeOfAttendance = new SelectList(db.TypeOfAttendance, "ID", "Name", tag.TypeOfAttendance);
            return View(tag);
        }

        // GET: Tags/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag tag = await db.Tag.FindAsync(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            ViewBag.Client = new SelectList(db.Client, "ID", "Name", tag.Client);
            ViewBag.Location = new SelectList(db.Location, "ID", "Name", tag.Location);
            ViewBag.TypeOfAttendance = new SelectList(db.TypeOfAttendance, "ID", "Name", tag.TypeOfAttendance);
            return View(tag);
        }

        // POST: Tags/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Location,TypeOfAttendance,Client,GPS,TagContent,SerialNumber")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tag).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Client = new SelectList(db.Client, "ID", "Name", tag.Client);
            ViewBag.Location = new SelectList(db.Location, "ID", "Name", tag.Location);
            ViewBag.TypeOfAttendance = new SelectList(db.TypeOfAttendance, "ID", "Name", tag.TypeOfAttendance);
            return View(tag);
        }

        // GET: Tags/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tag tag = await db.Tag.FindAsync(id);
            if (tag == null)
            {
                return HttpNotFound();
            }
            return View(tag);
        }

        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Tag tag = await db.Tag.FindAsync(id);
            db.Tag.Remove(tag);
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
