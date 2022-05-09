using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chaletin.Migrations
{
    public partial class update8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageSource",
                table: "Farm",
                newName: "SwimmingPoolDescription");

            migrationBuilder.AddColumn<string>(
                name: "BathRoomDescription",
                table: "Farm",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BedRoomDescription",
                table: "Farm",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KitchenDescription",
                table: "Farm",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LivingRoomDescription",
                table: "Farm",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicUtilityDescription",
                table: "Farm",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FarmId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comments_Farm_FarmId",
                        column: x => x.FarmId,
                        principalTable: "Farm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_FarmId",
                table: "Comments",
                column: "FarmId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropColumn(
                name: "BathRoomDescription",
                table: "Farm");

            migrationBuilder.DropColumn(
                name: "BedRoomDescription",
                table: "Farm");

            migrationBuilder.DropColumn(
                name: "KitchenDescription",
                table: "Farm");

            migrationBuilder.DropColumn(
                name: "LivingRoomDescription",
                table: "Farm");

            migrationBuilder.DropColumn(
                name: "PublicUtilityDescription",
                table: "Farm");

            migrationBuilder.RenameColumn(
                name: "SwimmingPoolDescription",
                table: "Farm",
                newName: "ImageSource");
        }
    }
}
