﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KryptonAPI.Migrations
{
    public partial class KryptonAPI0001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    StatusId = table.Column<long>(nullable: false)
                        .Annotation("Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.StatusId);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    JobId = table.Column<long>(nullable: false)
                        .Annotation("Autoincrement", true),
                    CreatedUTC = table.Column<DateTime>(nullable: false),
                    FinalResult = table.Column<byte[]>(nullable: true),
                    ModifiedUTC = table.Column<DateTime>(nullable: false),
                    StatusId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.JobId);
                    table.ForeignKey(
                        name: "FK_Jobs_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JobItems",
                columns: table => new
                {
                    JobItemId = table.Column<long>(nullable: false)
                        .Annotation("Autoincrement", true),
                    CreatedUTC = table.Column<DateTime>(nullable: false),
                    JobId = table.Column<long>(nullable: false),
                    JsonResult = table.Column<string>(nullable: true),
                    ModifiedUTC = table.Column<DateTime>(nullable: false),
                    StatusId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobItems", x => x.JobItemId);
                    table.ForeignKey(
                        name: "FK_JobItems_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "JobId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JobItems_Statuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Statuses",
                        principalColumn: "StatusId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_StatusId",
                table: "Jobs",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_JobItems_JobId",
                table: "JobItems",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_JobItems_StatusId",
                table: "JobItems",
                column: "StatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobItems");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "Statuses");
        }
    }
}