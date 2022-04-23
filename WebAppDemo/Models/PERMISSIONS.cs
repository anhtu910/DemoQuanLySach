using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppDemo.Models
{
	public static class PERMISSIONS
	{
		public static string HOST= "admin-host";
		public static string PHANLOAI_ADD = "phanloai-add";
		public static string PHANLOAI_EDIT = "phanloai-edit";
		public static string PHANLOAI_DELETE = "phanloai-delete";
		public static string PHANLOAI_VIEW = "phanloai-view";

		public static string SACH_ADD = "sach-add";
		public static string SACH_EDIT = "sach-edit";
		public static string SACH_DELETE = "sach-delete";
		public static string SACH_VIEW = "sach-view";
		// lấy danh sách toàn bộ quyền dạng KEY-VALUE
		public static Dictionary<string, string> GetAllPermission()
		{
			Dictionary<string, string> list = new Dictionary<string, string>();
			//nhóm phân loại
			list.Add("group1", "Nhóm phân loại");
			list.Add(PHANLOAI_ADD, "Có thể thêm phân loại");
			list.Add(PHANLOAI_VIEW, "Có thể xem phân loại");
			list.Add(PHANLOAI_EDIT, "Có thể sửa phân loại");
			list.Add(PHANLOAI_DELETE, "Có thể xóa phân loại");
			//nhóm Sách
			list.Add("group2", "Nhóm sách");
			list.Add(SACH_ADD, "Có thể thêm sách");
			list.Add(SACH_VIEW, "Có thể xem sách");
			list.Add(SACH_EDIT, "Có thể sửa sách");
			list.Add(SACH_DELETE, "Có thể xóa sách");
			return list;
		}
		
		
	}
}