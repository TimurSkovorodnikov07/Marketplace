using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server_app.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class sellerDescriptionConfWasChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "sellers",
                type: "character varying(125)",
                maxLength: 125,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "sellers",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(125)",
                oldMaxLength: 125);
        }
    }
}
