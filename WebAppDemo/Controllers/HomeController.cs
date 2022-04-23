using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppDemo.Models;
using System.Collections.Specialized;
using System.IO;

namespace WebAppDemo.Controllers
{
	public class HomeController : BaseController
	{

		DemoMVCEntities1 db = new DemoMVCEntities1();
	
		public ActionResult Xem1(string ma)
		{
			User anh = ckec_permission(new string[] { PERMISSIONS.SACH_VIEW }, true);
			if (ma == null) return Content("<script language='javascript' type='text/javascript'>alert('Quay lại thôi bro');window.history.back();</script>");
			var context = new DemoMVCEntities1();
			var ketqua = context.Saches.Find(ma);
			return View(ketqua);
		}
		public ActionResult Index(string SearchString,string SearchDate)
		{
			User anh = ckec_permission(new string[] { PERMISSIONS.SACH_VIEW }, true);
			
			if (SearchString == null && SearchDate == null)
			{
				DemoMVCEntities1 listSach = new DemoMVCEntities1();
				var ketqua = (from item in listSach.Saches
							  where item.DaXoa == false

							  select item
							  ).ToList();
				return View(ketqua);
			}
			else if (SearchString != null && SearchDate == "")
			{
				using (var context = new DemoMVCEntities1())
				{
					object[] parms =
				{
					new SqlParameter("@TenTacGia",SearchString),
					new SqlParameter("@TenCuonSach",SearchString),
					new SqlParameter("@TenNhaXuatBan",SearchString)
					};
					var data = context.Saches.SqlQuery("EXEC TimKiemTheoTen @TenTacGia,@TenCuonSach,@TenNhaXuatBan", parms).ToList();
					return View(data);
				}
			}
			else if (SearchString == "" && SearchDate != null)
			{
				using (var context = new DemoMVCEntities1())
				{
					object[] parms =
				{
					new SqlParameter("@NgayTao",SearchDate),
					new SqlParameter("@NgayXuatBan",SearchDate)
					};
					var data = context.Saches.SqlQuery("EXEC TimKiemTheoNgay @NgayTao,@NgayXuatBan", parms).ToList();
					return View(data);
				}
			}
			else
			{
				using (var context = new DemoMVCEntities1())
				{
					object[] parms =
				{
					new SqlParameter("@TenTacGia",SearchString),
					new SqlParameter("@TenCuonSach",SearchString),
					new SqlParameter("@TenNhaXuatBan",SearchString),
					new SqlParameter("@NgayTao",SearchDate),
					new SqlParameter("@NgayXuatBan",SearchDate)
					};
					var data = context.Saches.SqlQuery("EXEC TimKiemTenNgay @TenTacGia,@TenCuonSach,@TenNhaXuatBan,@NgayTao,@NgayXuatBan", parms).ToList();
					return View(data);
				}
			}
			
		}
		public ActionResult hiem()
		{
			User anh = ckec_permission(new string[] { PERMISSIONS.SACH_VIEW }, true);
			
			DemoMVCEntities1 listSach = new DemoMVCEntities1();
			var ketqua = (from item in listSach.Saches
						  where item.DaXoa == true

						  select item
						  ).ToList();
			return View(ketqua);
		}
		public ActionResult anhtu8444()
		{
			return View();
		}

		[HttpPost]
		public ActionResult anhtu8444(PhanLoaiSach model)
		{
			User anh = ckec_permission(new string[] { PERMISSIONS.SACH_VIEW }, true);
			
			try
			{
				// TODO: Add insert logic here
				var context = new DemoMVCEntities1();

				context.PhanLoaiSaches.Add(model);
				context.SaveChanges();
				return RedirectToAction("PhanLoai");


			}
			catch
			{
				return View();
			}
		}

		
	}
}