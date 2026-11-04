using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WearZone.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreProductImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BackImageUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SideImageUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackImageUrl",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SideImageUrl",
                table: "Products");
        }
    }
}
