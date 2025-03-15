using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationBetweenApplicationUserAndRealEstateAd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Client",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Client_ApplicationUserId",
                table: "Client",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Client_AspNetUsers_ApplicationUserId",
                table: "Client",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Client_AspNetUsers_ApplicationUserId",
                table: "Client");

            migrationBuilder.DropIndex(
                name: "IX_Client_ApplicationUserId",
                table: "Client");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Client");
        }
    }
}
