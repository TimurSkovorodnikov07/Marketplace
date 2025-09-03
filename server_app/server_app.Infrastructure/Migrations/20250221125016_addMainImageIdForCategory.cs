using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server_app.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addMainImageIdForCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "main_image_id",
                table: "product_categories",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "main_image_id",
                table: "product_categories");
        }
    }
}
