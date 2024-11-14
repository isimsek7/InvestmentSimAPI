using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CryptoAnalyzer.Data.Migrations
{
    /// <inheritdoc />
    public partial class SurelyNotInnit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Investments_Portfolios_PortfolioId",
                table: "Investments");

            migrationBuilder.AddForeignKey(
                name: "FK_Investments_Portfolios_PortfolioId",
                table: "Investments",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Investments_Portfolios_PortfolioId",
                table: "Investments");

            migrationBuilder.AddForeignKey(
                name: "FK_Investments_Portfolios_PortfolioId",
                table: "Investments",
                column: "PortfolioId",
                principalTable: "Portfolios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
