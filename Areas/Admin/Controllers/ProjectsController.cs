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
    public class ProjectsController : Controller
    {
        private AlluringDecorsEntities1 db = new AlluringDecorsEntities1();

        // GET: Admin/Projects
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
            var project = from s in db.Projects select s; //  students là biến tạo ra db.Students (Students) ở đây là 1 bảng trong database select s ( s là tên đặt rút gọn đặt gì cũng đc)
            if (!String.IsNullOrEmpty(search)) // check nếu search string có thì in ra hoặc không thì không in ra
            {
                project = project.Where(s => s.ProjectName.Contains(search)); // contains là để check xem lastname hoặc firstName có chứa search string ở trên 

            }
            // phần sort Dùng switchcase 
            switch (sortOrder)
            {
                case "name desc":
                    project = project.OrderByDescending(s => s.ProjectName); // các case tương đương với các cột muốn sort
                    break;
          
                default:
                    project = project.OrderBy(s => s.ProjectName);
                    break;
            }
            int pageSize = 4; //  khai báo số lượng record trên 1 page
            int pageNumber = (page ?? 1); // page number là page đang chọn nếu không chọn sẽ = 1
            return View(project.ToPagedList(pageNumber, pageSize)); // cuối cùng dùng hàm để trả về biến được gọi ở trên là 1 mảng giá trị 
        }

        // GET: Admin/Projects/Details/5
        public ActionResult Details(int? id)
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

        // GET: Admin/Projects/Create
        public ActionResult Create()
        {
            ViewBag.DesignTypeID = new SelectList(db.DesignTypes, "ID", "DesignTypeName");
            ViewBag.ImageID = new SelectList(db.Images, "ID", "ProjectImage");
            ViewBag.ProjectCategory_ID = new SelectList(db.ProjectCategories, "id", "CategoryName");
            return View();
        }

        // POST: Admin/Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,ProjectName,Area,Price,TotalPrice,Address,ImageID,Status,DesignTypeID,ProjectCategory_ID")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DesignTypeID = new SelectList(db.DesignTypes, "ID", "DesignTypeName", project.DesignTypeID);
            ViewBag.ImageID = new SelectList(db.Images, "ID", "ProjectImage", project.ImageID);
            ViewBag.ProjectCategory_ID = new SelectList(db.ProjectCategories, "id", "CategoryName", project.ProjectCategory_ID);
            return View(project);
        }

        // GET: Admin/Projects/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.DesignTypeID = new SelectList(db.DesignTypes, "ID", "DesignTypeName", project.DesignTypeID);
            ViewBag.ImageID = new SelectList(db.Images, "ID", "ProjectImage", project.ImageID);
            ViewBag.ProjectCategory_ID = new SelectList(db.ProjectCategories, "id", "CategoryName", project.ProjectCategory_ID);
            return View(project);
        }

        // POST: Admin/Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,ProjectName,Area,Price,TotalPrice,Address,ImageID,Status,DesignTypeID,ProjectCategory_ID")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DesignTypeID = new SelectList(db.DesignTypes, "ID", "DesignTypeName", project.DesignTypeID);
            ViewBag.ImageID = new SelectList(db.Images, "ID", "ProjectImage", project.ImageID);
            ViewBag.ProjectCategory_ID = new SelectList(db.ProjectCategories, "id", "CategoryName", project.ProjectCategory_ID);
            return View(project);
        }

        // GET: Admin/Projects/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Admin/Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
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
