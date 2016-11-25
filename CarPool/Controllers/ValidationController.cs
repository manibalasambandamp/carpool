using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarPool.Controllers
{
    public class ValidationController : Controller
    {
        [HttpPost]
        public JsonResult IsValidStartDate(string startDate)
        {
            var min = DateTime.Now.Date;
            var max = DateTime.Now.AddYears(1);
            var msg = string.Format("Please enter a value between {0:MM/dd/yyyy} and {1:MM/dd/yyyy}", min, max);
            try
            {
                var date = DateTime.Parse(startDate);
                if (date >= min && date <= max)
                    return Json(true);
                else
                    return Json(msg);
            }
            catch (Exception)
            {
                return Json(msg);
            }
        }

        [HttpPost]
        public JsonResult IsValidEndDate(string endDate)
        {
            var min = DateTime.Now.Date;
            var max = DateTime.Now.AddYears(1);
            var msg = string.Format("Please enter a value between {0:MM/dd/yyyy} and {1:MM/dd/yyyy}", min, max);
            try
            {
                var date = DateTime.Parse(endDate);
                if (date >= min && date <= max)
                    return Json(true);
                else
                    return Json(msg);
            }
            catch (Exception)
            {
                return Json(msg);
            }
        }
    }
}