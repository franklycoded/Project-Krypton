using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KryptonAPI.Migrations
{
    public partial class KryptonAPI0002 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "JobItems",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JsonData",
                table: "JobItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "JobItems");

            migrationBuilder.DropColumn(
                name: "JsonData",
                table: "JobItems");
        }
    }
}
