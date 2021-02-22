using Microsoft.EntityFrameworkCore.Migrations;

namespace Kontrer.OwnerServer.IdGeneratorService.Presentation.AspApi.Migrations
{
    public partial class First : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LastUsedIds",
                columns: table => new
                {
                    GroupName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LastUsedId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LastUsedIds", x => x.GroupName);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LastUsedIds");
        }
    }
}
