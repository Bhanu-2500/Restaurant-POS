using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant_POS.Migrations
{
    /// <inheritdoc />
    public partial class addProductCategoryEntity4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImgPath",
                table: "Categories",
                newName: "ImagePath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Categories",
                newName: "ImgPath");
        }
    }
}
