using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BussinessObjects.Migrations
{
    /// <inheritdoc />
    public partial class secondInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostInteractions_Users_UserId",
                table: "PostInteractions");

            migrationBuilder.AddForeignKey(
                name: "FK_PostInteractions_Users_UserId",
                table: "PostInteractions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostInteractions_Users_UserId",
                table: "PostInteractions");

            migrationBuilder.AddForeignKey(
                name: "FK_PostInteractions_Users_UserId",
                table: "PostInteractions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
