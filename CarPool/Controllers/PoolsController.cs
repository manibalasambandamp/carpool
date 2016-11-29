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
            var hostedPools = db.Pools.Where(p => p.host == User.Identity.Name);
            var joinedPools = db.Pools.Where(p => p.members.Contains(User.Identity.Name));
            IEnumerable<Pool> poolList = new List<Pool>();
            poolList = poolList.Concat(hostedPools.ToList());
            poolList = poolList.Concat(joinedPools.ToList());

            return View(poolList);
        }


        // GET: Pools/Details/5
        public ActionResult Details(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pool pool = db.Pools.FirstOrDefault(p =>p.Id == id);
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
                pool.members = db.Pools.FirstOrDefault(p => p.Id == pool.Id).members;
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

        // GET: Search
        public ActionResult Search(string fromAddress, string fromZip,String toAddress,String toZip,String poolDate)
        {
            try
            {
                // flag if search has been performed
                bool searchPerformed = false;

                var pools = db.Pools.Where(p => p.host != User.Identity.Name && (p.members == null || !p.members.Contains(User.Identity.Name)));

                if (!string.IsNullOrWhiteSpace(fromAddress))
                {
                    pools = pools.Where(p => p.fromAddress == fromAddress );
                    searchPerformed = true;
                }

                if (!string.IsNullOrWhiteSpace(fromZip))
                {
                    pools = pools.Where(p => p.fromZip == fromZip );
                    searchPerformed = true;
                }

                if (!string.IsNullOrWhiteSpace(toAddress))
                {
                    pools = pools.Where(p => p.toAddress == toAddress );
                    searchPerformed = true;
                }

                if (!string.IsNullOrWhiteSpace(toZip))
                {
                    pools = pools.Where(p => p.toZip == toZip);
                    searchPerformed = true;
                }

                if (!string.IsNullOrWhiteSpace(poolDate))
                {
                    DateTime poolDateValue = DateTime.Parse(poolDate);
                    pools = pools.Where(p => p.startDate == poolDateValue);
                    searchPerformed = true;
                }

                if (searchPerformed)
                {
                    // return search results
                    return View(pools.ToList());
                }
                else
                {
                    // return empty list
                    return View(new List<Pool>());
                }
            }
            catch (Exception e)
            {
                return View(new List<Pool>());
            }
        }

        public ActionResult Join(int? id) {
            Pool pool = db.Pools.FirstOrDefault(p => p.Id == id);
            var user=db.Users.FirstOrDefault(p => p.UserName == User.Identity.Name);
            String memberDetail = (User.Identity.Name + "," + user.PhoneNum);
            pool.members = (String.IsNullOrWhiteSpace(pool.members)) ? memberDetail :( pool.members + "#" + memberDetail);
            db.Entry(pool).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Details", new { id = pool.Id });
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
