### 4.2 Kế Hoạch Kiểm Thử Chi Tiết

#### 4.2.1 Đăng ký tài khoản
Mô tả chức năng:  
Chức năng đăng ký cho phép người dùng mới tạo một tài khoản để truy cập vào hệ thống website Fahasa Bookstore. Quá trình này yêu cầu người dùng cung cấp các thông tin cơ bản như Tên, Email, Mật khẩu và thực hiện mã hóa bảo mật trước khi lưu vào cơ sở dữ liệu.

Các trường hợp kiểm thử (Test Cases):
*   TC_REG_01: Đăng ký thành công với thông tin hợp lệ.
*   TC_REG_02: Báo lỗi khi để trống các trường bắt buộc.
*   TC_REG_03: Báo lỗi khi nhập email đã tồn tại trong hệ thống.
*   TC_REG_04: Báo lỗi khi nhập email sai định dạng (ví dụ: thiếu @).
*   TC_REG_05: Báo lỗi khi mật khẩu không đủ độ dài hoặc độ phức tạp.
*   TC_REG_06: Báo lỗi khi mật khẩu và xác nhận mật khẩu không khớp nhau.
*   TC_REG_07: Kiểm tra việc lưu trữ an toàn (Mã hóa Hash password) trong cơ sở dữ liệu sau khi đăng ký thành công.

Công cụ sử dụng:  
Cypress (Kiểm thử UI) + NUnit (Kiểm thử validate logic).

---

#### 4.2.2 Đăng nhập tài khoản
Mô tả chức năng:  
Chức năng đăng nhập giúp xác thực danh tính của người dùng đã có tài khoản trên hệ thống. Sau khi xác thực thành công, hệ thống sẽ cấp quyền truy cập, phân quyền (User/Admin) và thiết lập phiên đăng nhập (Session) để người dùng có thể thao tác mua hàng.

Các trường hợp kiểm thử (Test Cases):
*   TC_LOGIN_01: Đăng nhập thành công với Email và Mật khẩu đúng.
*   TC_LOGIN_02: Báo lỗi thông báo khi nhập sai Email hoặc Mật khẩu.
*   TC_LOGIN_03: Báo lỗi khi để trống Email hoặc Mật khẩu.
*   TC_LOGIN_04: Kiểm tra quyền truy cập bị từ chối đối với tài khoản đã bị khóa hoặc vô hiệu hóa.
*   TC_LOGIN_05: Kiểm tra hệ thống tạo và lưu trữ Session thành công thông qua API.

Công cụ sử dụng:  
Cypress (Kiểm thử UI) + Postman (Kiểm thử API /Account/ApiLogin).

---

#### 4.2.3 Tìm kiếm sản phẩm
Mô tả chức năng:  
Tìm kiếm là tính năng cốt lõi giúp người dùng dễ dàng tra cứu sách hoặc văn phòng phẩm dựa trên từ khóa, tên sản phẩm hoặc danh mục. Chức năng này sẽ quét trong cơ sở dữ liệu và trả về danh sách các sản phẩm phù hợp nhất.

Các trường hợp kiểm thử (Test Cases):
*   TC_SEARCH_01: Tìm kiếm thành công với từ khóa hợp lệ, trả về đúng danh sách sản phẩm.
*   TC_SEARCH_02: Hiển thị thông báo "Không tìm thấy" khi nhập từ khóa không tồn tại.
*   TC_SEARCH_03: Tìm kiếm với từ khóa chứa ký tự đặc biệt hoặc chỉ chứa toàn khoảng trắng.
*   TC_SEARCH_04: Kiểm tra việc kết hợp thanh tìm kiếm với bộ lọc (theo danh mục, giá tiền).

Công cụ sử dụng:  
Cypress (Kiểm thử UI).

---

#### 4.2.4 Giỏ hàng
Mô tả chức năng:  
Giỏ hàng cho phép người dùng lưu trữ tạm thời các sản phẩm muốn mua trước khi tiến hành đặt hàng. Hệ thống tự động tính toán tổng tiền, cho phép thay đổi số lượng, xóa sản phẩm và đồng bộ giỏ hàng với Database nếu người dùng đã đăng nhập.

Các trường hợp kiểm thử (Test Cases):
*   TC_CART_01: Thêm một sản phẩm mới vào giỏ hàng thành công.
*   TC_CART_02: Thêm trùng một sản phẩm đã có trong giỏ (hệ thống phải cộng dồn số lượng thay vì thêm dòng mới).
*   TC_CART_03: Cập nhật số lượng sản phẩm trong giỏ hàng (tăng/giảm) và kiểm tra tổng tiền thay đổi chính xác.
*   TC_CART_04: Xóa hoàn toàn một sản phẩm ra khỏi giỏ hàng.
*   TC_CART_05: Cập nhật số lượng sản phẩm về 0 (hệ thống tự động xóa sản phẩm khỏi giỏ).
*   TC_CART_06: Kiểm tra đồng bộ giỏ hàng từ Session vào Database ngay sau khi người dùng đăng nhập.

Công cụ sử dụng:  
Cypress (Kiểm thử UI) + Postman (Kiểm thử API /Cart/ApiAddToCart).

---

#### 4.2.5 Đặt hàng và thanh toán
Mô tả chức năng:  
Đây là bước cuối cùng trong luồng mua sắm, nơi người dùng điền thông tin giao hàng và xác nhận chốt đơn. Hệ thống sẽ ghi nhận thông tin đơn hàng vào cơ sở dữ liệu, trừ đi số lượng tồn kho tương ứng và hiển thị trang thông báo đặt hàng thành công.

Các trường hợp kiểm thử (Test Cases):
*   TC_ORDER_01: Đặt hàng thành công với đầy đủ thông tin giao hàng hợp lệ.
*   TC_ORDER_02: Báo lỗi khi bỏ trống các trường thông tin nhận hàng (Tên, SĐT, Địa chỉ).
*   TC_ORDER_03: Kiểm tra tổng tiền trên hóa đơn (Đơn hàng) phải khớp chính xác với tổng tiền từ Giỏ hàng.
*   TC_ORDER_04: Cố tình đặt hàng thất bại khi giỏ hàng đang trống.
*   TC_ORDER_05: Kiểm tra số lượng tồn kho của sản phẩm trong hệ thống có bị trừ đúng số lượng đã mua hay không.
*   TC_ORDER_06: Báo lỗi định dạng khi nhập Số điện thoại giao hàng không hợp lệ (ví dụ chứa chữ cái hoặc quá ngắn).
*   TC_ORDER_07: Kiểm tra trạng thái đơn hàng mặc định (Ví dụ: "Chờ xác nhận") ngay sau khi đặt hàng thành công.

Công cụ sử dụng:  
Cypress (Kiểm thử UI).

---

### 4.3 Môi trường kiểm thử tự động Cypress

#### 4.3.1 Cấu hình phần cứng và phần mềm
Yêu cầu Phần cứng:
- CPU: Intel Core i5 hoặc tương đương trở lên.
- RAM: Tối thiểu 8GB (Khuyến nghị 16GB để chạy mượt mà hệ thống, IDE, Database và Test Runner).
- Ổ cứng: Trống tối thiểu 10GB (Ưu tiên dùng SSD để tăng tốc độ khởi động và đọc ghi dữ liệu).

Yêu cầu Phần mềm:
- Hệ điều hành: Windows 10/11, macOS, hoặc phân phối Linux tương thích.
- Trình duyệt kiểm thử: Google Chrome, Microsoft Edge, hoặc Firefox (Cypress sẽ tự động nhận diện và chạy trực tiếp trên các trình duyệt này).
- Môi trường chạy nền: Node.js (Phiên bản LTS mới nhất, khuyên dùng v18.x trở lên).
- Trình soạn thảo mã (IDE): Visual Studio Code hoặc Visual Studio 2022.

#### 4.3.2 Môi trường chạy ứng dụng (Localhost)
- Nền tảng Backend/Frontend: Ứng dụng chạy trên nền tảng .NET 8.0 (hoặc bản tương thích) với mô hình ASP.NET Core MVC.
- Cơ sở dữ liệu: Microsoft SQL Server (sử dụng phiên bản Developer, Express hoặc LocalDB).
- Địa chỉ URL: Ứng dụng được khởi chạy tại môi trường phát triển (Development) qua cổng local, ví dụ: http://localhost:xxxx (Cypress sẽ cấu hình baseUrl trỏ trực tiếp vào URL này).
- Môi trường dữ liệu test: Sử dụng Seed Data (Dữ liệu mẫu giả lập) để tự động khởi tạo các tài khoản, sản phẩm, và danh mục mặc định mỗi lần tạo mới CSDL. Đảm bảo trạng thái Database luôn đồng nhất để tránh lỗi phụ thuộc dữ liệu khi kiểm thử.

---

### 4.4 Cài đặt và cấu hình công cụ

#### 4.4.1 Cài đặt và cấu hình Nunit
NUnit được áp dụng để kiểm thử các đơn vị logic (Unit Test) ở tầng Backend như các hàm Validate, tính toán giá trị đơn hàng, phân quyền.
- Cài đặt: 
  - Khởi tạo project Test mới thông qua lệnh .NET CLI: dotnet new nunit -n Fahasa.Tests
  - Thêm liên kết (Reference) project Test với project chính để có thể gọi các lớp logic: dotnet add reference ../Fahasa_Bookstore_KDPM/Fahasa_Bookstore_KDPM.csproj
- Cấu hình:
  - Cài đặt bổ sung các thư viện (NuGet packages) hỗ trợ kiểm thử: Moq (để giả lập các đối tượng Interface/Service), FluentAssertions (để viết câu lệnh Assert rõ ràng hơn).
  - Cấu trúc thư mục Test tuân theo chuẩn cấu trúc của project chính (Controllers, Services). Các hàm Test được đánh dấu bằng Attribute [TestFixture] và [Test].
  - Thực thi kiểm thử: Chạy test trực tiếp thông qua Test Explorer trong Visual Studio hoặc bằng lệnh dotnet test trên Terminal.

#### 4.4.2 Cài đặt và cấu hình Cypress
Cypress được sử dụng để kiểm thử giao diện người dùng (End-to-End Test), giúp mô phỏng lại các hành vi tương tác như click, nhập liệu, chuyển trang y hệt người dùng thật.
- Cài đặt:
  - Mở Terminal tại thư mục gốc của project Automation Test (hoặc thư mục riêng biệt).
  - Khởi tạo file cấu hình package.json: npm init -y
  - Cài đặt Cypress thông qua npm (lưu ở dạng dependency dành cho môi trường phát triển): npm install cypress --save-dev
- Cấu hình:
  - Khởi động Cypress lần đầu để phần mềm tự sinh ra cấu trúc thư mục chuẩn: npx cypress open
  - Trong file cấu hình cypress.config.js, thiết lập tham số baseUrl trỏ về địa chỉ localhost của ứng dụng web .NET.
  - Viết kịch bản kiểm thử (Test Scripts): Các kịch bản được viết bằng JavaScript, lưu trong thư mục cypress/e2e/ với định dạng đuôi .cy.js.
  - Tái sử dụng lệnh: Có thể định nghĩa thêm các lệnh tùy chỉnh (Custom Commands) trong cypress/support/commands.js (ví dụ lệnh cy.login(email, password) để tái sử dụng nhanh ở nhiều file test case khác nhau).
