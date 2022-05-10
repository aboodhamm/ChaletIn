using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chaletin.Migrations
{
    public partial class addBooked : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Booked",
                table: "Farm",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Booked",
                table: "Farm");
        }
    }
}
