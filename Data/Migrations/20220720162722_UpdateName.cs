using Microsoft.EntityFrameworkCore.Migrations;

namespace AjaxDataTable.Data.Migrations
{
    public partial class UpdateName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "CustomerTBs");

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "CustomerTBs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "CustomerTBs");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CustomerTBs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
