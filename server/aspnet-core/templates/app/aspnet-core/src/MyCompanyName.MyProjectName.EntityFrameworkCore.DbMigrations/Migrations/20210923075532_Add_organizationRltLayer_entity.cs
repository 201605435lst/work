using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyCompanyName.MyProjectName.Migrations
{
    public partial class Add_organizationRltLayer_entity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sn_Resource_OrganizationRltLayer",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OrganizationId = table.Column<Guid>(nullable: false),
                    LayerId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sn_Resource_OrganizationRltLayer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sn_Resource_OrganizationRltLayer_Sn_App_Organization_Organi~",
                        column: x => x.OrganizationId,
                        principalTable: "Sn_App_Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Resource_OrganizationRltLayer_OrganizationId",
                table: "Sn_Resource_OrganizationRltLayer",
                column: "OrganizationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sn_Resource_OrganizationRltLayer");
        }
    }
}
