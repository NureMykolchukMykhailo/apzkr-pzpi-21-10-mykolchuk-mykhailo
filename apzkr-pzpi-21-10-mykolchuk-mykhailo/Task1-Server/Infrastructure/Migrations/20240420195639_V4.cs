using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APZ_backend.Migrations
{
    /// <inheritdoc />
    public partial class V4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CarId",
                table: "Subordinates",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Sensors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_OwnerId",
                table: "Sensors",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_Users_OwnerId",
                table: "Sensors",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_Users_OwnerId",
                table: "Sensors");

            migrationBuilder.DropIndex(
                name: "IX_Sensors_OwnerId",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Sensors");

            migrationBuilder.AlterColumn<int>(
                name: "CarId",
                table: "Subordinates",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
