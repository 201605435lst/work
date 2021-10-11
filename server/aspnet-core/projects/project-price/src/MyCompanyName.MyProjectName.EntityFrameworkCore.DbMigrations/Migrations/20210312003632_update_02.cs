using Microsoft.EntityFrameworkCore.Migrations;

namespace MyCompanyName.MyProjectName.Migrations
{
    public partial class update_02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ProjectPrice_Project",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "ProjectPrice_Project");
        }
    }
}
