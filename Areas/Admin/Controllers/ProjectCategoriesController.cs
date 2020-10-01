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
    public class ProjectCategoriesController : Controller
    {
        private AlluringDecorsEntities1 db = new AlluringDecorsEntities1();

        // GET: Admin/ProjectCategories
        public ViewResult Index(string sortOrder, string search, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date"; // sort mỗi theo tên thì không cần dòng này
            if (search != null)
            {
                page = 1; // nếu search có giá trị trả về page = 1
            }
            else
            {
                search = currentFilter; //  nếu có thì render phần dữ liệu search ra
            }
            ViewBag.CurrentFilter = search;
            var projectcategory = from s in db.ProjectCategories select s; //  students là biến tạo ra db.Students (Students) ở đây là 1 bảng trong database select s ( s là tên đặt rút gọn đặt gì cũng đc)
            if (!String.IsNullOrEmpty(search)) // check nếu search string có thì in ra hoặc không thì không in ra
            {
                projectcategory = projectcategory.Where(s => s.CategoryName.Contains(search)); // contains là để check xem lastname hoặc firstName có chứa search string ở trên 

            }
            // phần sort Dùng switchcase 
            switch (sortOrder)
            {
                case "name desc":
                    projectcategory = projectcategory.OrderByDescending(s => s.CategoryName); // các case tương đương với các cột muốn sort
                    break;
               
                default:
                    projectcategory = projectcategory.OrderBy(s => s.CategoryName);
                    break;
            }
            int pageSize = 4; //  khai báo số lượng record trên 1 page
            int pageNumber = (page ?? 1); // page number là page đang chọn nếu không chọn sẽ = 1
            return View(projectcategory.ToPagedList(pageNumber, pageSize)); // cuối cùng dùng hàm để trả về biến được gọi ở trên là 1 mảng giá trị 
        }

        // GET: Admin/ProjectCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectCategory projectCategory = db.ProjectCategories.Find(id);
            if (projectCategory == null)
            {
                return HttpNotFound();
            }
            return View(projectCategory);
        }

        // GET: Admin/ProjectCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/ProjectCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,CategoryName")] ProjectCategory projectCategory)
        {
            if (ModelState.IsValid)
            {
                db.ProjectCategories.Add(projectCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(projectCategory);
        }

        // GET: Admin/ProjectCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectCategory projectCategory = db.ProjectCategories.Find(id);
            if (projectCategory == null)
            {
                return HttpNotFound();
            }
            return View(projectCategory);
        }

        // POST: Admin/ProjectCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,CategoryName")] ProjectCategory projectCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(projectCategory);
        }

        // GET: Admin/ProjectCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProjectCategory projectCategory = db.ProjectCategories.Find(id);
            if (projectCategory == null)
            {
                return HttpNotFound();
            }
            return View(projectCategory);
        }

        // POST: Admin/ProjectCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProjectCategory projectCategory = db.ProjectCategories.Find(id);
            db.ProjectCategories.Remove(projectCategory);
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
