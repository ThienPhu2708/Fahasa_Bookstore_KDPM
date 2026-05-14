-- Tạo bảng GioHangItem để lưu giỏ hàng theo tài khoản
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='GioHangItem' AND xtype='U')
BEGIN
    CREATE TABLE GioHangItem (
        Id          INT IDENTITY(1,1) NOT NULL,
        AccountId   INT NOT NULL,
        SanPhamId   INT NOT NULL,
        DonGia      DECIMAL(18,0) NOT NULL,
        SoLuong     INT NOT NULL,
        CONSTRAINT PK_GioHangItem PRIMARY KEY (Id),
        CONSTRAINT FK_GioHangItem_Accounts_AccountId
            FOREIGN KEY (AccountId) REFERENCES Accounts(ID) ON DELETE CASCADE,
        CONSTRAINT FK_GioHangItem_SanPham_SanPhamId
            FOREIGN KEY (SanPhamId) REFERENCES SanPham(ID) ON DELETE CASCADE
    );

    CREATE INDEX IX_GioHangItem_AccountId ON GioHangItem(AccountId);
    CREATE INDEX IX_GioHangItem_SanPhamId ON GioHangItem(SanPhamId);

    -- Ghi vào bảng lịch sử migration của EF Core
    INSERT INTO [__EFMigrationsHistory] (MigrationId, ProductVersion)
    VALUES ('20260514000001_AddGioHangTable', '8.0.0');

    PRINT 'Tao bang GioHangItem thanh cong!';
END
ELSE
BEGIN
    PRINT 'Bang GioHangItem da ton tai.';
END
