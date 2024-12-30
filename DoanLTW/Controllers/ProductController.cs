using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoanLTW.Models;
using PagedList;

namespace DoanLTW.Controllers
{

    public class ProductController : Controller
    {
        // GET: Product 
        private TCGShopDBContext dBContext = new TCGShopDBContext();

        public ActionResult Index(int? categoryId, int? page)
        {
            int pageSize = 6;
            int pageNumber = page ?? 1;
            // Lấy danh sách sản phẩm từ database
            IQueryable<Product> productsQuery = dBContext.Products.Include(p => p.Category);
            // Lọc theo categoryId nếu được truyền
            if (categoryId.HasValue && categoryId > 0)
            {
                productsQuery = productsQuery.Where(p => p.CatId == categoryId.Value); //categoryId.Value: Lấy giá trị thực từ biến nullable int?.
            }
            // Sắp xếp danh sách sản phẩm theo name
            productsQuery = productsQuery.OrderBy(p => p.ProName);
            // Áp dụng phân trang
            var product = productsQuery.ToPagedList(pageNumber, pageSize);
            // Truyền danh sách phân trang sang View
            return View(product);
        }

        public ActionResult Details(int id)
        {
            //Lấy một sản phẩm từ cơ sở dữ liệu dựa vào ProId, đồng thời nạp dữ liệu liên quan của bảng Category
            Product product = dBContext.Products.Include(cat => cat.Category).FirstOrDefault(p => p.ProId == id);
            
            var recommendedProducts = dBContext.Products //  san pham khuyen nghi
               .Where(p => p.CatId == product.CatId && p.ProId != product.ProId)//Chỉ lấy các sản phẩm có CatId giống với sản phẩm hiện tại và loại trừ sản phẩm hiện tại ra khoỉ danh sách.
               .Take(8) 
               .ToList();

            ViewBag.RecommendedProducts = recommendedProducts;
            return View(product);
        }
    }
}























//public class ProductController : Controller
//{
//    // GET: Product
//    private TCGShopDBContext dBContext = new TCGShopDBContext();

//    public ActionResult Index(int? categoryId, int? page)
//    {
//        int pageSize = 6;
//        int pageNumber = page ?? 1;
//        // Lấy danh sách sản phẩm từ database
//        IQueryable<Product> productsQuery = dBContext.Product.Include(p => p.Category);
//        // Lọc theo categoryId nếu được truyền
//        if (categoryId.HasValue && categoryId > 0)
//        {
//            productsQuery = productsQuery.Where(p => p.CatId == categoryId.Value);
//        }
//        // Sắp xếp danh sách sản phẩm 
//        productsQuery = productsQuery.OrderBy(p => p.ProName);
//        // Áp dụng phân trang
//        var product = productsQuery.ToPagedList(pageNumber, pageSize);
//        // Truyền danh sách phân trang sang View
//        return View(product);
//    }

//    public ActionResult Details(int id)
//    {
//        Product product = dBContext.Product.Include(cat => cat.Category).FirstOrDefault(p => p.ProId == id);
//        return View(product);
//    }
//}