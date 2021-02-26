using Microsoft.EntityFrameworkCore.Migrations;

namespace AddressBookApi.Migrations
{
    public partial class entities_updated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailAddress_Users_UserId",
                table: "EmailAddress");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmailAddress",
                table: "EmailAddress");

            migrationBuilder.RenameTable(
                name: "EmailAddress",
                newName: "EmailAddresses");

            migrationBuilder.RenameIndex(
                name: "IX_EmailAddress_UserId",
                table: "EmailAddresses",
                newName: "IX_EmailAddresses_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmailAddresses",
                table: "EmailAddresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailAddresses_Users_UserId",
                table: "EmailAddresses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailAddresses_Users_UserId",
                table: "EmailAddresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmailAddresses",
                table: "EmailAddresses");

            migrationBuilder.RenameTable(
                name: "EmailAddresses",
                newName: "EmailAddress");

            migrationBuilder.RenameIndex(
                name: "IX_EmailAddresses_UserId",
                table: "EmailAddress",
                newName: "IX_EmailAddress_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmailAddress",
                table: "EmailAddress",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailAddress_Users_UserId",
                table: "EmailAddress",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
