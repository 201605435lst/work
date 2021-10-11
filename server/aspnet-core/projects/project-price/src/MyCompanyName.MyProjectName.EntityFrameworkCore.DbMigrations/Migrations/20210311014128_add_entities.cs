using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyCompanyName.MyProjectName.Migrations
{
    public partial class add_entities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProjectPriceMyCompanyName.MyProjectName.Entities.Module",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    ParentId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    WorkDays = table.Column<float>(nullable: false),
                    Progress = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectPriceMyCompanyName.MyProjectName.Entities.Module", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectPriceMyCompanyName.MyProjectName.Entities.Module_Pro~",
                        column: x => x.ParentId,
                        principalTable: "ProjectPriceMyCompanyName.MyProjectName.Entities.Module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectPriceMyCompanyName.MyProjectName.Entities.Project",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    CreatorId = table.Column<Guid>(nullable: true),
                    LastModificationTime = table.Column<DateTime>(nullable: true),
                    LastModifierId = table.Column<Guid>(nullable: true),
                    ParentId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    SumPrice = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectPriceMyCompanyName.MyProjectName.Entities.Project", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectPriceMyCompanyName.MyProjectName.Entities.Project_Pr~",
                        column: x => x.ParentId,
                        principalTable: "ProjectPriceMyCompanyName.MyProjectName.Entities.Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectRltModule",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProjectId = table.Column<Guid>(nullable: false),
                    ModuleId = table.Column<Guid>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Price = table.Column<float>(nullable: false),
                    Remark = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectRltModule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectRltModule_ProjectPriceMyCompanyName.MyProjectName.En~",
                        column: x => x.ModuleId,
                        principalTable: "ProjectPriceMyCompanyName.MyProjectName.Entities.Module",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectRltModule_ProjectPriceMyCompanyName.MyProjectName.E~1",
                        column: x => x.ProjectId,
                        principalTable: "ProjectPriceMyCompanyName.MyProjectName.Entities.Project",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPriceMyCompanyName.MyProjectName.Entities.Module_Par~",
                table: "ProjectPriceMyCompanyName.MyProjectName.Entities.Module",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPriceMyCompanyName.MyProjectName.Entities.Project_Pa~",
                table: "ProjectPriceMyCompanyName.MyProjectName.Entities.Project",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectRltModule_ModuleId",
                table: "ProjectRltModule",
                column: "ModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectRltModule_ProjectId",
                table: "ProjectRltModule",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProjectRltModule");

            migrationBuilder.DropTable(
                name: "ProjectPriceMyCompanyName.MyProjectName.Entities.Module");

            migrationBuilder.DropTable(
                name: "ProjectPriceMyCompanyName.MyProjectName.Entities.Project");
        }
    }
}
