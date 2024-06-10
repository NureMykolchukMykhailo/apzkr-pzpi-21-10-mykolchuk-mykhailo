using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APZ_backend.Migrations
{
    /// <inheritdoc />
    public partial class V5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_Cars_Id",
                table: "Sensors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sensors",
                table: "Sensors"
                );

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Sensors");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Sensors",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_CarId",
                table: "Sensors",
                column: "CarId",
                unique: true,
                filter: "[CarId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_Cars_CarId",
                table: "Sensors",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sensors",
                table: "Sensors",
                column: "Id"
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_Cars_CarId",
                table: "Sensors");

            migrationBuilder.DropIndex(
                name: "IX_Sensors_CarId",
                table: "Sensors");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Sensors",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_Cars_Id",
                table: "Sensors",
                column: "Id",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
