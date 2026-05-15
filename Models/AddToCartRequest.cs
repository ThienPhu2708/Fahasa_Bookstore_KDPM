namespace CNPM_LIBRARY_MANAGEMENT.Models
{
    public class AddToCartRequest
    {
        public int SanPhamId { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
