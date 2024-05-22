using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solution.DAL.Migrations
{
    /// <inheritdoc />
    public partial class addingtab : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassTimeSlot",
                table: "Class");

            migrationBuilder.AddColumn<int>(
                name: "ClassTime",
                table: "Class",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassTime",
                table: "Class");

            migrationBuilder.AddColumn<int>(
                name: "ClassTimeSlot",
                table: "Class",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
