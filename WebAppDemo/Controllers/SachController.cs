using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppDemo.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections.Specialized;
using PagedList;




namespace WebAppDemo.Controllers
{
	public class SachController : BaseController
    {
        private DemoMVCEntities1 db = new DemoMVCEntities1();
        // GET: Sach
        public ActionResult Chart()
		{
			DemoMVCEntities1 data = new DemoMVCEntities1();
			var ketqua = data.Chart1.SqlQuery("Exec chart").ToList();

			return View(ketqua);
            
       
		}
        public ActionResult Index(string ma, int? page)
		{
            User anh = ckec_permission(new string[] { PERMISSIONS.SACH_VIEW }, true);
            if (ma == null) return Content("<script language='javascript' type='text/javascript'>alert('Quay lại thôi bro');window.history.back();</script>");

            DemoMVCEntities1 listSach = new DemoMVCEntities1();
            if (page == null) page = 1;

            // 3. Tạo truy vấn, lưu ý phải sắp xếp theo trường nào đó, ví dụ OrderBy
            // theo LinkID mới có thể phân trang.
            

            // 4. Tạo kích thước trang (pageSize) hay là số Link hiển thị trên 1 trang
            int pageSize = 4;

            // 4.1 Toán tử ?? trong C# mô tả nếu page khác null thì lấy giá trị page, còn
            // nếu page = null thì lấy giá trị 1 cho biến pageNumber.
            int pageNumber = (page ?? 1);
           
                object[] parms =
            {
                    new SqlParameter("@ma",ma),
                    
                    };
                var data = listSach.Saches.SqlQuery("EXEC ListSach @ma", parms).ToList();
                return View(data.ToPagedList(pageNumber, pageSize));
            

           
        }
        public ActionResult Xem(string ma)
        {
            User anh = ckec_permission(new string[] { PERMISSIONS.SACH_VIEW }, true);
            if (ma == null) return Content("<script language='javascript' type='text/javascript'>alert('Quay lại thôi bro');window.history.back();</script>");
            var context = new DemoMVCEntities1();
            var ketqua = context.Saches.Find(ma);
            ketqua.DaXoa = false;
            context.SaveChanges();
           return RedirectToAction("Hiem","Home");
        }
        public ActionResult Edit(string id)
        {
            User anh = ckec_permission(new string[] { PERMISSIONS.SACH_EDIT }, true);
            if (id == null) return Content("<script language='javascript' type='text/javascript'>alert('Quay lại thôi bro');window.history.back();</script>");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sach sach = db.Saches.Find(id);
            if (sach == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaPhanLoai = new SelectList(db.PhanLoaiSaches, "Ma", "TenPhanLoai", sach.MaPhanLoai);
            return View(sach);
        }

        // POST: Saches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Ma,MaPhanLoai,Ten,HinhBia,TomTat,Link,NgayXuatBan,NhaXuatBan,TenTacGia,DaXoa,NgayTao,NguoiTao,NgaySua,NguoiSua,NgayXoa,NguoiXoa")] Sach sach, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string path = Path.Combine(Server.MapPath("~/"), Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    sach.HinhBia = "~/" + file.FileName;
                }
                db.Entry(sach).State = EntityState.Modified;
                    db.SaveChanges();
             
                return RedirectToAction("Xem",new {ma=sach.Ma });
            }
            ViewBag.MaPhanLoai = new SelectList(db.PhanLoaiSaches, "Ma", "TenPhanLoai", sach.MaPhanLoai);
            return View(sach);
        }

        // GET: Saches/Delete/5
        public ActionResult Delete(string id)
        {
            User anh = ckec_permission(new string[] { PERMISSIONS.SACH_DELETE }, true);
            if (id == null) return Content("<script language='javascript' type='text/javascript'>alert('Quay lại thôi bro');window.history.back();</script>");
            var context = new DemoMVCEntities1();
            var delete = context.Saches.Find(id);
            return View(delete);
        }
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection, Sach modele)
        {
            try
            {
                // TODO: Add delete logic here
                var context = new DemoMVCEntities1();
                var delete = context.Saches.Find(id);
                    delete.DaXoa = true;
                    delete.NgayXoa = DateTime.Now;
                    delete.NguoiXoa = modele.NguoiXoa;
                    context.SaveChanges();

              

                return RedirectToAction("Index","Home");

            }
            catch
            {
                return View();
            }
        }

     
        public ActionResult PhanLoai()
		{
            User anh = ckec_permission(new string[] { PERMISSIONS.PHANLOAI_VIEW }, true);
			

			DemoMVCEntities1 listPhanLoai = new DemoMVCEntities1();
              
                var ketqua = listPhanLoai.PhanLoaiSaches.SqlQuery("EXEC lisPhanLoai").ToList();
            
            return View(ketqua);
            
            
        }
       
        public ActionResult PhanLoaiDaXoa()
        {
            User anh = ckec_permission(new string[] { PERMISSIONS.PHANLOAI_VIEW }, true);
           
            DemoMVCEntities1 listPhanLoai = new DemoMVCEntities1();
            using (var context = new DemoMVCEntities1())
            {
              
                var ketqua = context.PhanLoaiSaches.SqlQuery("EXEC lisPhanLoaiDaXoa").ToList();
				if (ketqua.ToArray().Length==0)
				{
                    ketqua = null;
                    return RedirectToAction("PhanLoai");

                }
				else
                return View(ketqua);
            }
            
            
        }
        public ActionResult CreatePhanLoai()
        {
            User anh = ckec_permission(new string[] { PERMISSIONS.PHANLOAI_ADD }, true);
            

            return View();
        }

        [HttpPost]
        public ActionResult CreatePhanLoai(PhanLoaiSach model)
        {
            var context = new DemoMVCEntities1();
            
           
                // TODO: Add insert logic here
               if (context.PhanLoaiSaches.Find(model.Ma)==null)
			    {
                    context.PhanLoaiSaches.Add(model);
                    context.SaveChanges();
                    return RedirectToAction("PhanLoai");
                }
				else { return Content("<script language='javascript' type='text/javascript'>alert('Mã sách đã tồn tại');window.history.back();</script>"); }





        }
        public ActionResult EditPhanLoai(string ma)
        {
            User anh = ckec_permission(new string[] { PERMISSIONS.PHANLOAI_EDIT }, true);
            if (ma == null) return Content("<script language='javascript' type='text/javascript'>alert('Quay lại thôi bro');window.history.back();</script>");
            DemoMVCEntities1 context = new DemoMVCEntities1();
            var editing = context.PhanLoaiSaches.Find(ma);


            return View(editing);
        }

        // POST: Student/Edit/5
        [HttpPost]
        public ActionResult EditPhanLoai(PhanLoaiSach model)
        {
            try
            {
                // TODO: Add update logic here
                var context = new DemoMVCEntities1();
        
                var oldItem = context.PhanLoaiSaches.Find(model.Ma);
                int i = 0;


                var ketqua = (from item in context.Saches
                              where item.MaPhanLoai == model.Ma
                              select item).ToList();
                foreach (var a in ketqua)
                {
                    i = i + 1;
                }

                if (i == 0)
                {
                    oldItem.DaXoa = model.DaXoa;

                }
			
                oldItem.TenPhanLoai = model.TenPhanLoai;
                
                oldItem.NgayTao = model.NgayTao;
                oldItem.NguoiTao = model.NguoiTao;
                oldItem.NgaySua = model.NgaySua;
                oldItem.NguoiSua = model.NguoiSua;
                oldItem.NgayXoa = model.NgayXoa;
                oldItem.NguoiXoa = model.NguoiXoa;
                
                context.SaveChanges();


                return RedirectToAction("PhanLoai");
            }
            catch
            {
                return View();
            }
        }

        // GET: Student/Delete/5
        public ActionResult DeletePhanLoai(string ma)
        {
            User anh = ckec_permission(new string[] { PERMISSIONS.PHANLOAI_DELETE }, true);
            if (ma == null) return Content("<script language='javascript' type='text/javascript'>alert('Quay lại thôi bro');window.history.back();</script>");
            var context = new DemoMVCEntities1();
            var delete = context.PhanLoaiSaches.Find(ma);
            return View(delete);
        }
     
        public ActionResult RestorePhanLoai(string ma, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var context = new DemoMVCEntities1();
                var delete = context.PhanLoaiSaches.Find(ma);
                delete.DaXoa = false;
                delete.NgaySua = DateTime.Now;
                
                context.SaveChanges();
                return RedirectToAction("PhanLoai");
            }
            catch
            {
                return View();
            }
        }

        // POST: Student/Delete/5
        [HttpPost]
        public ActionResult DeletePhanLoai(string ma, FormCollection collection,PhanLoaiSach modele)
        {
            User anh = ckec_permission(new string[] { PERMISSIONS.PHANLOAI_DELETE }, true);
            if (ma == null) return Content("<script language='javascript' type='text/javascript'>alert('Quay lại thôi bro');window.history.back();</script>");
            try
            {
                // TODO: Add delete logic here
                var context = new DemoMVCEntities1();
                var  delete = context.PhanLoaiSaches.Find(ma);
                int i = 0;


                var ketqua = (from item in context.Saches
                             where item.MaPhanLoai == ma
                             select item).ToList();
				foreach (var a in ketqua )
				{
                    i = i + 1;
				}

                if (i == 0)
                {
                    delete.DaXoa = true;
                    delete.NgayXoa = DateTime.Now;
                    delete.NguoiXoa = Session["FullName"].ToString();
                    context.SaveChanges();

                }
          
                return RedirectToAction("PhanLoai");
                
            }
            catch
            {
                return View();
            }
        }
        public ActionResult CreateSach()
        {
            User anh = ckec_permission(new string[] { PERMISSIONS.SACH_ADD }, true);
           
            ViewBag.MaPhanLoai = new SelectList(db.PhanLoaiSaches, "Ma", "TenPhanLoai");
          
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSach(Sach sach, HttpPostedFileBase file)
        {
          
            if (ModelState.IsValid)
            {
                string path = Path.Combine(Server.MapPath("~/"), Path.GetFileName(file.FileName));
                file.SaveAs(path);
                db.Saches.Add(new Sach
                {
                    Ma = sach.Ma,
                    MaPhanLoai = sach.MaPhanLoai,
                    Ten = sach.Ten,
                    HinhBia = "~/" + file.FileName,
                    TomTat = sach.TomTat,
                    Link = sach.Link,
                    NgayXuatBan = sach.NgayXuatBan,
                    NhaXuatBan = sach.NhaXuatBan,
                    TenTacGia = sach.TenTacGia,
                    DaXoa = sach.DaXoa,
                    NgayTao = sach.NgayTao,
                    NguoiTao = sach.NguoiTao,
                    NgaySua = sach.NgaySua,
                    NguoiSua = sach.NguoiSua,
                    NgayXoa = sach.NgayXoa,
                    NguoiXoa = sach.NguoiXoa,
                });
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
              
            }

            ViewBag.MaPhanLoai = new SelectList(db.PhanLoaiSaches, "Ma", "TenPhanLoai", sach.MaPhanLoai);
            return View(sach);
        }

    }
}