using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelInsuranceManagementSystem.Application.Migrations
{
    /// <inheritdoc />
    public partial class FixPolicyUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Policies_Users_UserId",
                table: "Policies");

            migrationBuilder.DropForeignKey(
                name: "FK_PolicyMember_PolicyMember_PolicyMemberMemberId",
                table: "PolicyMember");

            migrationBuilder.DropIndex(
                name: "IX_PolicyMember_PolicyMemberMemberId",
                table: "PolicyMember");

            migrationBuilder.DropColumn(
                name: "PolicyMemberMemberId",
                table: "PolicyMember");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Policies",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Policies_Users_UserId",
                table: "Policies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Policies_Users_UserId",
                table: "Policies");

            migrationBuilder.AddColumn<int>(
                name: "PolicyMemberMemberId",
                table: "PolicyMember",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Policies",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PolicyMember_PolicyMemberMemberId",
                table: "PolicyMember",
                column: "PolicyMemberMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_Policies_Users_UserId",
                table: "Policies",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PolicyMember_PolicyMember_PolicyMemberMemberId",
                table: "PolicyMember",
                column: "PolicyMemberMemberId",
                principalTable: "PolicyMember",
                principalColumn: "MemberId");
        }
    }
}
