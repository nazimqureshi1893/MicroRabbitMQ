using Microsoft.EntityFrameworkCore.Migrations;

namespace MicroRabbit.Banking.Data.Migrations
{
    public partial class InitalMigrations3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AccountBlance",
                table: "Accounts",
                nullable: false,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "AccountBlance",
                table: "Accounts",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
