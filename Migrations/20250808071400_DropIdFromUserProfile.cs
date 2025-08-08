using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ICTDashboard.Migrations
{
    /// <inheritdoc />
    public partial class DropIdFromUserProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserProfiles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserProfiles",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
