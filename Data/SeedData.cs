using CNPM_LIBRARY_MANAGEMENT.Data.Models;
using CNPM_LIBRARY_MANAGEMENT.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CNPM_LIBRARY_MANAGEMENT.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            using var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());

            // Tự động tạo bảng nếu chưa có
            await context.Database.MigrateAsync();

            // Chạy seed theo thứ tự (bảng cha trước, bảng con sau)
            SeedLoaiSanPham(context);
            SeedTacGia(context);
            SeedTheLoaiSach(context);
            SeedNhaXuatBan(context);
            SeedNhaSanXuat(context);
            SeedThuongHieu(context);
            SeedChatLieu(context);
            SeedMauSac(context);
            SeedLoaiVppham(context);
            await context.SaveChangesAsync();

            SeedSanPham(context);
            await context.SaveChangesAsync();

            SeedChiTietSach(context);
            SeedChiTietVpp(context);
            await context.SaveChangesAsync();

            await SeedAccounts(context);
        }

        // ===== DANH MỤC =====

        private static void SeedLoaiSanPham(AppDbContext context)
        {
            if (context.LoaiSanPhams.Any()) return;
            context.LoaiSanPhams.AddRange(
                new LoaiSanPham {  TenLoaiSp = "Sách" },
                new LoaiSanPham {  TenLoaiSp = "Văn phòng phẩm" }
            );
        }

        private static void SeedTacGia(AppDbContext context)
        {
            if (context.TacGia.Any()) return;
            context.TacGia.AddRange(
                new TacGium { TenTacGia = "Nguyễn Nhật Ánh" },
                new TacGium { TenTacGia = "Tô Hoài" },
                new TacGium { TenTacGia = "Dale Carnegie" },
                new TacGium { TenTacGia = "Nam Quốc Chấn" }
            );
        }

        private static void SeedTheLoaiSach(AppDbContext context)
        {
            if (context.TheLoaiSaches.Any()) return;
            context.TheLoaiSaches.AddRange(
                new TheLoaiSach { TenTheLoai = "Văn học" },
                new TheLoaiSach { TenTheLoai = "Thiếu nhi" },
                new TheLoaiSach { TenTheLoai = "Kỹ năng sống" },
                new TheLoaiSach { TenTheLoai = "Kinh tế" }
            );
        }

        private static void SeedNhaXuatBan(AppDbContext context)
        {
            if (context.NhaXuatBans.Any()) return;
            context.NhaXuatBans.AddRange(
                new NhaXuatBan { TenNxb = "NXB Trẻ" },
                new NhaXuatBan { TenNxb = "NXB Kim Đồng" },
                new NhaXuatBan { TenNxb = "NXB Giáo Dục" },
                new NhaXuatBan { TenNxb = "NXB Tổng hợp TP.HCM" }
            );
        }

        private static void SeedNhaSanXuat(AppDbContext context)
        {
            if (context.NhaSanXuats.Any()) return;
            context.NhaSanXuats.AddRange(
                new NhaSanXuat { TenNsx = "Thiên Long" },
                new NhaSanXuat { TenNsx = "Bến Nghé" },
                new NhaSanXuat { TenNsx = "Deli" }
            );
        }

        private static void SeedThuongHieu(AppDbContext context)
        {
            if (context.ThuongHieus.Any()) return;
            context.ThuongHieus.AddRange(
                new ThuongHieu { TenThuongHieu = "Thiên Long" },
                new ThuongHieu { TenThuongHieu = "Staedtler" },
                new ThuongHieu { TenThuongHieu = "Deli" }
            );
        }

        private static void SeedChatLieu(AppDbContext context)
        {
            if (context.ChatLieus.Any()) return;
            context.ChatLieus.AddRange(
                new ChatLieu { TenChatLieu = "Nhựa" },
                new ChatLieu { TenChatLieu = "Kim loại" },
                new ChatLieu {  TenChatLieu = "Gỗ" }
            );
        }

        private static void SeedMauSac(AppDbContext context)
        {
            if (context.MauSacs.Any()) return;
            context.MauSacs.AddRange(
                new MauSac { TenMauSac = "Đỏ" },
                new MauSac { TenMauSac = "Xanh dương" },
                new MauSac { TenMauSac = "Đen" },
                new MauSac { TenMauSac = "Trắng" }
            );
        }

        private static void SeedLoaiVppham(AppDbContext context)
        {
            if (context.LoaiVpphams.Any()) return;
            context.LoaiVpphams.AddRange(
                new LoaiVppham { TenLoaiVpp = "Bút viết" },
                new LoaiVppham { TenLoaiVpp = "Tập vở" },
                new LoaiVppham { TenLoaiVpp = "Dụng cụ học tập" }
            );
        }

        // ===== SẢN PHẨM =====

        private static void SeedSanPham(AppDbContext context)
        {
            if (context.SanPhams.Any()) return;
            context.SanPhams.AddRange(
                // Sách (LoaiSanPhamId = 1)
                new SanPham
                {
                    TenSanPham = "Tôi thấy hoa vàng trên cỏ xanh",
                    GiaBan = 85000, LoaiSanPhamId = 1, SoLuongDaBan = 120,
                    MoTa = "Tiểu thuyết của Nguyễn Nhật Ánh, câu chuyện về tuổi thơ đầy cảm xúc.",
                    HinhAnh = "hoa-vang.jpg", DanhGia = 5, NgayDang = DateOnly.FromDateTime(DateTime.Now),
                    HangMoiVe = false, PhanTramGiam = 10
                },
                new SanPham
                {
                    TenSanPham = "Dế Mèn Phiêu Lưu Ký",
                    GiaBan = 65000, LoaiSanPhamId = 1, SoLuongDaBan = 200,
                    MoTa = "Tác phẩm kinh điển của nhà văn Tô Hoài dành cho thiếu nhi.",
                    HinhAnh = "de-men.jpg", DanhGia = 5, NgayDang = DateOnly.FromDateTime(DateTime.Now),
                    HangMoiVe = false, PhanTramGiam = 0
                },
                new SanPham
                {
                    TenSanPham = "Đắc Nhân Tâm",
                    GiaBan = 110000, LoaiSanPhamId = 1, SoLuongDaBan = 350,
                    MoTa = "Cuốn sách kỹ năng giao tiếp bán chạy nhất mọi thời đại.",
                    HinhAnh = "dac-nhan-tam.jpg", DanhGia = 5, NgayDang = DateOnly.FromDateTime(DateTime.Now),
                    HangMoiVe = true, PhanTramGiam = 5
                },
                new SanPham
                {
                    TenSanPham = "Nhà Giả Kim",
                    GiaBan = 79000, LoaiSanPhamId = 1, SoLuongDaBan = 180,
                    MoTa = "Tiểu thuyết của Paulo Coelho về hành trình theo đuổi ước mơ.",
                    HinhAnh = "nha-gia-kim.jpg", DanhGia = 4, NgayDang = DateOnly.FromDateTime(DateTime.Now),
                    HangMoiVe = false, PhanTramGiam = 0
                },
                new SanPham
                {
                    TenSanPham = "Tuổi Thơ Dữ Dội",
                    GiaBan = 72000, LoaiSanPhamId = 1, SoLuongDaBan = 95,
                    MoTa = "Tiểu thuyết của Phùng Quán về những thiếu niên anh hùng.",
                    HinhAnh = "tuoi-tho.jpg", DanhGia = 4, NgayDang = DateOnly.FromDateTime(DateTime.Now),
                    HangMoiVe = true, PhanTramGiam = 0
                },

                // Văn phòng phẩm (LoaiSanPhamId = 2)
                new SanPham
                {
                    TenSanPham = "Bút bi Thiên Long TL-027",
                    GiaBan = 5000, LoaiSanPhamId = 2, SoLuongDaBan = 500,
                    MoTa = "Bút bi cao cấp, mực đều, viết trơn tru.",
                    HinhAnh = "but-bi-tl027.jpg", DanhGia = 4, NgayDang = DateOnly.FromDateTime(DateTime.Now),
                    HangMoiVe = false, PhanTramGiam = 0
                },
                new SanPham
                {
                    TenSanPham = "Tập học sinh 200 trang Bến Nghé",
                    GiaBan = 18000, LoaiSanPhamId = 2, SoLuongDaBan = 300,
                    MoTa = "Tập học sinh 200 trang, giấy trắng, kẻ ngang.",
                    HinhAnh = "tap-hoc-sinh.jpg", DanhGia = 4, NgayDang = DateOnly.FromDateTime(DateTime.Now),
                    HangMoiVe = false, PhanTramGiam = 0
                },
                new SanPham
                {
                    TenSanPham = "Thước kẻ nhựa 30cm Deli",
                    GiaBan = 8000, LoaiSanPhamId = 2, SoLuongDaBan = 150,
                    MoTa = "Thước kẻ nhựa trong suốt, độ chính xác cao.",
                    HinhAnh = "thuoc-ke.jpg", DanhGia = 3, NgayDang = DateOnly.FromDateTime(DateTime.Now),
                    HangMoiVe = true, PhanTramGiam = 0
                },
                new SanPham
                {
                    TenSanPham = "Truyện tranh Doraemon",
                    GiaBan = 20000, LoaiSanPhamId = 1, SoLuongDaBan = 50,
                    MoTa = "Truyện tranh chú mèo máy Doraemon đến từ tương lai.",
                    HinhAnh = "doraemon.jpg", DanhGia = 5, NgayDang = DateOnly.FromDateTime(DateTime.Now),
                    HangMoiVe = true, PhanTramGiam = 10
                }
            );
        }

        // ===== CHI TIẾT SÁCH =====

        private static void SeedChiTietSach(AppDbContext context)
        {
            if (context.ChiTietSaches.Any()) return;
            context.ChiTietSaches.AddRange(
                new ChiTietSach
                {
                    SanPhamId = 1, TheLoaiId = 1, TacGiaId = 1,
                    NhaXuatBanId = 1, SoTrang = 317,
                    LoaiBia = "Bìa mềm", NgayPhatHanh = new DateOnly(2015, 6, 1),
                    KichThuoc = "13 x 19 cm"
                },
                new ChiTietSach
                {
                    SanPhamId = 2, TheLoaiId = 2, TacGiaId = 2,
                    NhaXuatBanId = 2, SoTrang = 208,
                    LoaiBia = "Bìa mềm", NgayPhatHanh = new DateOnly(2010, 1, 1),
                    KichThuoc = "13 x 19 cm"
                },
                new ChiTietSach
                {
                    SanPhamId = 3, TheLoaiId = 3, TacGiaId = 3,
                    NhaXuatBanId = 4, SoTrang = 320,
                    LoaiBia = "Bìa cứng", NgayPhatHanh = new DateOnly(2016, 3, 1),
                    KichThuoc = "14 x 20.5 cm"
                },
                new ChiTietSach
                {
                    SanPhamId = 4, TheLoaiId = 1, TacGiaId = 4,
                    NhaXuatBanId = 1, SoTrang = 228,
                    LoaiBia = "Bìa mềm", NgayPhatHanh = new DateOnly(2013, 5, 1),
                    KichThuoc = "13 x 20 cm"
                },
                new ChiTietSach
                {
                    SanPhamId = 5, TheLoaiId = 1, TacGiaId = null,
                    NhaXuatBanId = 3, SoTrang = 456,
                    LoaiBia = "Bìa mềm", NgayPhatHanh = new DateOnly(2009, 9, 1),
                    KichThuoc = "13 x 19 cm"
                },
                new ChiTietSach
                {
                    SanPhamId = 9, TheLoaiId = 2, TacGiaId = null,
                    NhaXuatBanId = 2, SoTrang = 150,
                    LoaiBia = "Bìa mềm", NgayPhatHanh = new DateOnly(2020, 1, 1),
                    KichThuoc = "13 x 19 cm"
                }
            );
        }

        // ===== CHI TIẾT VĂN PHÒNG PHẨM =====

        private static void SeedChiTietVpp(AppDbContext context)
        {
            if (context.ChiTietVpps.Any()) return;
            context.ChiTietVpps.AddRange(
                new ChiTietVpp
                {
                    SanPhamId = 6, IdloaiVpp = 1,
                    ThuongHieuId = 1, NhaSanXuatId = 1
                },
                new ChiTietVpp
                {
                    SanPhamId = 7, IdloaiVpp = 2,
                    ThuongHieuId = null, NhaSanXuatId = 2
                },
                new ChiTietVpp
                {
                    SanPhamId = 8, IdloaiVpp = 3,
                    ThuongHieuId = 3, NhaSanXuatId = 3
                }
            );
        }

        // ===== TÀI KHOẢN =====

        private static async Task SeedAccounts(AppDbContext context)
        {
            if (context.Accounts.Any()) return;

            PasswordHelper.CreatePasswordHash("Admin@123", out byte[] adminHash, out byte[] adminSalt);
            PasswordHelper.CreatePasswordHash("User@123", out byte[] userHash, out byte[] userSalt);
            PasswordHelper.CreatePasswordHash("User@123", out byte[] user2Hash, out byte[] user2Salt);

            context.Accounts.AddRange(
                new Account
                {
                    Gmail = "admin@gmail.com",
                    TenNguoiDung = "Quản trị viên",
                    PasswordHash = adminHash,
                    PasswordSalt = adminSalt,
                    Role = "Admin",
                    NgayTao = DateTime.Now
                },
                new Account
                {
                    Gmail = "user1@gmail.com",
                    TenNguoiDung = "Nguyễn Văn A",
                    PasswordHash = userHash,
                    PasswordSalt = userSalt,
                    Role = "User",
                    NgayTao = DateTime.Now
                },
                new Account
                {
                    Gmail = "user2@gmail.com",
                    TenNguoiDung = "Trần Thị B",
                    PasswordHash = user2Hash,
                    PasswordSalt = user2Salt,
                    Role = "User",
                    NgayTao = DateTime.Now
                }
            );
            await context.SaveChangesAsync();
        }
    }
}
