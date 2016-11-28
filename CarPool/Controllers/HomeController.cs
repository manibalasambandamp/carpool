using CarPool.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarPool.Controllers
{
    public class HomeController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            
            return View();
        }
        [AllowAnonymous]
        public ActionResult About()
        {
            string msg = "";
            try
            {
                HelloWorldServiceClient client = new HelloWorldServiceClient();
                msg = client.GetMessage("Mike Liu");
            }
            catch (Exception e)
            {
                ViewBag.Message = "Your application description page.";
                return View();
            }
            ViewBag.Message = msg;

            return View();
        }
        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [AllowAnonymous]
        public FileContentResult UserPhotos()
        {
            if (User.Identity.IsAuthenticated)
            {
                String userId = User.Identity.GetUserId();

                if (userId == null)
                {
                    return getDefaultPhoto();
                }
                // to get the user details to load user Image 
                var bdUsers = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                var userImage = bdUsers.Users.Where(x => x.Id == userId).FirstOrDefault();
                if (userImage == null) {
                    return getDefaultPhoto();
                }

                return new FileContentResult(userImage.ProfilePic, "image/jpeg");
            }
            else
            {
                return getDefaultPhoto();
            }
        }

        public FileContentResult getDefaultPhoto()
        {
            string fileName = HttpContext.Server.MapPath(@"~/Images/nobody.jpg");

            byte[] imageData = null;
            FileInfo fileInfo = new FileInfo(fileName);
            long imageFileLength = fileInfo.Length;
            FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            imageData = br.ReadBytes((int)imageFileLength);
            return File(imageData, "image/png");
        }
    }
}