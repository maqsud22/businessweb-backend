using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessWeb.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNavigations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_StockIns_ProductId",
                table: "StockIns",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockIns_ProductPackageId",
                table: "StockIns",
                column: "ProductPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleLines_ProductPackageId",
                table: "SaleLines",
                column: "ProductPackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_SaleLines_ProductPackages_ProductPackageId",
                table: "SaleLines",
                column: "ProductPackageId",
                principalTable: "ProductPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SaleLines_Products_ProductId",
                table: "SaleLines",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockIns_ProductPackages_ProductPackageId",
                table: "StockIns",
                column: "ProductPackageId",
                principalTable: "ProductPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockIns_Products_ProductId",
                table: "StockIns",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaleLines_ProductPackages_ProductPackageId",
                table: "SaleLines");

            migrationBuilder.DropForeignKey(
                name: "FK_SaleLines_Products_ProductId",
                table: "SaleLines");

            migrationBuilder.DropForeignKey(
                name: "FK_StockIns_ProductPackages_ProductPackageId",
                table: "StockIns");

            migrationBuilder.DropForeignKey(
                name: "FK_StockIns_Products_ProductId",
                table: "StockIns");

            migrationBuilder.DropIndex(
                name: "IX_StockIns_ProductId",
                table: "StockIns");

            migrationBuilder.DropIndex(
                name: "IX_StockIns_ProductPackageId",
                table: "StockIns");

            migrationBuilder.DropIndex(
                name: "IX_SaleLines_ProductPackageId",
                table: "SaleLines");
        }
    }
}
