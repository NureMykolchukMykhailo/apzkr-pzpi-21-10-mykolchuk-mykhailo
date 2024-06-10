using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APZ_backend.Migrations
{
    /// <inheritdoc />
    public partial class V6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TestData",
                table: "Records",
                newName: "RightTurns");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Records",
                newName: "TripStart");

            migrationBuilder.AddColumn<int>(
                name: "DangerousLeftTurns",
                table: "Records",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DangerousRightTurns",
                table: "Records",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FastStart",
                table: "Records",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LeftTurns",
                table: "Records",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "TripEnd",
                table: "Records",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "EngineSpeeds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordId = table.Column<int>(type: "int", nullable: false),
                    Begin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    End = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AvgEngineSpeed = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EngineSpeeds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EngineSpeeds_Records_RecordId",
                        column: x => x.RecordId,
                        principalTable: "Records",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SuddenBraking",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecordId = table.Column<int>(type: "int", nullable: false),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InitialSpeed = table.Column<double>(type: "float", nullable: false),
                    SubsequentSpeed = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuddenBraking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuddenBraking_Records_RecordId",
                        column: x => x.RecordId,
                        principalTable: "Records",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EngineSpeeds_RecordId",
                table: "EngineSpeeds",
                column: "RecordId");

            migrationBuilder.CreateIndex(
                name: "IX_SuddenBraking_RecordId",
                table: "SuddenBraking",
                column: "RecordId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EngineSpeeds");

            migrationBuilder.DropTable(
                name: "SuddenBraking");

            migrationBuilder.DropColumn(
                name: "DangerousLeftTurns",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "DangerousRightTurns",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "FastStart",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "LeftTurns",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "TripEnd",
                table: "Records");

            migrationBuilder.RenameColumn(
                name: "TripStart",
                table: "Records",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "RightTurns",
                table: "Records",
                newName: "TestData");
        }
    }
}
