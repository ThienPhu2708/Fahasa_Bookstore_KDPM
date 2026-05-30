# Nhật Ký Chỉnh Sửa Code (Walkthrough)

Báo cáo này tóm tắt các thay đổi đã thực hiện để khắc phục sự cố Crash ứng dụng liên quan đến quá trình khởi tạo dữ liệu mẫu (Seed Data) và database.

## 1. Khắc phục lỗi `IDENTITY_INSERT`

**Vấn Đề:**
- Hệ thống báo lỗi: `Cannot insert explicit value for identity column in table '[Tên_Bảng]' when IDENTITY_INSERT is set to OFF.`
- Xảy ra khi Entity Framework cố gắng chèn bản ghi vào database với ID gán sẵn, trong khi cột ID đã được định nghĩa là `IDENTITY` (tự động tăng) trong SQL Server.

**Các Thay Đổi Đã Thực Hiện:**
- **`ChatLieusController.cs`**: Xóa `"Id,"` khỏi Data Annotation `[Bind(...)]` trong phương thức `Create` để ngăn việc form vô tình đẩy giá trị Id lên khi thêm mới.
- **`Data/SeedData.cs`**: Xóa toàn bộ các thiết lập gán cứng thuộc tính `Id = ...` ra khỏi các thực thể khi gọi hàm `AddRange` để seed dữ liệu, bao gồm các bảng: `TacGium`, `TheLoaiSach`, `NhaXuatBan`, `NhaSanXuat`, `ThuongHieu`, `MauSac`, `LoaiVppham`, `SanPham`.
  
  ```diff
  - new TacGium { Id = 1, TenTacGia = "Nguyễn Nhật Ánh" }
  + new TacGium { TenTacGia = "Nguyễn Nhật Ánh" }
  ```

## 2. Khắc phục lỗi Xung Đột Khóa Ngoại (Foreign Key Constraint)

**Vấn Đề:**
- Sau khi sửa lỗi `IDENTITY_INSERT`, khi chạy ứng dụng tiếp tục xuất hiện lỗi: `The MERGE statement conflicted with the FOREIGN KEY constraint "FK__SanPham__LoaiSan__35BCFE0A".`
- **Nguyên nhân:** Bảng `SanPham` trong Seed Data đang gán cứng khóa ngoại (ví dụ: `LoaiSanPhamId = 1`). Tuy nhiên, do những lần chạy lỗi trước đó, bảng `LoaiSanPham` dưới database đã bị nhảy số thứ tự tự động tăng (ví dụ Id trở thành 3, 4 chứ không còn là 1, 2). Do đó, dữ liệu bảng `SanPham` khi được insert không tìm thấy khóa ngoại `LoaiSanPhamId = 1` ở bảng cha tương ứng.

**Các Thay Đổi Đã Thực Hiện:**
- Chạy lệnh SQL (thông qua SQLCMD) để **xóa hoàn toàn (Drop) Database `CNPM_LIBRARY_MANAGEMENT`** hiện tại chứa các rác dữ liệu cũ.
- **Mục đích:** Để ở lần khởi chạy tiếp theo (`dotnet run`), Entity Framework thông qua hàm `MigrateAsync` sẽ tạo lại một Database hoàn toàn sạch từ đầu. Bộ đếm Id của tất cả các bảng sẽ được khởi tạo lại về 1, đảm bảo các liên kết khóa ngoại (Foreign Key) trong Seed Data khớp nhau hoàn hảo.

## 3. Khắc phục lỗi Ảnh Sản Phẩm Không Hiển Thị Trên Web

**Thời gian:** 30/05/2026

**Vấn Đề:**
- Khi chạy ứng dụng web, các thẻ `<img>` của sản phẩm hiển thị dưới dạng ảnh vỡ (broken image) — chỉ thấy tên thay thế (alt text), không thấy hình.
- **Nguyên nhân:** Các file ảnh trong thư mục `wwwroot/images/product-images/` được đặt tên theo định dạng UUID (ví dụ: `a95a98a5-..._ToiThayhoavangtrencoxanh.jpg`), trong khi `SeedData.cs` gọi đến các tên ngắn gọn (ví dụ: `hoa-vang.jpg`). Tên file không khớp nên trình duyệt không tìm thấy ảnh.

**Các Thay Đổi Đã Thực Hiện:**

*Bước 1 — Đổi tên 8 file ảnh trong `wwwroot/images/product-images/`:*

| Tên file cũ (UUID) | Tên file mới (khớp SeedData) |
|---|---|
| `a95a98a5-..._ToiThayhoavangtrencoxanh.jpg` | `hoa-vang.jpg` |
| `d6ed1937-..._DeMenPhieuLuuKy_ToHoai.jpg` | `de-men.jpg` |
| `ed4df63d-..._DacNhanTam.jpg` | `dac-nhan-tam.jpg` |
| `9aa9c566-..._NhaGiaKim.jpg` | `nha-gia-kim.jpg` |
| `19141701-..._TuoiThoDuDoi_tap1.jpg` | `tuoi-tho.jpg` |
| `95cabfcd-..._ButBi_ThienLong_Hop20.jpg` | `but-bi-tl027.jpg` |
| `df554bab-..._HongHa_VoHS_200page.jpg` | `tap-hoc-sinh.jpg` |
| `58fc4449-..._Thuoc_ThienLong_36cm.jpg` | `thuoc-ke.jpg` |

*Bước 2 — Reset Database để SeedData chạy lại:*
- Vì database cũ đã lưu tên ảnh UUID cũ, cần xóa toàn bộ database.
- Chạy lệnh SQL qua SQLCMD để Drop database `CNPM_LIBRARY_MANAGEMENT`.
- Khi chạy lại `dotnet run`, EF Core tự tạo lại database mới sạch và SeedData sẽ điền đúng tên ảnh.

**Kết quả:**
- Ứng dụng khởi động lại, database được tạo mới, 8 sản phẩm được seed với tên ảnh chính xác.
- Ảnh sản phẩm hiển thị đầy đủ trên giao diện web.
