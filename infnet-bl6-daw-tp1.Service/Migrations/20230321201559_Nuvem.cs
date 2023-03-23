using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infnet_bl6_daw_tp1.Service.Migrations
{
    /// <inheritdoc />
    public partial class Nuvem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_amigos",
                table: "amigos");

            migrationBuilder.RenameTable(
                name: "amigos",
                newName: "Amigos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Amigos",
                table: "Amigos",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Amigos",
                table: "Amigos");

            migrationBuilder.RenameTable(
                name: "Amigos",
                newName: "amigos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_amigos",
                table: "amigos",
                column: "Id");
        }
    }
}
