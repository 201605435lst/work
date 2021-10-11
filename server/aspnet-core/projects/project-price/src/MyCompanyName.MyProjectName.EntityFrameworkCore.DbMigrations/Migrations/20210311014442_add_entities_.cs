using Microsoft.EntityFrameworkCore.Migrations;

namespace MyCompanyName.MyProjectName.Migrations
{
    public partial class add_entities_ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPriceMyCompanyName.MyProjectName.Entities.Module_Pro~",
                table: "ProjectPriceMyCompanyName.MyProjectName.Entities.Module");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectPriceMyCompanyName.MyProjectName.Entities.Project_Pr~",
                table: "ProjectPriceMyCompanyName.MyProjectName.Entities.Project");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectRltModule_ProjectPriceMyCompanyName.MyProjectName.En~",
                table: "ProjectRltModule");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectRltModule_ProjectPriceMyCompanyName.MyProjectName.E~1",
                table: "ProjectRltModule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectRltModule",
                table: "ProjectRltModule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectPriceMyCompanyName.MyProjectName.Entities.Project",
                table: "ProjectPriceMyCompanyName.MyProjectName.Entities.Project");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectPriceMyCompanyName.MyProjectName.Entities.Module",
                table: "ProjectPriceMyCompanyName.MyProjectName.Entities.Module");

            migrationBuilder.RenameTable(
                name: "ProjectRltModule",
                newName: "ProjectPrice_MyCompanyName.MyProjectName.Entities.ProjectRltModule");

            migrationBuilder.RenameTable(
                name: "ProjectPriceMyCompanyName.MyProjectName.Entities.Project",
                newName: "ProjectPrice_MyCompanyName.MyProjectName.Entities.Project");

            migrationBuilder.RenameTable(
                name: "ProjectPriceMyCompanyName.MyProjectName.Entities.Module",
                newName: "ProjectPrice_MyCompanyName.MyProjectName.Entities.Module");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectRltModule_ProjectId",
                table: "ProjectPrice_MyCompanyName.MyProjectName.Entities.ProjectRltModule",
                newName: "IX_ProjectPrice_MyCompanyName.MyProjectName.Entities.ProjectR~1");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectRltModule_ModuleId",
                table: "ProjectPrice_MyCompanyName.MyProjectName.Entities.ProjectRltModule",
                newName: "IX_ProjectPrice_MyCompanyName.MyProjectName.Entities.ProjectRl~");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectPriceMyCompanyName.MyProjectName.Entities.Project_Pa~",
                table: "ProjectPrice_MyCompanyName.MyProjectName.Entities.Project",
                newName: "IX_ProjectPrice_MyCompanyName.MyProjectName.Entities.Project_P~");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectPriceMyCompanyName.MyProjectName.Entities.Module_Par~",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
                newName: "ProjectRltModule");

            migrationBuilder.RenameTable(
                name: "ProjectPrice_MyCompanyName.MyProjectName.Entities.Project",
                newName: "ProjectPriceMyCompanyName.MyProjectName.Entities.Project");

            migrationBuilder.RenameTable(
                name: "ProjectPrice_MyCompanyName.MyProjectName.Entities.Module",
                newName: "ProjectPriceMyCompanyName.MyProjectName.Entities.Module");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectPrice_MyCompanyName.MyProjectName.Entities.ProjectR~1",
                table: "ProjectRltModule",
                newName: "IX_ProjectRltModule_ProjectId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectPrice_MyCompanyName.MyProjectName.Entities.ProjectRl~",
                table: "ProjectRltModule",
                newName: "IX_ProjectRltModule_ModuleId");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectPrice_MyCompanyName.MyProjectName.Entities.Project_P~",
                table: "ProjectPriceMyCompanyName.MyProjectName.Entities.Project",
                newName: "IX_ProjectPriceMyCompanyName.MyProjectName.Entities.Project_Pa~");

            migrationBuilder.RenameIndex(
                name: "IX_ProjectPrice_MyCompanyName.MyProjectName.Entities.Module_Pa~",
                table: "ProjectPriceMyCompanyName.MyProjectName.Entities.Module",
                newName: "IX_ProjectPriceMyCompanyName.MyProjectName.Entities.Module_Par~");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectRltModule",
                table: "ProjectRltModule",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectPriceMyCompanyName.MyProjectName.Entities.Project",
                table: "ProjectPriceMyCompanyName.MyProjectName.Entities.Project",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectPriceMyCompanyName.MyProjectName.Entities.Module",
                table: "ProjectPriceMyCompanyName.MyProjectName.Entities.Module",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPriceMyCompanyName.MyProjectName.Entities.Module_Pro~",
                table: "ProjectPriceMyCompanyName.MyProjectName.Entities.Module",
                column: "ParentId",
                principalTable: "ProjectPriceMyCompanyName.MyProjectName.Entities.Module",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectPriceMyCompanyName.MyProjectName.Entities.Project_Pr~",
                table: "ProjectPriceMyCompanyName.MyProjectName.Entities.Project",
                column: "ParentId",
                principalTable: "ProjectPriceMyCompanyName.MyProjectName.Entities.Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectRltModule_ProjectPriceMyCompanyName.MyProjectName.En~",
                table: "ProjectRltModule",
                column: "ModuleId",
                principalTable: "ProjectPriceMyCompanyName.MyProjectName.Entities.Module",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectRltModule_ProjectPriceMyCompanyName.MyProjectName.E~1",
                table: "ProjectRltModule",
                column: "ProjectId",
                principalTable: "ProjectPriceMyCompanyName.MyProjectName.Entities.Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
