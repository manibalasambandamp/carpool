using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarPool.Models;

namespace CarPool.Controllers
{
    [Authorize]
    public class PoolsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Pools
        public ActionResult Index()
        {
            var pools = db.Pools.Where(p => p.host == User.Identity.Name);
            return View(pools.ToList());
        }

        // GET: Pools/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pool pool = db.Pools.FirstOrDefault(p=>p.host == User.Identity.Name && p.Id == id);
            if (pool == null)
            {
                return HttpNotFound();
            }
            return View(pool);
        }

        // GET: Pools/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pools/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,fromAddress,fromZip,toAddress,toZip,carType,carNumber,seatsToOffer,isDaily,startDate,endDate,startTime")] Pool pool)
        {
            if (ModelState.IsValid)
            {
                pool.host = User.Identity.Name;
                db.Pools.Add(pool);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pool);
        }

        // GET: Pools/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pool pool = db.Pools.FirstOrDefault(p => p.host == User.Identity.Name && p.Id == id);
            if (pool == null)
            {
                return HttpNotFound();
            }
            return View(pool);
        }

        // POST: Pools/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,fromAddress,fromZip,toAddress,toZip,carType,carNumber,seatsToOffer,isDaily,startDate,endDate,startTime")] Pool pool)
        {
            bool usersPool = db.Pools.Any(p => p.host == User.Identity.Name && p.Id == pool.Id);
            if (!usersPool) {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                pool.host = User.Identity.Name;
                db.Entry(pool).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pool);
        }

        // GET: Pools/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pool pool = db.Pools.FirstOrDefault(p => p.host == User.Identity.Name && p.Id == id);
            if (pool == null)
            {
                return HttpNotFound();
            }
            return View(pool);
        }

        // POST: Pools/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pool pool = db.Pools.FirstOrDefault(p => p.host == User.Identity.Name && p.Id == id);
            db.Pools.Remove(pool);
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
