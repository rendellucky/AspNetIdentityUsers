using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HW10.Migrations
{
    public partial class AddUserSkillsSetMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSkill_Skill_SkillId",
                table: "UserSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkill_Users_UserId",
                table: "UserSkill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSkill",
                table: "UserSkill");

            migrationBuilder.RenameTable(
                name: "UserSkill",
                newName: "UserSkills");

            migrationBuilder.RenameIndex(
                name: "IX_UserSkill_UserId",
                table: "UserSkills",
                newName: "IX_UserSkills_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserSkill_SkillId",
                table: "UserSkills",
                newName: "IX_UserSkills_SkillId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSkills",
                table: "UserSkills",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkills_Skill_SkillId",
                table: "UserSkills",
                column: "SkillId",
                principalTable: "Skill",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkills_Users_UserId",
                table: "UserSkills",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSkills_Skill_SkillId",
                table: "UserSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkills_Users_UserId",
                table: "UserSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSkills",
                table: "UserSkills");

            migrationBuilder.RenameTable(
                name: "UserSkills",
                newName: "UserSkill");

            migrationBuilder.RenameIndex(
                name: "IX_UserSkills_UserId",
                table: "UserSkill",
                newName: "IX_UserSkill_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserSkills_SkillId",
                table: "UserSkill",
                newName: "IX_UserSkill_SkillId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSkill",
                table: "UserSkill",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkill_Skill_SkillId",
                table: "UserSkill",
                column: "SkillId",
                principalTable: "Skill",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkill_Users_UserId",
                table: "UserSkill",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
