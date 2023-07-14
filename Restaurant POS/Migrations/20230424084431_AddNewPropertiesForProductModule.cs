using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant_POS.Migrations
{
    /// <inheritdoc />
    public partial class AddNewPropertiesForProductModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Products",
                newName: "MentionedPrice");

            migrationBuilder.AddColumn<double>(
                name: "Discount",
                table: "Products",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TaxRate",
                table: "InvoiceBills",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalTax",
                table: "InvoiceBills",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TaxRate",
                table: "InvoiceBills");

            migrationBuilder.DropColumn(
                name: "TotalTax",
                table: "InvoiceBills");

            migrationBuilder.RenameColumn(
                name: "MentionedPrice",
                table: "Products",
                newName: "Price");
        }
    }
}
