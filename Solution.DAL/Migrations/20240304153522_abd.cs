using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solution.DAL.Migrations
{
    /// <inheritdoc />
    public partial class abd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleMenu_Menus_MenuId",
                table: "RoleMenu");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleMenu_Roles_RoleId",
                table: "RoleMenu");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleMenu",
                table: "RoleMenu");

            migrationBuilder.RenameTable(
                name: "RoleMenu",
                newName: "RoleMenus");

            migrationBuilder.RenameIndex(
                name: "IX_RoleMenu_RoleId",
                table: "RoleMenus",
                newName: "IX_RoleMenus_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleMenu_MenuId",
                table: "RoleMenus",
                newName: "IX_RoleMenus_MenuId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleMenus",
                table: "RoleMenus",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleMenus_Menus_MenuId",
                table: "RoleMenus",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleMenus_Roles_RoleId",
                table: "RoleMenus",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleMenus_Menus_MenuId",
                table: "RoleMenus");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleMenus_Roles_RoleId",
                table: "RoleMenus");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleMenus",
                table: "RoleMenus");

            migrationBuilder.RenameTable(
                name: "RoleMenus",
                newName: "RoleMenu");

            migrationBuilder.RenameIndex(
                name: "IX_RoleMenus_RoleId",
                table: "RoleMenu",
                newName: "IX_RoleMenu_RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RoleMenus_MenuId",
                table: "RoleMenu",
                newName: "IX_RoleMenu_MenuId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleMenu",
                table: "RoleMenu",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoleMenu_Menus_MenuId",
                table: "RoleMenu",
                column: "MenuId",
                principalTable: "Menus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoleMenu_Roles_RoleId",
                table: "RoleMenu",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
