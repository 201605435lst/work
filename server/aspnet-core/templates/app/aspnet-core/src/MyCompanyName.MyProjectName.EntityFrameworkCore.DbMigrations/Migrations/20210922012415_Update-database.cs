using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyCompanyName.MyProjectName.Migrations
{
    public partial class Updatedatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrganizationRootTagId",
                table: "Sn_Construction_UnplannedTask");

            migrationBuilder.DropColumn(
                name: "ProjectTagId",
                table: "Sn_Construction_UnplannedTask");

            migrationBuilder.DropColumn(
                name: "OrganizationRootTagId",
                table: "Sn_Construction_DispatchRltStandard");

            migrationBuilder.DropColumn(
                name: "ProjectTagId",
                table: "Sn_Construction_DispatchRltStandard");

            migrationBuilder.DropColumn(
                name: "OrganizationRootTagId",
                table: "Sn_Construction_DispatchRltSection");

            migrationBuilder.DropColumn(
                name: "ProjectTagId",
                table: "Sn_Construction_DispatchRltSection");

            migrationBuilder.DropColumn(
                name: "OrganizationRootTagId",
                table: "Sn_Construction_DispatchRltMaterial");

            migrationBuilder.DropColumn(
                name: "ProjectTagId",
                table: "Sn_Construction_DispatchRltMaterial");

            migrationBuilder.DropColumn(
                name: "OrganizationRootTagId",
                table: "Sn_Construction_DailyRltSafe");

            migrationBuilder.DropColumn(
                name: "ProjectTagId",
                table: "Sn_Construction_DailyRltSafe");

            migrationBuilder.DropColumn(
                name: "OrganizationRootTagId",
                table: "Sn_Construction_DailyRltPlanMaterial");

            migrationBuilder.DropColumn(
                name: "ProjectTagId",
                table: "Sn_Construction_DailyRltPlanMaterial");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationRootTagId",
                table: "Sn_Construction_UnplannedTask",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectTagId",
                table: "Sn_Construction_UnplannedTask",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationRootTagId",
                table: "Sn_Construction_DispatchRltStandard",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectTagId",
                table: "Sn_Construction_DispatchRltStandard",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationRootTagId",
                table: "Sn_Construction_DispatchRltSection",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectTagId",
                table: "Sn_Construction_DispatchRltSection",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationRootTagId",
                table: "Sn_Construction_DispatchRltMaterial",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectTagId",
                table: "Sn_Construction_DispatchRltMaterial",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationRootTagId",
                table: "Sn_Construction_DailyRltSafe",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectTagId",
                table: "Sn_Construction_DailyRltSafe",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationRootTagId",
                table: "Sn_Construction_DailyRltPlanMaterial",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectTagId",
                table: "Sn_Construction_DailyRltPlanMaterial",
                type: "uuid",
                nullable: true);
        }
    }
}
