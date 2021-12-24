using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiCors.Migrations
{
    public partial class updatedRefreshTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Revoked",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "RevokedByIp",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "RevokedByToken",
                table: "RefreshToken");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Revoked",
                table: "RefreshToken",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RevokedByIp",
                table: "RefreshToken",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RevokedByToken",
                table: "RefreshToken",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
