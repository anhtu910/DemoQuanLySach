using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppDemo.Models;


namespace WebAppDemo.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        public BaseController()
        {
			if (System.Web.HttpContext.Current.Session["UserAdmin"].Equals(""))
			{
                System.Web.HttpContext.Current.Response.Redirect("~/admin/login");

            }
        }
        public User ckec_permission(string[] permission_to_check,bool check_and)
		{
          string a = (string)Session["UserPermission"];
            //kiem tra quyen and
			if (check_and)
			{
                foreach (string per in permission_to_check)
				{
                    if(!a.Contains(per)) {
                        //System.Web.HttpContext.Current.Response.Redirect("~/admin/login");
                        //Content("<script language='javascript' type='text/javascript'>alert('Mã sách đã tồn tại');window.history.back();</script>");
                        Response.Write("<script language='javascript' type='text/javascript'>alert('Bạn không có quyền truy cập');window.history.back();</script>");
                        return null;
                    }
				}
                return null;
			}
            return null;
            
        }
       
		
    }
}