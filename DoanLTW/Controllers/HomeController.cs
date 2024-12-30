using DoanLTW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;

namespace DoanLTW.Controllers
{
    // HomeController
    public class HomeController : Controller
    {
        private TCGShopDBContext dBContext = new TCGShopDBContext();

        public ActionResult Index(int? id)
        {
            // Danh mục mặc định cho phần Bán chạy nhất
            int bestSellerCategoryId = 1; // Đặt thành ID danh mục cụ thể cho Bán chạy nhấ

            int recommendedItemsCategoryId = 2;

            // Tìm kiếm sản phẩm bán chạy nhất
            List<Product> bestSellerProducts = dBContext.Products
                                                        .Include(p => p.Category)
                                                        .Where(p => p.CatId == bestSellerCategoryId)
                                                        .Take(8)
                                                        .ToList();

            // Tìm nạp các mặt hàng được đề xuất Sản phẩm
            List<Product> recommendedProducts = dBContext.Products
                                                        .Include(p => p.Category)
                                                        .Where(p => p.CatId == recommendedItemsCategoryId)
                                                        .Take(8)
                                                        .ToList();

            // Chuyển cả hai danh sách vào chế độ xem
            ViewBag.BestSellerProducts = bestSellerProducts;
            ViewBag.RecommendedProducts = recommendedProducts;

            return View();
        }
    }

}
