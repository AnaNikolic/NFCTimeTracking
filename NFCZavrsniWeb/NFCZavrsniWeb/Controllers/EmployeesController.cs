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
using Microsoft.AspNet.Identity.Owin;
using NFCZavrsniWeb.Models;
using System.Configuration;
using Microsoft.AspNet.Identity;

namespace NFCZavrsniWeb.Controllers
{
    [NoDirectAccess]
    [Authorize(Roles = "SystemAdministrator, ClientAdministrator")]
    public class EmployeesController : Controller
    {
        private ModelNFCZavrsniWeb db = new ModelNFCZavrsniWeb();
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public EmployeesController()
        {
        }

        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? Request.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        // GET: Employees
        public async Task<ActionResult> Index()
        {
            IQueryable<Employee> employee;
            if (User.IsInRole("SystemAdministrator"))
            {
                employee = db.Employee.Include(e => e.Client1).Include(e => e.Person);
            }
            else
            {
                employee = db.Employee.Where(x => x.Client == db.Employee.Where(c => c.Email == User.Identity.Name).First().Client).Include(e => e.Client1).Include(e => e.Person);
            }
            return View(await employee.ToListAsync());
        }

        // GET: Employees/Details/5
        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = await db.Employee.FindAsync(id);
            if (!User.IsInRole("SystemAdministrator") && employee.Client != db.Employee.Where(c => c.Email == User.Identity.Name).FirstOrDefault().Client)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Cannot access requested employee due to invalid administrator role");
            }
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            if (!User.IsInRole("SystemAdministrator"))
            {
                ViewBag.Client = new SelectList(db.Client.Where(x => x.ID == db.Employee.Where(c => c.Email == User.Identity.Name).FirstOrDefault().Client), "ID", "Name");
            }
            else
            {
                ViewBag.Client = new SelectList(db.Client, "ID", "Name");
            }
            ViewBag.OIB = new SelectList(db.Person, "OIB", "FirstName");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,OIB,Client,Working,WorkPlace,EmploymentDate,Email,PhoneNumber,PhoneID")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = employee.Email, Email = employee.Email };
                var result = await UserManager.CreateAsync(user, ConfigurationManager.AppSettings["UserPassword"]);
                
                if (result.Succeeded)
                {
                    user = await UserManager.FindByNameAsync(employee.Email);
                    await UserManager.AddToRoleAsync(user.Id, "User");
                    await UserManager.SetPhoneNumberAsync(user.Id, employee.PhoneNumber);
                    await UserManager.SetTwoFactorEnabledAsync(user.Id, true);
                    var code = await UserManager.GenerateChangePhoneNumberTokenAsync(user.Id, employee.PhoneNumber);
                    result = await UserManager.ChangePhoneNumberAsync(user.Id, employee.PhoneNumber, code);
                    if (result.Succeeded)
                    {
                        db.Employee.Add(employee);
                        await db.SaveChangesAsync();

                        //var message = new IdentityMessage
                        //{
                        //    Destination = employee.PhoneNumber,
                        //    Body = "Your account has been succesfully created"
                        //};
                        //await UserManager.SmsService.SendAsync(message);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to verify phone");
                        if (!User.IsInRole("SystemAdministrator"))
                        {
                            ViewBag.Client = new SelectList(db.Client.Where(x => x.ID == db.Employee.Where(c => c.Email == User.Identity.Name).FirstOrDefault().Client), "ID", "Name");
                        }
                        else
                        {
                            ViewBag.Client = new SelectList(db.Client, "ID", "Name");
                        }
                        ViewBag.OIB = new SelectList(db.Person, "OIB", "FirstName", employee.OIB);
                        return View(employee);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Failed to create user");
                    if (!User.IsInRole("SystemAdministrator"))
                    {
                        ViewBag.Client = new SelectList(db.Client.Where(x => x.ID == db.Employee.Where(c => c.Email == User.Identity.Name).FirstOrDefault().Client), "ID", "Name");
                    }
                    else
                    {
                        ViewBag.Client = new SelectList(db.Client, "ID", "Name");
                    }
                    ViewBag.OIB = new SelectList(db.Person, "OIB", "FirstName", employee.OIB);
                    return View(employee);
                }
                return RedirectToAction("Index");
            }
            if (!User.IsInRole("SystemAdministrator"))
            {
                ViewBag.Client = new SelectList(db.Client.Where(x => x.ID == db.Employee.Where(c => c.Email == User.Identity.Name).FirstOrDefault().Client), "ID", "Name");
            }
            else
            {
                ViewBag.Client = new SelectList(db.Client, "ID", "Name");
            }
            ViewBag.OIB = new SelectList(db.Person, "OIB", "FirstName", employee.OIB);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = await db.Employee.FindAsync(id);
            if (!User.IsInRole("SystemAdministrator") && employee.Client != db.Employee.Where(c => c.Email == User.Identity.Name).FirstOrDefault().Client)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Cannot access requested client due to invalid administrator role");
            }
            if (employee == null)
            {
                return HttpNotFound();
            }
            if (!User.IsInRole("SystemAdministrator"))
            {
                ViewBag.Client = new SelectList(db.Client.Where(x => x.ID == db.Employee.Where(c => c.Email == User.Identity.Name).FirstOrDefault().Client), "ID", "Name");
            }
            else
            {
                ViewBag.Client = new SelectList(db.Client, "ID", "Name");
            }
            ViewBag.OIB = new SelectList(db.Person, "OIB", "FirstName", employee.OIB);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,OIB,Client,Working,WorkPlace,EmploymentDate,Email,PhoneNumber,PhoneID")] Employee employee)
        {

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = employee.Email, Email = employee.Email };
                var result = await UserManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    user = await UserManager.FindByNameAsync(employee.Email);
                    await UserManager.SetPhoneNumberAsync(user.Id, employee.PhoneNumber);
                    var code = await UserManager.GenerateChangePhoneNumberTokenAsync(user.Id, employee.PhoneNumber);
                    result = await UserManager.ChangePhoneNumberAsync(user.Id, employee.PhoneNumber, code);
                    if (result.Succeeded)
                    {
                        db.Entry(employee).State = EntityState.Modified;
                        await db.SaveChangesAsync();

                        //var message = new IdentityMessage
                        //{
                        //    Destination = employee.PhoneNumber,
                        //    Body = "Your account has been succesfully created"
                        //};
                        //await UserManager.SmsService.SendAsync(message);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to verify phone");
                        if (!User.IsInRole("SystemAdministrator"))
                        {
                            ViewBag.Client = new SelectList(db.Client.Where(x => x.ID == db.Employee.Where(c => c.Email == User.Identity.Name).FirstOrDefault().Client), "ID", "Name");
                        }
                        else
                        {
                            ViewBag.Client = new SelectList(db.Client, "ID", "Name");
                        }
                        ViewBag.OIB = new SelectList(db.Person, "OIB", "FirstName", employee.OIB);
                        return View(employee);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Failed to update user");
                    if (!User.IsInRole("SystemAdministrator"))
                    {
                        ViewBag.Client = new SelectList(db.Client.Where(x => x.ID == db.Employee.Where(c => c.Email == User.Identity.Name).FirstOrDefault().Client), "ID", "Name");
                    }
                    else
                    {
                        ViewBag.Client = new SelectList(db.Client, "ID", "Name");
                    }
                    ViewBag.OIB = new SelectList(db.Person, "OIB", "FirstName", employee.OIB);
                    return View(employee);
                }
                return RedirectToAction("Index");
            }
            if (!User.IsInRole("SystemAdministrator"))
            {
                ViewBag.Client = new SelectList(db.Client.Where(x => x.ID == db.Employee.Where(c => c.Email == User.Identity.Name).FirstOrDefault().Client), "ID", "Name");
            }
            else
            {
                ViewBag.Client = new SelectList(db.Client, "ID", "Name");
            }
            ViewBag.OIB = new SelectList(db.Person, "OIB", "FirstName", employee.OIB);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee employee = await db.Employee.FindAsync(id);
            if (!User.IsInRole("SystemAdministrator") && employee.Client != db.Employee.Where(c => c.Email == User.Identity.Name).FirstOrDefault().Client)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Cannot access requested client due to invalid administrator role");
            }
            if (employee == null)
            {
                return HttpNotFound();
            }
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            Employee employee = await db.Employee.FindAsync(id);
            var user = await UserManager.FindByNameAsync(employee.Email);
            
            await UserManager.RemoveFromRoleAsync(user.Id, "User");
            var result = await UserManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                db.Employee.Remove(employee);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Delete");
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
