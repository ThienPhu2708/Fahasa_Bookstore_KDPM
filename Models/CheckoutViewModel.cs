using System.Collections.Generic;

namespace CNPM_LIBRARY_MANAGEMENT.Models
{
    public class CheckoutViewModel
    {
        // Danh sách sản phẩm trong giỏ để hiển thị
        public List<CartItemViewModel> CartItems { get; set; } = new List<CartItemViewModel>();

        // Thông tin để form gửi đi
        public OrderViewModel OrderInfo { get; set; } = new OrderViewModel();
    }
}