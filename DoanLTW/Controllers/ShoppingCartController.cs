using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoanLTW.Models;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Data.Entity;
using static System.Web.Razor.Parser.SyntaxConstants;
namespace DoanLTW.Controllers
{
    public class ShoppingCartController : Controller
    {   
        private TCGShopDBContext dBContext = new TCGShopDBContext();
        private string strCart = "Cart";
        // GET: ShoppingCart
        public ActionResult Index() //Hiển thị giỏ hàng 
        {
            return View();
        }
        public ActionResult OrderNow(int ? Id)//Thêm sản phẩm vào giỏ hàng
        {
            if(Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Nếu giỏ hàng chưa tồn tại trong Session, tạo một giỏ hàng mới và thêm sản phẩm vào.
            if (Session[strCart] == null)
            {
                List<Cart> ListCart = new List<Cart>()
                {
                    new Cart(dBContext.Products.Find(Id),1)
                };
                Session[strCart] = ListCart;
            }
            else //Nếu giỏ hàng đã tồn tại
            {
                List<Cart> ListCart = (List<Cart>)Session[strCart];//lấy giỏ hàng hiện tại từ Session và gán nó vào biến ListCart.
                int check = IsExistingCheck(Id);
                if (check == -1)//Nếu chưa có thêm mới 
                    ListCart.Add(new Cart(dBContext.Products.Find(Id), 1));
                else//Nếu đã có tăng số lượng.
                    ListCart[check].Quantity++;
                Session[strCart] = ListCart;    
            }
            return RedirectToAction("Index");

        }
        private int IsExistingCheck(int? Id)//Kiểm tra sản phẩm có tồn tại trong giỏ hàng 
        {
            List<Cart> ListCart = (List<Cart>)Session[strCart];
            for (int i = 0; i < ListCart.Count; i++)
            {
                if (ListCart[i].Product.ProId == Id)
                {
                    return i;
                }
            }
            return -1;
        }
        public ActionResult RemoveItem(int? Id)//Xóa sản phẩm khỏi giỏ hàng
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int check = IsExistingCheck(Id);
            List<Cart> ListCart = (List<Cart>)Session[strCart];
            //Nếu giỏ hàng trống sau khi xóa, đặt Session về null.
            if (ListCart.Count == 0)
            {
                Session[strCart] = null;
            }
            //Nếu không, cập nhật danh sách sau khi xóa sản phẩm.
            else
            {
                ListCart.RemoveAt(check);
                Session[strCart] = ListCart;
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult UpdateCart(FormCollection field)//Cập nhật số lượng sản phẩm trong giỏ 
        {
            //Lấy danh sách số lượng sản phẩm từ form 
            string[] quantities = field.GetValues("quantity");
            List<Cart> ListCart = (List<Cart>)Session[strCart];
            //Cập nhật số lượng từng sản phẩm trong danh sách giỏ hàng.
            for (int i = 0; i < ListCart.Count; i++)
            {
                ListCart[i].Quantity = Convert.ToInt32(quantities[i]);
            }
            Session[strCart] = ListCart;
            return RedirectToAction("Index");
        }
        public ActionResult ClearCart()//Xóa toàn bộ giỏ hàng 
        {
            Session[strCart] = null;
            return RedirectToAction("Index");
        }

        public ActionResult Checkout()//Thanh toán 
        {
            return View();
        }
        [HttpPost]
        //Xử lý đơn hàng từ giỏ hàng và lưu vào cơ sở dữ liệu.
        public ActionResult ProcessOrder(FormCollection field)
        {

            List<Cart> ListCart = (List<Cart>)Session[strCart];
            //Tạo một đối tượng Order chứa thông tin khách hàng và trạng thái đơn hàng.
            var order = new DoanLTW.Models.Order()
            {
                CustomerName = field["cusName"],
                CustomerPhone = field["cusPhone"],
                CustomerEmail = field["cusEmail"],
                CustomerAddress = field["cusAddress"],
                OrderDate = DateTime.Now,
                PaymentType = "Cash",
                Status = "Processing"
            };
            dBContext.Orders.Add(order);
            dBContext.SaveChanges();
            //Duyệt qua từng sản phẩm trong giỏ hàng.
            foreach (Cart cart in ListCart)
            {
                OrderDetail orderDetail = new OrderDetail()
                {
                    OrderId = order.OrderId,
                    ProductId = cart.Product.ProId,
                    Quantity = Convert.ToInt32(cart.Quantity),
                    Price = Convert.ToDecimal(cart.Product.Price)
                };
                order.OrderName = "MD" + order.OrderId;
                dBContext.OrderDetails.Add(orderDetail);
                dBContext.SaveChanges();
            }

            Session.Remove(strCart);

            return View("OrderSuccess");
        }
    }
}