using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WearZone.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductSchema_Images : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SecondaryImageUrl",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecondaryImageUrl",
                table: "Products");
        }
    }
}
