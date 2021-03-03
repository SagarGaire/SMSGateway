using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SMSGateway.Models;

namespace SMSGateway.Controllers
{
    public class AccessLogsController : Controller
    {
        private SMSGateway_DevEntities db = new SMSGateway_DevEntities();

        // GET: AccessLogs
        public ActionResult Index()
        {
            var accessLogs = db.AccessLogs.Include(a => a.Saf);
            return View(accessLogs.ToList());
        }

        // GET: AccessLogs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccessLogs accessLogs = db.AccessLogs.Find(id);
            if (accessLogs == null)
            {
                return HttpNotFound();
            }
            return View(accessLogs);
        }

        // GET: AccessLogs/Create
        public ActionResult Create()
        {
            ViewBag.UserName = new SelectList(db.Saf, "Username", "Password");
            return View();
        }

        // POST: AccessLogs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LogId,LogDateTime,UserName,LoggedFrom")] AccessLogs accessLogs)
        {
            if (ModelState.IsValid)
            {
                db.AccessLogs.Add(accessLogs);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserName = new SelectList(db.Saf, "Username", "Password", accessLogs.UserName);
            return View(accessLogs);
        }

        // GET: AccessLogs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccessLogs accessLogs = db.AccessLogs.Find(id);
            if (accessLogs == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserName = new SelectList(db.Saf, "Username", "Password", accessLogs.UserName);
            return View(accessLogs);
        }

        // POST: AccessLogs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LogId,LogDateTime,UserName,LoggedFrom")] AccessLogs accessLogs)
        {
            if (ModelState.IsValid)
            {
                db.Entry(accessLogs).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserName = new SelectList(db.Saf, "Username", "Password", accessLogs.UserName);
            return View(accessLogs);
        }

        // GET: AccessLogs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccessLogs accessLogs = db.AccessLogs.Find(id);
            if (accessLogs == null)
            {
                return HttpNotFound();
            }
            return View(accessLogs);
        }

        // POST: AccessLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AccessLogs accessLogs = db.AccessLogs.Find(id);
            db.AccessLogs.Remove(accessLogs);
            db.SaveChanges();
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
