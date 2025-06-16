using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HouseBroker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initasdfasdfadsfial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c206ab53-aeba-492d-91b5-611eb97bb1f6",
                column: "PasswordHash",
                value: "AAQAAAAIAAYagAAAAEJ/ymT9arb6ihetqM+VwODZ1CT47Nbc6epAy5JVnl5zdmfSeSHkzoQbs7mQgMZQA/g==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c206ab53-aeba-492d-91b5-611eb97bb1f6",
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEMvwDQ6Axmnz+GvOhxVgOks+aAo3nhv4q8LJ06MvBPgNwUhlx+zhnFwqI6v07z9/ig==");
        }
    }
}
