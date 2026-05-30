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
