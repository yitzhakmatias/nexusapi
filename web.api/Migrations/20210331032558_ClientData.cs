using Microsoft.EntityFrameworkCore.Migrations;

namespace web.api.Migrations
{
    public partial class ClientData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] {"Name"},
                values: new object[] {"John Smith"});
            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Name" },
                values: new object[] { "Peter Sulivan" });
            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Name" },
                values: new object[] { "Mark Smith" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}