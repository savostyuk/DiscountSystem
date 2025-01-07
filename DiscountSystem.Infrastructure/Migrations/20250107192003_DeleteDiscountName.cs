using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DiscountSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DeleteDiscountName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountName",
                table: "Discounts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiscountName",
                table: "Discounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
