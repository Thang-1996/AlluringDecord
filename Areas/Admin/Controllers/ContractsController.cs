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
    public class ContractsController : Controller
    {
        private AlluringDecorsEntities1 db = new AlluringDecorsEntities1();

        // GET: Admin/Contracts
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
            var contracts = from s in db.Contracts select s; //  students là biến tạo ra db.Students (Students) ở đây là 1 bảng trong database select s ( s là tên đặt rút gọn đặt gì cũng đc)
            if (!String.IsNullOrEmpty(search)) // check nếu search string có thì in ra hoặc không thì không in ra
            {
                contracts = contracts.Where(s => s.ContractName.Contains(search) || s.Project.ProjectName.Contains(search)); // contains là để check xem lastname hoặc firstName có chứa search string ở trên 

            }
            // phần sort Dùng switchcase 
            switch (sortOrder)
            {
                case "name desc":
                    contracts = contracts.OrderByDescending(s => s.ContractName); // các case tương đương với các cột muốn sort
                    break;
                case "date_desc":
                    contracts = contracts.OrderByDescending(s => s.ContractSigningDate);
                    break;
                case "Date":
                    contracts = contracts.OrderBy(s => s.ContractStartDate);
                    break;
                default:
                    contracts = contracts.OrderBy(s => s.ContractName);
                    break;
            }
            int pageSize = 4; //  khai báo số lượng record trên 1 page
            int pageNumber = (page ?? 1); // page number là page đang chọn nếu không chọn sẽ = 1
            return View(contracts.ToPagedList(pageNumber, pageSize)); // cuối cùng dùng hàm để trả về biến được gọi ở trên là 1 mảng giá trị 
        }

        // GET: Admin/Contracts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = db.Contracts.Find(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            return View(contract);
        }

        // GET: Admin/Contracts/Create
        public ActionResult Create()
        {
            ViewBag.ProjectID = new SelectList(db.Projects, "id", "ProjectName");
            return View();
        }

        // POST: Admin/Contracts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,ContractName,TotalPrice,ContractStartDate,ContractEndDate,ContractType,ContractStatus,ProjectID,ContractSigningDate")] Contract contract)
        {
            if (ModelState.IsValid)
            {
                db.Contracts.Add(contract);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProjectID = new SelectList(db.Projects, "id", "ProjectName", contract.ProjectID);
            return View(contract);
        }

        // GET: Admin/Contracts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = db.Contracts.Find(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "id", "ProjectName", contract.ProjectID);
            return View(contract);
        }

        // POST: Admin/Contracts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ContractName,TotalPrice,ContractStartDate,ContractEndDate,ContractType,ContractStatus,ProjectID,ContractSigningDate")] Contract contract)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contract).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProjectID = new SelectList(db.Projects, "id", "ProjectName", contract.ProjectID);
            return View(contract);
        }

        // GET: Admin/Contracts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contract contract = db.Contracts.Find(id);
            if (contract == null)
            {
                return HttpNotFound();
            }
            return View(contract);
        }

        // POST: Admin/Contracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contract contract = db.Contracts.Find(id);
            db.Contracts.Remove(contract);
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
