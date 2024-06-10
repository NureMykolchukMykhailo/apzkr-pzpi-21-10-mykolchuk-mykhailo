using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APZ_backend.Migrations
{
    /// <inheritdoc />
    public partial class V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subordinates_Cars_CarId",
                table: "Subordinates");

            migrationBuilder.DropForeignKey(
                name: "FK_Subordinates_Users_ChiefId",
                table: "Subordinates");

            migrationBuilder.AddForeignKey(
                name: "FK_Subordinates_Cars_CarId",
                table: "Subordinates",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subordinates_Users_ChiefId",
                table: "Subordinates",
                column: "ChiefId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subordinates_Cars_CarId",
                table: "Subordinates");

            migrationBuilder.DropForeignKey(
                name: "FK_Subordinates_Users_ChiefId",
                table: "Subordinates");

            migrationBuilder.AddForeignKey(
                name: "FK_Subordinates_Cars_CarId",
                table: "Subordinates",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Subordinates_Users_ChiefId",
                table: "Subordinates",
                column: "ChiefId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
