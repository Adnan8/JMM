using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solution.DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleMenus_Menus_MenuId",
                table: "RoleMenus");

            migrationBuilder.DropForeignKey(
                name: "FK_RoleMenus_Roles_RoleId",
                table: "RoleMenus");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePermissions_Roles_RoleId",
                table: "RolePermissions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_RolePermissions_RoleId",
                table: "RolePermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleMenus",
                table: "RoleMenus");

            migrationBuilder.DropIndex(
                name: "IX_RoleMenus_RoleId",
                table: "RoleMenus");

            migrationBuilder.RenameTable(
                name: "RoleMenus",
                newName: "RoleMenu");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoleMenu_Menus_MenuId",
                table: "RoleMenu");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoleMenu",
                table: "RoleMenu");

            migrationBuilder.RenameTable(
                name: "RoleMenu",
                newName: "RoleMenus");

            migrationBuilder.RenameIndex(
                name: "IX_RoleMenu_MenuId",
                table: "RoleMenus",
                newName: "IX_RoleMenus_MenuId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoleMenus",
                table: "RoleMenus",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompId = table.Column<int>(type: "int", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_RoleId",
                table: "RolePermissions",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleMenus_RoleId",
                table: "RoleMenus",
                column: "RoleId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_RolePermissions_Roles_RoleId",
                table: "RolePermissions",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
