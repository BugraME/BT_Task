using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BT.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitializeDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TBL_LOG",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    YOL = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    KULLANICI_IP = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ISLEM_TARIHI = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HATA = table.Column<bool>(type: "bit", nullable: false),
                    HATAMESAJI = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBL_LOG", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TBL_LOG");
        }
    }
}
