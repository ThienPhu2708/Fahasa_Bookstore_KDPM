# E-Commerce Project - CNPM

ASP.NET Core 8 MVC + SQL Server

## Yêu cầu môi trường

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server Express (hoặc bất kỳ phiên bản SQL Server nào)
- Node.js (nếu chạy Cypress test)

---

## Hướng dẫn deploy (lần đầu)

### 1. Clone repo

```bash
git clone <url-repo>
cd E-Commerce-Project-CNPM
```

### 2. Cấu hình connection string

Tạo file `appsettings.Development.json` trong thư mục gốc (file này **không được commit** vào git):

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=TÊN_MÁY_BẠN\\SQLEXPRESS;Database=CNPM_LIBRARY_MANAGEMENT;Trusted_Connection=True;MultipleActiveResultSets=True;Encrypt=False"
  }
}
```

> **Lưu ý:** Thay `TÊN_MÁY_BẠN\\SQLEXPRESS` bằng tên SQL Server instance trên máy bạn.
> Ví dụ: `DESKTOP-ABC123\\SQLEXPRESS` hoặc `localhost\\SQLEXPRESS` hoặc chỉ `(local)` nếu dùng SQL Server mặc định.

Để kiểm tra tên instance SQL Server của bạn, chạy lệnh này trong SQL Server Management Studio (SSMS):
```sql
SELECT @@SERVERNAME
```

### 3. Tạo Database và chạy Migration

Mở terminal tại thư mục gốc project rồi chạy:

```bash
dotnet ef database update
```

Lệnh này sẽ tự tạo database và **toàn bộ các bảng** (không cần chạy file `.sql` nào thêm).

> Nếu chưa có `dotnet-ef` tool:
> ```bash
> dotnet tool install --global dotnet-ef
> ```

### 4. Chạy project

```bash
dotnet run
```

Sau đó mở trình duyệt tại `https://localhost:5001` hoặc `http://localhost:5000`.

---

## Lưu ý khi phát triển

- **Không commit** file `appsettings.Development.json` — file này chứa connection string riêng của từng máy.
- Khi thêm Migration mới: `dotnet ef migrations add TenMigration`
- Khi pull code mới về có migration: nhớ chạy lại `dotnet ef database update`
