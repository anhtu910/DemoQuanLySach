using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppDemo.Models;

namespace WebAppDemo.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        DemoMVCEntities1 db = new DemoMVCEntities1();
        public ActionResult Register()
        {
            if (!Session["UserAdmin"].Equals(""))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Register(FormCollection fided)
        {
         
            string strerror1 = "";
            string fullName = fided["firtname"]+" "+fided["lastname"];
            string acc = fided["acc"];
            string password = fided["password"];
            string password2 = fided["password2"];
            object[] parms =
                {
                new SqlParameter("@ma",fided["acc"])
                };
            var ketqua = db.Users.SqlQuery("EXEC searchuser @ma", parms).FirstOrDefault();
            if (ketqua != null)
            {
                strerror1 = "Tên đăng nhập đã tồn tại";
            }
            else
            {
                if (password.Equals(password2))
                {
                    var modle = new User();
                    int res = modle.Canxxi(fullName, acc, password);
                    if(res>0)
                    return RedirectToAction("Login", "Auth");
                    else
                        strerror1 = "Đã xảy ra lỗi ngoài ý muốn !";
                }
                else
                {
                    strerror1 = "Mật khẩu xác nhận không đúng";
                }
            }

            ViewBag.Error = strerror1;
            return View();
        }
        public ActionResult Login()
        {
			if (!Session["UserAdmin"].Equals(""))
			{
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Error = "";
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection fied)
        {
            string strerror = "";
            string username = fied["username"];
            string password = fied["password"];
            object[] parms =
                {
                new SqlParameter("@ma",fied["username"])
                };
            var ketqua = db.Users.SqlQuery("EXEC searchuser @ma", parms).FirstOrDefault();
			if (ketqua==null)
			{
                strerror = "Tên đăng nhập không tồn tại";
			}
            else
			{
				if (ketqua.passwordd.Equals(password))
				{
                    object[] parms2 =
               {
                new SqlParameter("@id",ketqua.UserGroupID)
                };
                    var group = db.UsersGroups.SqlQuery("EXEC TimGR @id",parms2).FirstOrDefault();

                    Session["UserPermission"] = group.GroupPermissions;
					Session["UserAdmin"] = ketqua.Username;
					Session["UserPassword"] = ketqua.passwordd;
					Session["FullName"] = ketqua.Fullname;
                    Session["group"] = ketqua.UserGroupID;

                    return RedirectToAction("Index", "Home");
				}
				else
				{
                    strerror = "Mật khẩu không đúng";
				}                    
			}


            ViewBag.Error = strerror;
            return View();
        }

		

		public ActionResult logout()
        {
            Session["UserAdmin"] = "";
            Session["UserPassword"] = "";
            Session["FullName"] ="";
            Session["UserPermission"] = "";
            return RedirectToAction("Login", "Auth");
        }
        public ActionResult changePassword()
		{

            return View();
		}
        [HttpPost]
        public ActionResult changePassword(FormCollection fied)
        {
            var strerror1 = "";
           
			if (!Session["UserPassword"].Equals(fied["oldpassword"]))
			{
                strerror1 = "Mật khẩu không đúng";
            }
			else
			{
				if (!fied["newpassword"].Equals(fied["password2"]))
				{
                    strerror1 = "Mật khẩu xác nhận không đúng";
                }
				else
				{
                    Object[] parms5 =
            {
                new SqlParameter("@ma",Session["UserAdmin"]),
                 new SqlParameter("@pass",fied["newpassword"])
                };
                   var a = db.Database.ExecuteSqlCommand("updatepass @ma,@pass", parms5);
                    return RedirectToAction("Index", "Home");

                }
			}
            ViewBag.Error = strerror1;
            return View();
        }
        public ActionResult profile()
		{
            if (System.Web.HttpContext.Current.Session["UserAdmin"].Equals(""))
            {
                System.Web.HttpContext.Current.Response.Redirect("~/admin/login");

            }
            return View();
		}
        [HttpPost]
        public ActionResult changeName(FormCollection changepass)
        {

            if (System.Web.HttpContext.Current.Session["UserAdmin"].Equals(""))
            {
                System.Web.HttpContext.Current.Response.Redirect("~/admin/login");

            }
            var i = changepass["Sua"].ToString();
            object[] parms =
              {
                new SqlParameter("@ma",Session["UserAdmin"]),
                new SqlParameter("@newName",changepass["Sua"])
                };
            var ketqua = db.Database.ExecuteSqlCommand("EXEC ChangePass @ma,@newName", parms);
            Session["FullName"] = i;


            return RedirectToAction("profile", "Auth");
        }
       
    }
}