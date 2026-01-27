using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddProductTableV3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "TBProduct",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TBProduct_UserId",
                table: "TBProduct",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TBProduct_TBUser_UserId",
                table: "TBProduct",
                column: "UserId",
                principalTable: "TBUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TBProduct_TBUser_UserId",
                table: "TBProduct");

            migrationBuilder.DropIndex(
                name: "IX_TBProduct_UserId",
                table: "TBProduct");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TBProduct");
        }
    }
}
