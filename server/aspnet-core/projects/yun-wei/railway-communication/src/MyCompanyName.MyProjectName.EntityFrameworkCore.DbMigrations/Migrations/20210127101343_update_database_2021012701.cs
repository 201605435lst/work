using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyCompanyName.MyProjectName.Migrations
{
    public partial class update_database_2021012701 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sn_Schedule_ScheduleRltSchedule_Sn_Schedule_Schedule_FrontS~",
                table: "Sn_Schedule_ScheduleRltSchedule");

            migrationBuilder.DropIndex(
                name: "IX_Sn_Schedule_ScheduleRltSchedule_FrontScheduleId",
                table: "Sn_Schedule_ScheduleRltSchedule");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "Sn_Schedule_ScheduleRltSchedule");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "Sn_Schedule_ScheduleRltSchedule");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Sn_Schedule_ScheduleRltSchedule");

            migrationBuilder.AlterColumn<Guid>(
                name: "ScheduleId",
                table: "Sn_Schedule_ScheduleRltSchedule",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "FrontScheduleId",
                table: "Sn_Schedule_ScheduleRltSchedule",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "Sn_Schedule_Schedule",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "Sn_Schedule_Schedule",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Schedule_ScheduleRltSchedule_ScheduleId",
                table: "Sn_Schedule_ScheduleRltSchedule",
                column: "ScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sn_Schedule_ScheduleRltSchedule_Sn_Schedule_Schedule_Schedu~",
                table: "Sn_Schedule_ScheduleRltSchedule",
                column: "ScheduleId",
                principalTable: "Sn_Schedule_Schedule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sn_Schedule_ScheduleRltSchedule_Sn_Schedule_Schedule_Schedu~",
                table: "Sn_Schedule_ScheduleRltSchedule");

            migrationBuilder.DropIndex(
                name: "IX_Sn_Schedule_ScheduleRltSchedule_ScheduleId",
                table: "Sn_Schedule_ScheduleRltSchedule");

            migrationBuilder.AlterColumn<Guid>(
                name: "ScheduleId",
                table: "Sn_Schedule_ScheduleRltSchedule",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "FrontScheduleId",
                table: "Sn_Schedule_ScheduleRltSchedule",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "Sn_Schedule_ScheduleRltSchedule",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "Sn_Schedule_ScheduleRltSchedule",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Sn_Schedule_ScheduleRltSchedule",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "StartTime",
                table: "Sn_Schedule_Schedule",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndTime",
                table: "Sn_Schedule_Schedule",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sn_Schedule_ScheduleRltSchedule_FrontScheduleId",
                table: "Sn_Schedule_ScheduleRltSchedule",
                column: "FrontScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sn_Schedule_ScheduleRltSchedule_Sn_Schedule_Schedule_FrontS~",
                table: "Sn_Schedule_ScheduleRltSchedule",
                column: "FrontScheduleId",
                principalTable: "Sn_Schedule_Schedule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
