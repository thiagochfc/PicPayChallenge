using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PicPayChallenge.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            ArgumentNullException.ThrowIfNull(migrationBuilder);
            
            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid ", nullable: false),
                    fullName = table.Column<string>(type: "VARCHAR(150)", nullable: false),
                    cpfCnpj = table.Column<string>(type: "VARCHAR(14)", nullable: false),
                    email = table.Column<string>(type: "VARCHAR(150)", nullable: false),
                    password = table.Column<string>(type: "VARCHAR(150)", nullable: false),
                    type = table.Column<string>(type: "VARCHAR(11)", nullable: false),
                    balance = table.Column<decimal>(type: "numeric(13,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            ArgumentNullException.ThrowIfNull(migrationBuilder);
            
            migrationBuilder.DropTable(
                name: "accounts");
        }
    }
}
