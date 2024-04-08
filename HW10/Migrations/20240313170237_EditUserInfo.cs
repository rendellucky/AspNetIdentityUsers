using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HW10.Migrations
{
    public partial class EditUserInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Users_UserId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Groups_GroupId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Images_ImageId",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkills_Users_UserId",
                table: "UserSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "UserInfos");

            migrationBuilder.RenameIndex(
                name: "IX_Users_ImageId",
                table: "UserInfos",
                newName: "IX_UserInfos_ImageId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_GroupId",
                table: "UserInfos",
                newName: "IX_UserInfos_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserInfos",
                table: "UserInfos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_UserInfos_UserId",
                table: "Images",
                column: "UserId",
                principalTable: "UserInfos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserInfos_Groups_GroupId",
                table: "UserInfos",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserInfos_Images_ImageId",
                table: "UserInfos",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkills_UserInfos_UserId",
                table: "UserSkills",
                column: "UserId",
                principalTable: "UserInfos",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_UserInfos_UserId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInfos_Groups_GroupId",
                table: "UserInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_UserInfos_Images_ImageId",
                table: "UserInfos");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkills_UserInfos_UserId",
                table: "UserSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserInfos",
                table: "UserInfos");

            migrationBuilder.RenameTable(
                name: "UserInfos",
                newName: "Users");

            migrationBuilder.RenameIndex(
                name: "IX_UserInfos_ImageId",
                table: "Users",
                newName: "IX_Users_ImageId");

            migrationBuilder.RenameIndex(
                name: "IX_UserInfos_GroupId",
                table: "Users",
                newName: "IX_Users_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Users_UserId",
                table: "Images",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Groups_GroupId",
                table: "Users",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Images_ImageId",
                table: "Users",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkills_Users_UserId",
                table: "UserSkills",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
