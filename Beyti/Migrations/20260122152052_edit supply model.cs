using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beyti.Migrations
{
    /// <inheritdoc />
    public partial class editsupplymodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Supplies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Supplies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
