using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoAnalyzer.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditMethodUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionHistories_Investments_InvestmentEntityId",
                table: "TransactionHistories");

            migrationBuilder.DropIndex(
                name: "IX_TransactionHistories_InvestmentEntityId",
                table: "TransactionHistories");

            migrationBuilder.DropColumn(
                name: "InvestmentEntityId",
                table: "TransactionHistories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InvestmentEntityId",
                table: "TransactionHistories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionHistories_InvestmentEntityId",
                table: "TransactionHistories",
                column: "InvestmentEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionHistories_Investments_InvestmentEntityId",
                table: "TransactionHistories",
                column: "InvestmentEntityId",
                principalTable: "Investments",
                principalColumn: "Id");
        }
    }
}
