using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyCompanyName.MyProjectName.Migrations
{
    public partial class update_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPrice_ProjectRltModule_ProjectPrice_Module_ModuleId",
                table: "ProjectPrice_ProjectRltModule");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModuleId",
                table: "ProjectPrice_ProjectRltModule",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "ProjectPrice_Module",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPrice_ProjectRltModule_ProjectPrice_Module_ModuleId",
                table: "ProjectPrice_ProjectRltModule",
                column: "ModuleId",
                principalTable: "ProjectPrice_Module",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPrice_ProjectRltModule_ProjectPrice_Module_ModuleId",
                table: "ProjectPrice_ProjectRltModule");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "ProjectPrice_Module");

            migrationBuilder.AlterColumn<Guid>(
                name: "ModuleId",
                table: "ProjectPrice_ProjectRltModule",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPrice_ProjectRltModule_ProjectPrice_Module_ModuleId",
                table: "ProjectPrice_ProjectRltModule",
                column: "ModuleId",
                principalTable: "ProjectPrice_Module",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
