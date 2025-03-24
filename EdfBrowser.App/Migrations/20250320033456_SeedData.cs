using Microsoft.EntityFrameworkCore.Migrations;

namespace EdfBrowser.App.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ActionItems",
                columns: new[] { "Id", "Description", "Title" },
                values: new object[] { -1, "Open local edf files", "Open File" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ActionItems",
                keyColumn: "Id",
                keyValue: -1);
        }
    }
}
