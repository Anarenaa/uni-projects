using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Migrations
{
    /// <inheritdoc />
    public partial class ChangeLogModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EntityData",
                table: "Logs",
                newName: "AuthorEmail");

            migrationBuilder.AddColumn<int>(
                name: "EntityId",
                table: "Logs",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "Logs");

            migrationBuilder.RenameColumn(
                name: "AuthorEmail",
                table: "Logs",
                newName: "EntityData");
        }
    }
}
