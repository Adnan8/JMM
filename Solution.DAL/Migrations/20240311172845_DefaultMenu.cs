using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solution.DAL.Migrations
{
    /// <inheritdoc />
    public partial class DefaultMenu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsMenu",
                table: "Permissions",
                newName: "IsDefault");

            migrationBuilder.AddColumn<bool>(
                name: "IsDefault",
                table: "Menus",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDefault",
                table: "Menus");

            migrationBuilder.RenameColumn(
                name: "IsDefault",
                table: "Permissions",
                newName: "IsMenu");
        }
    }
}
