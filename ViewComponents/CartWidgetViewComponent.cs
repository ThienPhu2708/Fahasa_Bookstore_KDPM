using Microsoft.AspNetCore.Mvc;
using CNPM_LIBRARY_MANAGEMENT.Models;
using CNPM_LIBRARY_MANAGEMENT.Helpers; 
using System.Linq; 

namespace CNPM_LIBRARY_MANAGEMENT.ViewComponents
{
    [ViewComponent(Name = "CartWidget")]
    public class CartWidgetViewComponent : ViewComponent
    {
        // ViewComponent có thể truy cập HttpContext trực tiếp, không cần Inject IHttpContextAccessor cũng được
        public IViewComponentResult Invoke()
        {
            // 1. Lấy danh sách sản phẩm từ Session "Cart" (Đúng key mà Controller đang dùng)
            // Lưu ý: Phải có namespace CNPM_LIBRARY_MANAGEMENT.Helpers mới dùng được .Get<List<...>>
            var cart = HttpContext.Session.Get<List<CartItemViewModel>>("Cart");

            // 2. Tính tổng số lượng
            // Nếu giỏ hàng null (chưa mua gì) thì trả về 0
            // Nếu có hàng, dùng Sum để cộng dồn số lượng của từng món (ví dụ: mua 2 món, mỗi món 5 cái => kết quả là 10)
            var totalQuantity = cart != null ? cart.Sum(item => item.SoLuong) : 0;

            // 3. Trả về con số tổng cho View
            return View(totalQuantity);
        }
    }
}