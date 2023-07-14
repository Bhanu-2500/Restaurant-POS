using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant_POS.Migrations
{
    /// <inheritdoc />
    public partial class AddNewPropertiesForProductModule2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalTax",
                table: "InvoiceBills");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TotalTax",
                table: "InvoiceBills",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
