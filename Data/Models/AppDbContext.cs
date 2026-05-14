using CNPM_LIBRARY_MANAGEMENT.Data.Models; 
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace CNPM_LIBRARY_MANAGEMENT.Data.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<ChatLieu> ChatLieus { get; set; }

    public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }

    public virtual DbSet<ChiTietSach> ChiTietSaches { get; set; }

    public virtual DbSet<ChiTietVpp> ChiTietVpps { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<LoaiSanPham> LoaiSanPhams { get; set; }

    public virtual DbSet<LoaiVppham> LoaiVpphams { get; set; }

    public virtual DbSet<MauSac> MauSacs { get; set; }

    public virtual DbSet<NhaSanXuat> NhaSanXuats { get; set; }

    public virtual DbSet<NhaXuatBan> NhaXuatBans { get; set; }

    public virtual DbSet<SanPham> SanPhams { get; set; }

    public virtual DbSet<TacGium> TacGia { get; set; }

    public virtual DbSet<TheLoaiSach> TheLoaiSaches { get; set; }

    public virtual DbSet<ThuongHieu> ThuongHieus { get; set; }

    public DbSet<DanhGia> DanhGias { get; set; }
    public DbSet<YeuThich> YeuThichs { get; set; }
    public DbSet<GioHangItem> GioHangItems { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Accounts__3214EC2723585D18");

            entity.HasIndex(e => e.Gmail, "IX_Accounts_Gmail");

            entity.HasIndex(e => e.Gmail, "UQ__Accounts__B488B103D1E1DFBE").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AvatarNguoiDung).HasMaxLength(255);
            entity.Property(e => e.Gmail).HasMaxLength(100);
            entity.Property(e => e.IdnguoiDung)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasComputedColumnSql("('USER'+right('000'+CONVERT([varchar](3),[ID]),(3)))", true)
                .HasColumnName("IDNguoiDung");
            entity.Property(e => e.NgayTao)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasDefaultValue("User");
            entity.Property(e => e.TenNguoiDung).HasMaxLength(100);
        });

        modelBuilder.Entity<ChatLieu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ChatLieu__3214EC27F776C123");

            entity.ToTable("ChatLieu");

            entity.HasIndex(e => e.TenChatLieu, "UQ__ChatLieu__7FE6CCCAB2BD0100").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaChatLieu)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasComputedColumnSql("('CL'+right('00'+CONVERT([varchar](2),[ID]),(2)))", true);
            entity.Property(e => e.TenChatLieu).HasMaxLength(50);
        });

        modelBuilder.Entity<ChiTietHoaDon>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ChiTietH__3214EC27DB44F1B5");

            entity.ToTable("ChiTietHoaDon");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.HoaDonId).HasColumnName("HoaDonID");
            entity.Property(e => e.SanPhamId).HasColumnName("SanPhamID");

            entity.HasOne(d => d.HoaDon).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.HoaDonId)
                .HasConstraintName("FK__ChiTietHo__HoaDo__4D94879B");

            entity.HasOne(d => d.SanPham).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.SanPhamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietHo__SanPh__4E88ABD4");
        });

        modelBuilder.Entity<ChiTietSach>(entity =>
        {
            entity.HasKey(e => e.SanPhamId).HasName("PK__ChiTietS__05180FF432D4D6A7");

            entity.ToTable("ChiTietSach");

            entity.Property(e => e.SanPhamId)
                .ValueGeneratedNever()
                .HasColumnName("SanPhamID");
            entity.Property(e => e.KichThuoc).HasMaxLength(50);
            entity.Property(e => e.LoaiBia).HasMaxLength(50);
            entity.Property(e => e.NhaXuatBanId).HasColumnName("NhaXuatBanID");
            entity.Property(e => e.TacGiaId).HasColumnName("TacGiaID");
            entity.Property(e => e.TheLoaiId).HasColumnName("TheLoaiID");

            entity.HasOne(d => d.NhaXuatBan).WithMany(p => p.ChiTietSaches)
                .HasForeignKey(d => d.NhaXuatBanId)
                .HasConstraintName("FK__ChiTietSa__NhaXu__3C69FB99");

            entity.HasOne(d => d.SanPham).WithOne(p => p.ChiTietSach)
                .HasForeignKey<ChiTietSach>(d => d.SanPhamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietSa__SanPh__398D8EEE");

            entity.HasOne(d => d.TacGia).WithMany(p => p.ChiTietSaches)
                .HasForeignKey(d => d.TacGiaId)
                .HasConstraintName("FK__ChiTietSa__TacGi__3B75D760");

            entity.HasOne(d => d.TheLoai).WithMany(p => p.ChiTietSaches)
                .HasForeignKey(d => d.TheLoaiId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietSa__TheLo__3A81B327");
        });

        modelBuilder.Entity<ChiTietVpp>(entity =>
        {
            entity.HasKey(e => e.SanPhamId).HasName("PK__ChiTietV__05180FF4D6B8F4E4");

            entity.ToTable("ChiTietVPP");

            entity.Property(e => e.SanPhamId)
                .ValueGeneratedNever()
                .HasColumnName("SanPhamID");
            entity.Property(e => e.IdloaiVpp).HasColumnName("IDLoaiVPP");
            entity.Property(e => e.NhaSanXuatId).HasColumnName("NhaSanXuatID");
            entity.Property(e => e.ThuongHieuId).HasColumnName("ThuongHieuID");

            entity.HasOne(d => d.IdloaiVppNavigation).WithMany(p => p.ChiTietVpps)
                .HasForeignKey(d => d.IdloaiVpp)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietVP__IDLoa__403A8C7D");

            entity.HasOne(d => d.NhaSanXuat).WithMany(p => p.ChiTietVpps)
                .HasForeignKey(d => d.NhaSanXuatId)
                .HasConstraintName("FK__ChiTietVP__NhaSa__4222D4EF");

            entity.HasOne(d => d.SanPham).WithOne(p => p.ChiTietVpp)
                .HasForeignKey<ChiTietVpp>(d => d.SanPhamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietVP__SanPh__3F466844");

            entity.HasOne(d => d.ThuongHieu).WithMany(p => p.ChiTietVpps)
                .HasForeignKey(d => d.ThuongHieuId)
                .HasConstraintName("FK__ChiTietVP__Thuon__412EB0B6");

            entity.HasMany(d => d.ChatLieus).WithMany(p => p.SanPhamVpps)
                .UsingEntity<Dictionary<string, object>>(
                    "SanPhamVppChatLieu",
                    r => r.HasOne<ChatLieu>().WithMany()
                        .HasForeignKey("ChatLieuId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__SanPhamVP__ChatL__49C3F6B7"),
                    l => l.HasOne<ChiTietVpp>().WithMany()
                        .HasForeignKey("SanPhamVppId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__SanPhamVP__SanPh__48CFD27E"),
                    j =>
                    {
                        j.HasKey("SanPhamVppId", "ChatLieuId").HasName("PK__SanPhamV__C2BDD873D903A74D");
                        j.ToTable("SanPhamVPP_ChatLieu");
                        j.IndexerProperty<int>("SanPhamVppId").HasColumnName("SanPhamVPP_ID");
                        j.IndexerProperty<int>("ChatLieuId").HasColumnName("ChatLieuID");
                    });

            entity.HasMany(d => d.MauSacs).WithMany(p => p.SanPhamVpps)
                .UsingEntity<Dictionary<string, object>>(
                    "SanPhamVppMauSac",
                    r => r.HasOne<MauSac>().WithMany()
                        .HasForeignKey("MauSacId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__SanPhamVP__MauSa__45F365D3"),
                    l => l.HasOne<ChiTietVpp>().WithMany()
                        .HasForeignKey("SanPhamVppId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK__SanPhamVP__SanPh__44FF419A"),
                    j =>
                    {
                        j.HasKey("SanPhamVppId", "MauSacId").HasName("PK__SanPhamV__CFC60C5D9F1A39B5");
                        j.ToTable("SanPhamVPP_MauSac");
                        j.IndexerProperty<int>("SanPhamVppId").HasColumnName("SanPhamVPP_ID");
                        j.IndexerProperty<int>("MauSacId").HasColumnName("MauSacID");
                    });
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__HoaDon__3214EC27C5AB1E57");

            entity.ToTable("HoaDon");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.DiaChiGiaoHang).HasMaxLength(500);
            entity.Property(e => e.HoTenNguoiNhan).HasMaxLength(100);
            entity.Property(e => e.NgayDatHang)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SoDienThoai).HasMaxLength(20);
            entity.Property(e => e.TongTien).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.TrangThai)
                .HasMaxLength(100)
                .HasDefaultValue("Chờ xử lý");


            entity.Property(e => e.GiamGia)
                .HasColumnType("decimal(18, 0)") 
                .HasDefaultValueSql("((0))");    
                                                 

            entity.HasOne(d => d.Account).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__HoaDon__AccountI__173876EA");
        });

        modelBuilder.Entity<LoaiSanPham>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LoaiSanP__3214EC27454A2435");

            entity.ToTable("LoaiSanPham");

            entity.HasIndex(e => e.TenLoaiSp, "UQ__LoaiSanP__F434DB49100F9A7C").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdloaiSp)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasComputedColumnSql("('L'+right('00'+CONVERT([varchar](2),[ID]),(2)))", true)
                .HasColumnName("IDLoaiSP");
            entity.Property(e => e.TenLoaiSp)
                .HasMaxLength(100)
                .HasColumnName("TenLoaiSP");
        });

        modelBuilder.Entity<LoaiVppham>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LoaiVPPh__3214EC27CC22E15A");

            entity.ToTable("LoaiVPPham");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdloaiVpp)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasComputedColumnSql("('LVP'+right('00'+CONVERT([varchar](2),[ID]),(2)))", true)
                .HasColumnName("IDLoaiVPP");
            entity.Property(e => e.TenLoaiVpp)
                .HasMaxLength(100)
                .HasColumnName("TenLoaiVPP");
        });

        modelBuilder.Entity<MauSac>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MauSac__3214EC273199FA6A");

            entity.ToTable("MauSac");

            entity.HasIndex(e => e.TenMauSac, "UQ__MauSac__23D77400EA5ED49F").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaMauSac)
                .HasMaxLength(4)
                .HasComputedColumnSql("('MS'+right('00'+CONVERT([nvarchar](2),[ID]),(2)))", true);
            entity.Property(e => e.TenMauSac).HasMaxLength(20);
        });

        modelBuilder.Entity<NhaSanXuat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NhaSanXu__3214EC2786F08741");

            entity.ToTable("NhaSanXuat");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaNsx)
                .HasMaxLength(6)
                .HasComputedColumnSql("('NSX'+right('000'+CONVERT([nvarchar](3),[ID]),(3)))", true)
                .HasColumnName("MaNSX");
            entity.Property(e => e.TenNsx)
                .HasMaxLength(100)
                .HasColumnName("TenNSX");
        });

        modelBuilder.Entity<NhaXuatBan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__NhaXuatB__3214EC2791B538D0");

            entity.ToTable("NhaXuatBan");

            entity.HasIndex(e => e.TenNxb, "UQ__NhaXuatB__CCE3868DF879A71E").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaNxb)
                .HasMaxLength(6)
                .IsUnicode(false)
                .HasComputedColumnSql("('NXB'+right('000'+CONVERT([varchar](3),[ID]),(3)))", true)
                .HasColumnName("MaNXB");
            entity.Property(e => e.TenNxb)
                .HasMaxLength(100)
                .HasColumnName("TenNXB");
        });

        modelBuilder.Entity<SanPham>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SanPham__3214EC274653BBA8");

            entity.ToTable("SanPham");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.GiaBan).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.HinhAnh).HasMaxLength(255);
            entity.Property(e => e.LoaiSanPhamId).HasColumnName("LoaiSanPhamID");
            entity.Property(e => e.MaSp)
                .HasMaxLength(7)
                .IsUnicode(false)
                .HasComputedColumnSql("('SP'+right('00000'+CONVERT([varchar](5),[ID]),(5)))", true)
                .HasColumnName("MaSP");
            entity.Property(e => e.NgayDang).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.SoLuongDaBan).HasDefaultValue(0);
            entity.Property(e => e.TenSanPham).HasMaxLength(200);
            entity.Property(e => e.TrongLuong).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.PhanTramGiam).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.LoaiSanPham).WithMany(p => p.SanPhams)
                .HasForeignKey(d => d.LoaiSanPhamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SanPham__LoaiSan__35BCFE0A");
        });

        modelBuilder.Entity<TacGium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TacGia__3214EC27579F2477");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaTacGia)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasComputedColumnSql("('TG'+right('000'+CONVERT([varchar](3),[ID]),(3)))", true);
            entity.Property(e => e.TenTacGia).HasMaxLength(100);
        });

        modelBuilder.Entity<TheLoaiSach>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TheLoaiS__3214EC273E2B81FF");

            entity.ToTable("TheLoaiSach");

            entity.HasIndex(e => e.TenTheLoai, "UQ__TheLoaiS__327F958F9319C08A").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaTheLoai)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasComputedColumnSql("('TLS'+right('00'+CONVERT([varchar](2),[ID]),(2)))", true);
            entity.Property(e => e.TenTheLoai).HasMaxLength(100);
        });

        modelBuilder.Entity<ThuongHieu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ThuongHi__3214EC27D19FB015");

            entity.ToTable("ThuongHieu");

            entity.HasIndex(e => e.TenThuongHieu, "UQ__ThuongHi__98D6A8341293E8CF").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaThuongHieu)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasComputedColumnSql("('TT'+right('000'+CONVERT([varchar](3),[ID]),(3)))", true);
            entity.Property(e => e.TenThuongHieu).HasMaxLength(100);
        });

        modelBuilder.Entity<GioHangItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.ToTable("GioHangItem");
            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Account)
                .WithMany(p => p.GioHangItems)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.SanPham)
                .WithMany()
                .HasForeignKey(d => d.SanPhamId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
