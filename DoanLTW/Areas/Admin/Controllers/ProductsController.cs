
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using DoanLTW.Models;

namespace DoanLTW.Areas.Admin.Controllers
{
    
    public class ProductsController : Controller
    {
        private TCGShopDBContext db = new TCGShopDBContext();

        // GET: Admin/Products1
        //[Authorize(Roles = "Admin, User")]
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Category);
            return View(products.ToList());
        }

        // GET: Admin/Products1/Details/5
        //[Authorize(Roles = "Admin, User")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Products1/Create
        //[Authorize(Roles = "Admin, User")]
        public ActionResult Create()
        {
            ViewBag.CatId = new SelectList(db.Categories, "Id", "CatName");
            return View();
        }

        // POST: Admin/Products1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Admin, User")]
        public ActionResult Create([Bind(Include = "ProId,ProName,ProImage,SetName,ProNumber,Price,MarketPrice,CardType,AttackPower1,AttackPower2,CatId")] Product product, HttpPostedFileBase imgfile)
        {
            if (ModelState.IsValid)
            {
                product.ProImage = FileUpload(imgfile);
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CatId = new SelectList(db.Categories, "Id", "CatName", product.CatId);
            return View(product);
        }

        // GET: Admin/Products1/Edit/5
        //[Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CatId = new SelectList(db.Categories, "Id", "CatName", product.CatId);
            return View(product);
        }

        // POST: Admin/Products1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProId,ProName,ProImage,SetName,ProNumber,Price,MarketPrice,CardType,AttackPower1,AttackPower2,CatId")] Product product, HttpPostedFileBase imgfile)
        {
            if (ModelState.IsValid)
            {
                if (imgfile != null)
                {
                    product.ProImage = FileUpload(imgfile);
                }
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CatId = new SelectList(db.Categories, "Id", "CatName", product.CatId);
            return View(product);
        }

        // GET: Admin/Products1/Delete/5
        //[Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Products1/Delete/5
        //[Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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

        public string FileUpload(HttpPostedFileBase file)
        {
            string path = Server.MapPath("~/images/");
            string filename = Path.GetFileName(file.FileName);
            string fullPath = Path.Combine(path, filename);

            file.SaveAs(fullPath);
            return fullPath;
        }
    }
}
