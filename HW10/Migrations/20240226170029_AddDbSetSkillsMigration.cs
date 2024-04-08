using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HW10.Migrations
{
    public partial class AddDbSetSkillsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skill_Images_ImageId",
                table: "Skill");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkills_Skill_SkillId",
                table: "UserSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Skill",
                table: "Skill");

            migrationBuilder.RenameTable(
                name: "Skill",
                newName: "Skills");

            migrationBuilder.RenameIndex(
                name: "IX_Skill_ImageId",
                table: "Skills",
                newName: "IX_Skills_ImageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Skills",
                table: "Skills",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_Images_ImageId",
                table: "Skills",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkills_Skills_SkillId",
                table: "UserSkills",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skills_Images_ImageId",
                table: "Skills");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkills_Skills_SkillId",
                table: "UserSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Skills",
                table: "Skills");

            migrationBuilder.RenameTable(
                name: "Skills",
                newName: "Skill");

            migrationBuilder.RenameIndex(
                name: "IX_Skills_ImageId",
                table: "Skill",
                newName: "IX_Skill_ImageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Skill",
                table: "Skill",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_Images_ImageId",
                table: "Skill",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkills_Skill_SkillId",
                table: "UserSkills",
                column: "SkillId",
                principalTable: "Skill",
                principalColumn: "Id");
        }
    }
}
