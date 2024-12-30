using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DoanLTW
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Đăng ký các khu vực trong ứng dụng
            AreaRegistration.RegisterAllAreas();

            // Đăng ký các tuyến đường
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            // Cấu hình bộ lọc toàn cục
            GlobalFilters.Filters.Add(new HandleErrorAttribute());
        }

        protected void Application_Error()
        {
            // Xử lý lỗi toàn cục
            var exception = Server.GetLastError();
            Response.Clear();

            // Đăng nhập lỗi (nếu cần)
            // Ví dụ: LogError(exception);

            // Chuyển hướng đến trang lỗi
            Server.ClearError();
            Response.Redirect("~/Error");
        }

        protected void Application_BeginRequest()
        {
            // Thêm các tiêu đề bảo mật hoặc xử lý request nếu cần
            Response.Headers.Add("X-Content-Type-Options", "nosniff");
            Response.Headers.Add("X-XSS-Protection", "1; mode=block");
        }

        protected void Application_EndRequest()
        {
            // Xử lý khi một yêu cầu kết thúc (nếu cần)
            // Ví dụ: Kiểm tra trạng thái phản hồi
            if (Response.StatusCode == 404)
            {
                Response.Redirect("~/Error/NotFound");
            }
        }

    }
}
