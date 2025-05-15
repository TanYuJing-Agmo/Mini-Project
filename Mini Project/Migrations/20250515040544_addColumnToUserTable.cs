using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mini_Project.Migrations
{
    /// <inheritdoc />
    public partial class addColumnToUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsWithdrawn",
                table: "Enrollments");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Enrollments",
                newName: "EnrolledDate");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "AspNetUsers",
                newName: "LastName");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "EnrolledDate",
                table: "Enrollments",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "AspNetUsers",
                newName: "FullName");

            migrationBuilder.AddColumn<bool>(
                name: "IsWithdrawn",
                table: "Enrollments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
