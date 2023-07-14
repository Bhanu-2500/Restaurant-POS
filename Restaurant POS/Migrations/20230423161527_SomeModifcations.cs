using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant_POS.Migrations
{
    /// <inheritdoc />
    public partial class SomeModifcations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkHistory_Users_UserId",
                table: "WorkHistory");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "WorkHistory",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "WorkHistory",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkHistory_Users_UserId",
                table: "WorkHistory",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkHistory_Users_UserId",
                table: "WorkHistory");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "WorkHistory",
                newName: "id");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "WorkHistory",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkHistory_Users_UserId",
                table: "WorkHistory",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
