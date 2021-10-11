using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyCompanyName.MyProjectName.Migrations
{
    public partial class update_rlt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentId",
                table: "ProjectPrice_ProjectRltModule",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPrice_ProjectRltModule_ParentId",
                table: "ProjectPrice_ProjectRltModule",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPrice_ProjectRltModule_ProjectPrice_ProjectRltModule~",
                table: "ProjectPrice_ProjectRltModule",
                column: "ParentId",
                principalTable: "ProjectPrice_ProjectRltModule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPrice_ProjectRltModule_ProjectPrice_ProjectRltModule~",
                table: "ProjectPrice_ProjectRltModule");

            migrationBuilder.DropIndex(
                name: "IX_ProjectPrice_ProjectRltModule_ParentId",
                table: "ProjectPrice_ProjectRltModule");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "ProjectPrice_ProjectRltModule");
        }
    }
}
