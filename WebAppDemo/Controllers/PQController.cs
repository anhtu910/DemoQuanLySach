using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppDemo.Models;

namespace WebAppDemo.Controllers
{
    public class PQController : BaseController
    {
        DemoMVCEntities1 db = new DemoMVCEntities1();
        // GET: PQ
        public ActionResult PhanQuyen()
        {
			User anh = ckec_permission(new string[] { PERMISSIONS.HOST }, true);

			var ketqua = (from item in db.Users
                          where item.UserGroupID != "hostgroup"
                          select item
                               ).ToList();
            return View(ketqua);
        }

        public ActionResult EditPQ(string id)
        {

			User anh = ckec_permission(new string[] { PERMISSIONS.HOST }, true);
			if (id == null) return Content("<script language='javascript' type='text/javascript'>alert('Quay lại thôi bro');window.history.back();</script>");
			User us = db.Users.Find(id);
            ViewBag.UserGroupID = new SelectList(db.UsersGroups, "UniqueID", "UniqueID");
            return View(us);
        }
        [HttpPost]
        public ActionResult EditPQ(FormCollection PQ, User model)
        {
            var oldItem = db.Users.Find(model.Username);
            oldItem.UserGroupID = PQ["UserGroupID"];
            db.SaveChanges();

            return RedirectToAction("PhanQuyen", "PQ");
        }
    }
}