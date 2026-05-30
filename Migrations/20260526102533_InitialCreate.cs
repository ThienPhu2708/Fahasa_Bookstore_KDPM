using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CNPM_LIBRARY_MANAGEMENT.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDNguoiDung = table.Column<string>(type: "varchar(7)", unicode: false, maxLength: 7, nullable: true, computedColumnSql: "('USER'+right('000'+CONVERT([varchar](3),[ID]),(3)))", stored: true),
                    TenNguoiDung = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Gmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "User"),
                    AvatarNguoiDung = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    GioiThieu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Accounts__3214EC2723585D18", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ChatLieu",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaChatLieu = table.Column<string>(type: "varchar(4)", unicode: false, maxLength: 4, nullable: true, computedColumnSql: "('CL'+right('00'+CONVERT([varchar](2),[ID]),(2)))", stored: true),
                    TenChatLieu = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChatLieu__3214EC27F776C123", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LoaiSanPham",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDLoaiSP = table.Column<string>(type: "varchar(3)", unicode: false, maxLength: 3, nullable: true, computedColumnSql: "('L'+right('00'+CONVERT([varchar](2),[ID]),(2)))", stored: true),
                    TenLoaiSP = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LoaiSanP__3214EC27454A2435", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LoaiVPPham",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IDLoaiVPP = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true, computedColumnSql: "('LVP'+right('00'+CONVERT([varchar](2),[ID]),(2)))", stored: true),
                    TenLoaiVPP = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__LoaiVPPh__3214EC27CC22E15A", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "MauSac",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaMauSac = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true, computedColumnSql: "('MS'+right('00'+CONVERT([nvarchar](2),[ID]),(2)))", stored: true),
                    TenMauSac = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__MauSac__3214EC273199FA6A", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "NhaSanXuat",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNSX = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: true, computedColumnSql: "('NSX'+right('000'+CONVERT([nvarchar](3),[ID]),(3)))", stored: true),
                    TenNSX = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NhaSanXu__3214EC2786F08741", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "NhaXuatBan",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaNXB = table.Column<string>(type: "varchar(6)", unicode: false, maxLength: 6, nullable: true, computedColumnSql: "('NXB'+right('000'+CONVERT([varchar](3),[ID]),(3)))", stored: true),
                    TenNXB = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__NhaXuatB__3214EC2791B538D0", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TacGia",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaTacGia = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true, computedColumnSql: "('TG'+right('000'+CONVERT([varchar](3),[ID]),(3)))", stored: true),
                    TenTacGia = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TacGia__3214EC27579F2477", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TheLoaiSach",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaTheLoai = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true, computedColumnSql: "('TLS'+right('00'+CONVERT([varchar](2),[ID]),(2)))", stored: true),
                    TenTheLoai = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__TheLoaiS__3214EC273E2B81FF", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ThuongHieu",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaThuongHieu = table.Column<string>(type: "varchar(5)", unicode: false, maxLength: 5, nullable: true, computedColumnSql: "('TT'+right('000'+CONVERT([varchar](3),[ID]),(3)))", stored: true),
                    TenThuongHieu = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ThuongHi__3214EC27D19FB015", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HoaDon",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountID = table.Column<int>(type: "int", nullable: false),
                    NgayDatHang = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    TongTien = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    TrangThai = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, defaultValue: "Chờ xử lý"),
                    HoTenNguoiNhan = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DiaChiGiaoHang = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SoDienThoai = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    GiamGia = table.Column<decimal>(type: "decimal(18,0)", nullable: false, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__HoaDon__3214EC27C5AB1E57", x => x.ID);
                    table.ForeignKey(
                        name: "FK__HoaDon__AccountI__173876EA",
                        column: x => x.AccountID,
                        principalTable: "Accounts",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "SanPham",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaSP = table.Column<string>(type: "varchar(7)", unicode: false, maxLength: 7, nullable: true, computedColumnSql: "('SP'+right('00000'+CONVERT([varchar](5),[ID]),(5)))", stored: true),
                    TenSanPham = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GiaBan = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HinhAnh = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TrongLuong = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    DanhGia = table.Column<byte>(type: "tinyint", nullable: true),
                    SoLuongDaBan = table.Column<int>(type: "int", nullable: true, defaultValue: 0),
                    NgayDang = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "(getdate())"),
                    LoaiSanPhamID = table.Column<int>(type: "int", nullable: false),
                    PhanTramGiam = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    HangMoiVe = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SanPham__3214EC274653BBA8", x => x.ID);
                    table.ForeignKey(
                        name: "FK__SanPham__LoaiSan__35BCFE0A",
                        column: x => x.LoaiSanPhamID,
                        principalTable: "LoaiSanPham",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietHoaDon",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HoaDonID = table.Column<int>(type: "int", nullable: false),
                    SanPhamID = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChiTietH__3214EC27DB44F1B5", x => x.ID);
                    table.ForeignKey(
                        name: "FK__ChiTietHo__HoaDo__4D94879B",
                        column: x => x.HoaDonID,
                        principalTable: "HoaDon",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK__ChiTietHo__SanPh__4E88ABD4",
                        column: x => x.SanPhamID,
                        principalTable: "SanPham",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietSach",
                columns: table => new
                {
                    SanPhamID = table.Column<int>(type: "int", nullable: false),
                    TheLoaiID = table.Column<int>(type: "int", nullable: false),
                    TacGiaID = table.Column<int>(type: "int", nullable: true),
                    NhaXuatBanID = table.Column<int>(type: "int", nullable: true),
                    SoTrang = table.Column<int>(type: "int", nullable: true),
                    LoaiBia = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NgayPhatHanh = table.Column<DateOnly>(type: "date", nullable: true),
                    KichThuoc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChiTietS__05180FF432D4D6A7", x => x.SanPhamID);
                    table.ForeignKey(
                        name: "FK__ChiTietSa__NhaXu__3C69FB99",
                        column: x => x.NhaXuatBanID,
                        principalTable: "NhaXuatBan",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK__ChiTietSa__SanPh__398D8EEE",
                        column: x => x.SanPhamID,
                        principalTable: "SanPham",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK__ChiTietSa__TacGi__3B75D760",
                        column: x => x.TacGiaID,
                        principalTable: "TacGia",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK__ChiTietSa__TheLo__3A81B327",
                        column: x => x.TheLoaiID,
                        principalTable: "TheLoaiSach",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "ChiTietVPP",
                columns: table => new
                {
                    SanPhamID = table.Column<int>(type: "int", nullable: false),
                    IDLoaiVPP = table.Column<int>(type: "int", nullable: false),
                    ThuongHieuID = table.Column<int>(type: "int", nullable: true),
                    NhaSanXuatID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChiTietV__05180FF4D6B8F4E4", x => x.SanPhamID);
                    table.ForeignKey(
                        name: "FK__ChiTietVP__IDLoa__403A8C7D",
                        column: x => x.IDLoaiVPP,
                        principalTable: "LoaiVPPham",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK__ChiTietVP__NhaSa__4222D4EF",
                        column: x => x.NhaSanXuatID,
                        principalTable: "NhaSanXuat",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK__ChiTietVP__SanPh__3F466844",
                        column: x => x.SanPhamID,
                        principalTable: "SanPham",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK__ChiTietVP__Thuon__412EB0B6",
                        column: x => x.ThuongHieuID,
                        principalTable: "ThuongHieu",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "DanhGias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SanPhamId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Sao = table.Column<byte>(type: "tinyint", nullable: false),
                    NoiDung = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayTao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhGias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DanhGias_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DanhGias_SanPham_SanPhamId",
                        column: x => x.SanPhamId,
                        principalTable: "SanPham",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GioHangItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    SanPhamId = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<decimal>(type: "decimal(18,0)", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GioHangItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GioHangItem_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GioHangItem_SanPham_SanPhamId",
                        column: x => x.SanPhamId,
                        principalTable: "SanPham",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "YeuThich",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    SanPhamId = table.Column<int>(type: "int", nullable: false),
                    NgayThem = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YeuThich", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YeuThich_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_YeuThich_SanPham_SanPhamId",
                        column: x => x.SanPhamId,
                        principalTable: "SanPham",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SanPhamVPP_ChatLieu",
                columns: table => new
                {
                    SanPhamVPP_ID = table.Column<int>(type: "int", nullable: false),
                    ChatLieuID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SanPhamV__C2BDD873D903A74D", x => new { x.SanPhamVPP_ID, x.ChatLieuID });
                    table.ForeignKey(
                        name: "FK__SanPhamVP__ChatL__49C3F6B7",
                        column: x => x.ChatLieuID,
                        principalTable: "ChatLieu",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK__SanPhamVP__SanPh__48CFD27E",
                        column: x => x.SanPhamVPP_ID,
                        principalTable: "ChiTietVPP",
                        principalColumn: "SanPhamID");
                });

            migrationBuilder.CreateTable(
                name: "SanPhamVPP_MauSac",
                columns: table => new
                {
                    SanPhamVPP_ID = table.Column<int>(type: "int", nullable: false),
                    MauSacID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SanPhamV__CFC60C5D9F1A39B5", x => new { x.SanPhamVPP_ID, x.MauSacID });
                    table.ForeignKey(
                        name: "FK__SanPhamVP__MauSa__45F365D3",
                        column: x => x.MauSacID,
                        principalTable: "MauSac",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK__SanPhamVP__SanPh__44FF419A",
                        column: x => x.SanPhamVPP_ID,
                        principalTable: "ChiTietVPP",
                        principalColumn: "SanPhamID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Gmail",
                table: "Accounts",
                column: "Gmail");

            migrationBuilder.CreateIndex(
                name: "UQ__Accounts__B488B103D1E1DFBE",
                table: "Accounts",
                column: "Gmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__ChatLieu__7FE6CCCAB2BD0100",
                table: "ChatLieu",
                column: "TenChatLieu",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDon_HoaDonID",
                table: "ChiTietHoaDon",
                column: "HoaDonID");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietHoaDon_SanPhamID",
                table: "ChiTietHoaDon",
                column: "SanPhamID");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietSach_NhaXuatBanID",
                table: "ChiTietSach",
                column: "NhaXuatBanID");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietSach_TacGiaID",
                table: "ChiTietSach",
                column: "TacGiaID");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietSach_TheLoaiID",
                table: "ChiTietSach",
                column: "TheLoaiID");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietVPP_IDLoaiVPP",
                table: "ChiTietVPP",
                column: "IDLoaiVPP");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietVPP_NhaSanXuatID",
                table: "ChiTietVPP",
                column: "NhaSanXuatID");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietVPP_ThuongHieuID",
                table: "ChiTietVPP",
                column: "ThuongHieuID");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_AccountId",
                table: "DanhGias",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_SanPhamId",
                table: "DanhGias",
                column: "SanPhamId");

            migrationBuilder.CreateIndex(
                name: "IX_GioHangItem_AccountId",
                table: "GioHangItem",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_GioHangItem_SanPhamId",
                table: "GioHangItem",
                column: "SanPhamId");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDon_AccountID",
                table: "HoaDon",
                column: "AccountID");

            migrationBuilder.CreateIndex(
                name: "UQ__LoaiSanP__F434DB49100F9A7C",
                table: "LoaiSanPham",
                column: "TenLoaiSP",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__MauSac__23D77400EA5ED49F",
                table: "MauSac",
                column: "TenMauSac",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__NhaXuatB__CCE3868DF879A71E",
                table: "NhaXuatBan",
                column: "TenNXB",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SanPham_LoaiSanPhamID",
                table: "SanPham",
                column: "LoaiSanPhamID");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamVPP_ChatLieu_ChatLieuID",
                table: "SanPhamVPP_ChatLieu",
                column: "ChatLieuID");

            migrationBuilder.CreateIndex(
                name: "IX_SanPhamVPP_MauSac_MauSacID",
                table: "SanPhamVPP_MauSac",
                column: "MauSacID");

            migrationBuilder.CreateIndex(
                name: "UQ__TheLoaiS__327F958F9319C08A",
                table: "TheLoaiSach",
                column: "TenTheLoai",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__ThuongHi__98D6A8341293E8CF",
                table: "ThuongHieu",
                column: "TenThuongHieu",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_YeuThich_AccountId",
                table: "YeuThich",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_YeuThich_SanPhamId",
                table: "YeuThich",
                column: "SanPhamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietHoaDon");

            migrationBuilder.DropTable(
                name: "ChiTietSach");

            migrationBuilder.DropTable(
                name: "DanhGias");

            migrationBuilder.DropTable(
                name: "GioHangItem");

            migrationBuilder.DropTable(
                name: "SanPhamVPP_ChatLieu");

            migrationBuilder.DropTable(
                name: "SanPhamVPP_MauSac");

            migrationBuilder.DropTable(
                name: "YeuThich");

            migrationBuilder.DropTable(
                name: "HoaDon");

            migrationBuilder.DropTable(
                name: "NhaXuatBan");

            migrationBuilder.DropTable(
                name: "TacGia");

            migrationBuilder.DropTable(
                name: "TheLoaiSach");

            migrationBuilder.DropTable(
                name: "ChatLieu");

            migrationBuilder.DropTable(
                name: "MauSac");

            migrationBuilder.DropTable(
                name: "ChiTietVPP");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "LoaiVPPham");

            migrationBuilder.DropTable(
                name: "NhaSanXuat");

            migrationBuilder.DropTable(
                name: "SanPham");

            migrationBuilder.DropTable(
                name: "ThuongHieu");

            migrationBuilder.DropTable(
                name: "LoaiSanPham");
        }
    }
}
