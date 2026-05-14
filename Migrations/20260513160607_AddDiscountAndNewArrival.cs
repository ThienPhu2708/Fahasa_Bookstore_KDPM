using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CNPM_LIBRARY_MANAGEMENT.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscountAndNewArrival : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PhanTramGiam",
                table: "SanPham",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HangMoiVe",
                table: "SanPham",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhanTramGiam",
                table: "SanPham");

            migrationBuilder.DropColumn(
                name: "HangMoiVe",
                table: "SanPham");
        }
    }
}
