using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangeProductThumbnailImageIdToOptional2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ThumbnailImages_ThumbnailImageId",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "ThumbnailImageId",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ThumbnailImages_ThumbnailImageId",
                table: "Products",
                column: "ThumbnailImageId",
                principalTable: "ThumbnailImages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ThumbnailImages_ThumbnailImageId",
                table: "Products");

            migrationBuilder.AlterColumn<int>(
                name: "ThumbnailImageId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ThumbnailImages_ThumbnailImageId",
                table: "Products",
                column: "ThumbnailImageId",
                principalTable: "ThumbnailImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
