using Microsoft.EntityFrameworkCore.Migrations;

namespace MyCompanyName.MyProjectName.Migrations
{
    public partial class add_entities_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPrice_MyCompanyName.MyProjectName.Entities.Module_Pr~",
                table: "ProjectPrice_MyCompanyName.MyProjectName.Entities.Module");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPrice_MyCompanyName.MyProjectName.Entities.Project_P~",
                table: "ProjectPrice_MyCompanyName.MyProjectName.Entities.Project");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPrice_MyCompanyName.MyProjectName.Entities.ProjectRl~",
                table: "ProjectPrice_MyCompanyName.MyProjectName.Entities.ProjectRltModule");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPrice_MyCompanyName.MyProjectName.Entities.ProjectR~1",
                table: "ProjectPrice_MyCompanyName.MyProjectName.Entities.ProjectRltModule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectPrice_MyCompanyName.MyProjectName.Entities.ProjectRl~",
                table: "ProjectPrice_MyCompanyName.MyProjectName.Entities.ProjectRltModule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectPrice_MyCompanyName.MyProjectName.Entities.Project",
                table: "ProjectPrice_MyCompanyName.MyProjectName.Entities.Project");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectPrice_MyCompanyName.MyProjectName.Entities.Module",
                table: "ProjectPrice_MyCompanyName.MyProjectName.Entities.Module");

            migrationBuilder.RenameTable(
                name: "ProjectPrice_MyCompanyName.MyProjectName.Entities.ProjectRltModule",
                newName: "ProjectPrice_ProjectRltModule");

            migrationBuilder.RenameTable(
                name: "ProjectPrice_MyCompanyName.MyProjectName.Entities.Project",
                newName: "ProjectPrice_Project");

            migrationBuilder.RenameTable(
                name: "ProjectPrice_MyCompanyName.MyProjectName.Entities.Module",
                newName: "ProjectPrice_Module");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectPrice_MyCompanyName.MyProjectName.Entities.ProjectR~1",
                table: "ProjectPrice_ProjectRltModule",
                newName: "IX_ProjectPrice_ProjectRltModule_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectPrice_MyCompanyName.MyProjectName.Entities.ProjectRl~",
                table: "ProjectPrice_ProjectRltModule",
                newName: "IX_ProjectPrice_ProjectRltModule_ModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectPrice_MyCompanyName.MyProjectName.Entities.Project_P~",
                table: "ProjectPrice_Project",
                newName: "IX_ProjectPrice_Project_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectPrice_MyCompanyName.MyProjectName.Entities.Module_Pa~",
                table: "ProjectPrice_Module",
                newName: "IX_ProjectPrice_Module_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectPrice_ProjectRltModule",
                table: "ProjectPrice_ProjectRltModule",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectPrice_Project",
                table: "ProjectPrice_Project",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectPrice_Module",
                table: "ProjectPrice_Module",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPrice_Module_ProjectPrice_Module_ParentId",
                table: "ProjectPrice_Module",
                column: "ParentId",
                principalTable: "ProjectPrice_Module",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPrice_Project_ProjectPrice_Project_ParentId",
                table: "ProjectPrice_Project",
                column: "ParentId",
                principalTable: "ProjectPrice_Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPrice_ProjectRltModule_ProjectPrice_Module_ModuleId",
                table: "ProjectPrice_ProjectRltModule",
                column: "ModuleId",
                principalTable: "ProjectPrice_Module",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPrice_ProjectRltModule_ProjectPrice_Project_ProjectId",
                table: "ProjectPrice_ProjectRltModule",
                column: "ProjectId",
                principalTable: "ProjectPrice_Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPrice_Module_ProjectPrice_Module_ParentId",
                table: "ProjectPrice_Module");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPrice_Project_ProjectPrice_Project_ParentId",
                table: "ProjectPrice_Project");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPrice_ProjectRltModule_ProjectPrice_Module_ModuleId",
                table: "ProjectPrice_ProjectRltModule");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPrice_ProjectRltModule_ProjectPrice_Project_ProjectId",
                table: "ProjectPrice_ProjectRltModule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectPrice_ProjectRltModule",
                table: "ProjectPrice_ProjectRltModule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectPrice_Project",
                table: "ProjectPrice_Project");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectPrice_Module",
                table: "ProjectPrice_Module");

            migrationBuilder.RenameTable(
                name: "ProjectPrice_ProjectRltModule",
                newName: "ProjectPrice_MyCompanyName.MyProjectName.Entities.ProjectRltModule");

            migrationBuilder.RenameTable(
                name: "ProjectPrice_Project",
                newName: "ProjectPrice_MyCompanyName.MyProjectName.Entities.Project");

            migrationBuilder.RenameTable(
                name: "ProjectPrice_Module",
                newName: "ProjectPrice_MyCompanyName.MyProjectName.Entities.Module");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectPrice_ProjectRltModule_ProjectId",
                table: "ProjectPrice_MyCompanyName.MyProjectName.Entities.ProjectRltModule",
                newName: "IX_ProjectPrice_MyCompanyName.MyProjectName.Entities.ProjectR~1");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectPrice_ProjectRltModule_ModuleId",
                table: "ProjectPrice_MyCompanyName.MyProjectName.Entities.ProjectRltModule",
                newName: "IX_ProjectPrice_MyCompanyName.MyProjectName.Entities.ProjectRl~");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectPrice_Project_ParentId",
                table: "ProjectPrice_MyCompanyName.MyProjectName.Entities.Project",
                newName: "IX_ProjectPrice_MyCompanyName.MyProjectName.Entities.Project_P~");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectPrice_Module_ParentId",
                table: "ProjectPrice_MyCompanyName.MyProjectName.Entities.Module",
                newName: "IX_ProjectPrice_MyCompanyName.MyProjectName.Entities.Module_Pa~");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectPrice_MyCompanyName.MyProjectName.Entities.ProjectRl~",
                table: "ProjectPrice_MyCompanyName.MyProjectName.Entities.ProjectRltModule",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectPrice_MyCompanyName.MyProjectName.Entities.Project",
                table: "ProjectPrice_MyCompanyName.MyProjectName.Entities.Project",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectPrice_MyCompanyName.MyProjectName.Entities.Module",
                table: "ProjectPrice_MyCompanyName.MyProjectName.Entities.Module",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPrice_MyCompanyName.MyProjectName.Entities.Module_Pr~",
                table: "ProjectPrice_MyCompanyName.MyProjectName.Entities.Module",
                column: "ParentId",
                principalTable: "ProjectPrice_MyCompanyName.MyProjectName.Entities.Module",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPrice_MyCompanyName.MyProjectName.Entities.Project_P~",
                table: "ProjectPrice_MyCompanyName.MyProjectName.Entities.Project",
                column: "ParentId",
                principalTable: "ProjectPrice_MyCompanyName.MyProjectName.Entities.Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPrice_MyCompanyName.MyProjectName.Entities.ProjectRl~",
                table: "ProjectPrice_MyCompanyName.MyProjectName.Entities.ProjectRltModule",
                column: "ModuleId",
                principalTable: "ProjectPrice_MyCompanyName.MyProjectName.Entities.Module",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPrice_MyCompanyName.MyProjectName.Entities.ProjectR~1",
                table: "ProjectPrice_MyCompanyName.MyProjectName.Entities.ProjectRltModule",
                column: "ProjectId",
                principalTable: "ProjectPrice_MyCompanyName.MyProjectName.Entities.Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
