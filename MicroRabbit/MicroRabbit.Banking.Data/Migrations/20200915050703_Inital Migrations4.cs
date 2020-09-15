using Microsoft.EntityFrameworkCore.Migrations;

namespace MicroRabbit.Banking.Data.Migrations
{
    public partial class InitalMigrations4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "AccountBlance",
                table: "Accounts",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AccountBlance",
                table: "Accounts",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
