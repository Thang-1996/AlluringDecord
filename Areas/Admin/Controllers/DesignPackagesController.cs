using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AlluringDecors.Models;
using PagedList;

namespace AlluringDecors.Areas.Admin.Controllers
{
    public class DesignPackagesController : Controller
    {
        private AlluringDecorsEntities1 db = new AlluringDecorsEntities1();

        // GET: Admin/DesignPackages
        public ViewResult Index(string sortOrder, string search, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            if (search != null)
            {
                page = 1; // nếu search có giá trị trả về page = 1
            }
            else
            {
                search = currentFilter; //  nếu có thì render phần dữ liệu search ra
            }
            ViewBag.CurrentFilter = search;
            var designpackage = from s in db.DesignPackages select s; //  students là biến tạo ra db.Students (Students) ở đây là 1 bảng trong database select s ( s là tên đặt rút gọn đặt gì cũng đc)
            if (!String.IsNullOrEmpty(search)) // check nếu search string có thì in ra hoặc không thì không in ra
            {
                designpackage = designpackage.Where(s => s.PackageName.Contains(search)); // contains là để check xem lastname hoặc firstName có chứa search string ở trên 

            }
            // phần sort Dùng switchcase 
            switch (sortOrder)
            {
                case "name desc":
                    designpackage = designpackage.OrderByDescending(s => s.PackageName); // các case tương đương với các cột muốn sort
                    break;
               
                default:
                    designpackage = designpackage.OrderBy(s => s.PackageName);
                    break;
            }
            int pageSize = 4; //  khai báo số lượng record trên 1 page
            int pageNumber = (page ?? 1); // page number là page đang chọn nếu không chọn sẽ = 1
            return View(designpackage.ToPagedList(pageNumber, pageSize)); // cuối cùng dùng hàm để trả về biến được gọi ở trên là 1 mảng giá trị 
        }

        // GET: Admin/DesignPackages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DesignPackage designPackage = db.DesignPackages.Find(id);
            if (designPackage == null)
            {
                return HttpNotFound();
            }
            return View(designPackage);
        }

        // GET: Admin/DesignPackages/Create
        public ActionResult Create()
        {
            ViewBag.ProjectID = new SelectList(db.Projects, "id", "ProjectName");
            return View();
        }

        // POST: Admin/DesignPackages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,PackageName,Price,ProjectID")] DesignPackage designPackage)
        {
            if (ModelState.IsValid)
            {
                db.DesignPackages.Add(designPackage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectID = new SelectList(db.Projects, "id", "ProjectName", designPackage.ProjectID);
            return View(designPackage);
        }

        // GET: Admin/DesignPackages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DesignPackage designPackage = db.DesignPackages.Find(id);
            if (designPackage == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "id", "ProjectName", designPackage.ProjectID);
            return View(designPackage);
        }

        // POST: Admin/DesignPackages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,PackageName,Price,ProjectID")] DesignPackage designPackage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(designPackage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "id", "ProjectName", designPackage.ProjectID);
            return View(designPackage);
        }

        // GET: Admin/DesignPackages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DesignPackage designPackage = db.DesignPackages.Find(id);
            if (designPackage == null)
            {
                return HttpNotFound();
            }
            return View(designPackage);
        }

        // POST: Admin/DesignPackages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DesignPackage designPackage = db.DesignPackages.Find(id);
            db.DesignPackages.Remove(designPackage);
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
