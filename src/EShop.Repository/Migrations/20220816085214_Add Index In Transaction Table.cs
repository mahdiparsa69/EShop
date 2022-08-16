using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EShop.Repository.Migrations
{
    public partial class AddIndexInTransactionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_transactions_order_id",
                table: "transactions");

            migrationBuilder.DropIndex(
                name: "ix_transactions_status",
                table: "transactions");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_order_id_status",
                table: "transactions",
                columns: new[] { "order_id", "status" },
                unique: true,
                filter: "NOT is_deleted AND status=1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_transactions_order_id_status",
                table: "transactions");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_order_id",
                table: "transactions",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_transactions_status",
                table: "transactions",
                column: "status",
                unique: true,
                filter: "NOT is_deleted");
        }
    }
}
