using DoanLTW.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.Entity;
using System.Data;


namespace DoanLTW.Controllers
{
    public class AccountsController : Controller
    {
        // GET: Accounts
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserModel model)
        {
            using (TCGShopDBContext context = new TCGShopDBContext())
            {
                model.UserPassword = GetMD5(model.UserPassword);
                bool isValidUser = context.Users.Any(user => user.UserName.ToLower() ==
                    model.UserName.ToLower() && user.UserPassword == model.UserPassword);
                if (isValidUser)
                {
                    // Lấy thông tin user và role
                    var user = context.UserRolesMappings
                        .Include(urm => urm.User)
                        .Include(urm => urm.RoleMaster)
                        .FirstOrDefault(urm => urm.User.UserName == model.UserName);
                    FormsAuthentication.SetAuthCookie(model.UserName, false);
                    if (user.RoleID == 1 || user.RoleID == 2)
                    {                                                              
                            return RedirectToAction("Index", "Products", new  { area = "Admin" });
                        }
                        else 
                        {
                            return RedirectToAction("Index", "Home");
                        }                                         
                }
                ModelState.AddModelError("", "Invalid Username or Password");
            }
            return View();
        }
        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(User model)
        {
            using (TCGShopDBContext context = new TCGShopDBContext())
            {
                model.UserPassword = GetMD5(model.UserPassword);
                context.Users.Add(model);                    
                context.SaveChanges();
            }
            return RedirectToAction("Login");
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        public string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }

            return byte2String;
        }
        public static string GETSHA256(string str)
        {
            SHA256 sha256 = new SHA256CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = sha256.ComputeHash(fromData);
            string byte2String = null;
            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }
            return byte2String;
        }
    }
}