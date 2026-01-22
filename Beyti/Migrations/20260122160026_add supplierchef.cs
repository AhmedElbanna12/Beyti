using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beyti.Migrations
{
    /// <inheritdoc />
    public partial class addsupplierchef : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SupplierChefs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierProfileId = table.Column<int>(type: "int", nullable: false),
                    ChefProfileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierChefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierChefs_ChefProfiles_ChefProfileId",
                        column: x => x.ChefProfileId,
                        principalTable: "ChefProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupplierChefs_SupplierProfiles_SupplierProfileId",
                        column: x => x.SupplierProfileId,
                        principalTable: "SupplierProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SupplierChefs_ChefProfileId",
                table: "SupplierChefs",
                column: "ChefProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierChefs_SupplierProfileId",
                table: "SupplierChefs",
                column: "SupplierProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SupplierChefs");
        }
    }
}
