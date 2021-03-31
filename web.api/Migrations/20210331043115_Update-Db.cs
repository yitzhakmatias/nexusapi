using Microsoft.EntityFrameworkCore.Migrations;

namespace web.api.Migrations
{
    public partial class UpdateDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Bills_BillId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_BillId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "BillId",
                table: "Clients");

            migrationBuilder.CreateIndex(
                name: "IX_Bills_ClientId",
                table: "Bills",
                column: "ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Clients_ClientId",
                table: "Bills",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Clients_ClientId",
                table: "Bills");

            migrationBuilder.DropIndex(
                name: "IX_Bills_ClientId",
                table: "Bills");

            migrationBuilder.AddColumn<int>(
                name: "BillId",
                table: "Clients",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_BillId",
                table: "Clients",
                column: "BillId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Bills_BillId",
                table: "Clients",
                column: "BillId",
                principalTable: "Bills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
