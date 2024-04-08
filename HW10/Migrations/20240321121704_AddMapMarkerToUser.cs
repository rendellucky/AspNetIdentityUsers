using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HW10.Migrations
{
    public partial class AddMapMarkerToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MapMarkerId",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_MapMarkerId",
                table: "AspNetUsers",
                column: "MapMarkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_MapMarkers_MapMarkerId",
                table: "AspNetUsers",
                column: "MapMarkerId",
                principalTable: "MapMarkers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_MapMarkers_MapMarkerId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_MapMarkerId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "MapMarkerId",
                table: "AspNetUsers");
        }
    }
}
