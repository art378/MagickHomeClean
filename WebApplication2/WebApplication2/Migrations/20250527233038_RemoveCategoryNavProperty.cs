using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCategoryNavProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryNavId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_CategoryNavId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CategoryNavId",
                table: "Products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryNavId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryNavId",
                table: "Products",
                column: "CategoryNavId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryNavId",
                table: "Products",
                column: "CategoryNavId",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}
