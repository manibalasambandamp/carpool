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
    public class RequestsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Requests
        public ActionResult Index()
        {
            var requests = db.Requests.Where(r => r.requestor == User.Identity.Name);
            return View(requests.ToList());
        }

        // GET: Requests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.FirstOrDefault(r => r.requestor == User.Identity.Name && r.Id ==id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // GET: Requests/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Requests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,fromAddress,fromZip,toAddress,toZip,isDaily,startDate,endDate,startTime")] Request request)
        {
            if (ModelState.IsValid)
            {
                request.requestor = User.Identity.Name;
                db.Requests.Add(request);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(request);
        }

        // GET: Requests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.FirstOrDefault(r => r.requestor == User.Identity.Name && r.Id == id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // POST: Requests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,fromAddress,fromZip,toAddress,toZip,isDaily,startDate,endDate,startTime")] Request request)
        {
            if (ModelState.IsValid)
            {
                request.requestor = User.Identity.Name;
                db.Entry(request).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(request);
        }

        // GET: Requests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.FirstOrDefault(r => r.requestor == User.Identity.Name && r.Id == id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // POST: Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Request request = db.Requests.FirstOrDefault(r => r.requestor == User.Identity.Name && r.Id == id);
            db.Requests.Remove(request);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Search
        public ActionResult Search(string fromAddress, string fromZip, String toAddress, String toZip, String poolDate)
        {
            try
            {
                // flag if search has been performed
                bool searchPerformed = false;

                var requests = db.Requests.Where(p => p.requestor != User.Identity.Name && p.isAccepted == false);

                if (!string.IsNullOrWhiteSpace(fromAddress))
                {
                    requests = requests.Where(p => p.fromAddress == fromAddress);
                    searchPerformed = true;
                }

                if (!string.IsNullOrWhiteSpace(fromZip))
                {
                    requests = requests.Where(p => p.fromZip == fromZip);
                    searchPerformed = true;
                }

                if (!string.IsNullOrWhiteSpace(toAddress))
                {
                    requests = requests.Where(p => p.toAddress == toAddress);
                    searchPerformed = true;
                }

                if (!string.IsNullOrWhiteSpace(toZip))
                {
                    requests = requests.Where(p => p.toZip == toZip);
                    searchPerformed = true;
                }

                if (!string.IsNullOrWhiteSpace(poolDate))
                {
                    DateTime poolDateValue = DateTime.Parse(poolDate);
                    requests = requests.Where(p => p.startDate == poolDateValue);
                    searchPerformed = true;
                }

                if (searchPerformed)
                {
                    // return search results
                    return View(requests.ToList());
                }
                else
                {
                    // return empty list
                    return View(new List<Request>());
                }
            }
            catch (Exception e)
            {
                return View(new List<Request>());
            }
        }


        public ActionResult Offer(int? id)
        {
            Request request = db.Requests.FirstOrDefault(r => r.Id == id);
            request.isAccepted = true;
            db.Entry(request).State = EntityState.Modified;
            db.SaveChanges();

            Pool pool = new Pool();
            pool.fromAddress = request.fromAddress;
            pool.fromZip = request.fromZip;
            pool.toAddress = request.toAddress;
            pool.toZip = request.toZip;
            pool.startDate = request.startDate;
            pool.endDate = request.endDate;
            pool.isDaily = request.isDaily;
            pool.startTime = request.startTime;

           // return RedirectToAction( "Create", "Pools",pool);

            return View( "../Pools/Create", pool);
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
