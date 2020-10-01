using AlluringDecors.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace AlluringDecors.Controllers
{
    public class HomeController : Controller
    {
        private AlluringDecorsEntities1 db = new AlluringDecorsEntities1();
        // GET: Home
        public ViewResult Index()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Blog()
        {
            return View();
        }
        public ActionResult Service()
        {
            var project = db.Projects;
            return View(project.ToList());
        }
        public ActionResult DetailtProject(int ? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

    }
}