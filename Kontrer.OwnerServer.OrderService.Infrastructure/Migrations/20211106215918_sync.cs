using Microsoft.EntityFrameworkCore.Migrations;

namespace Kontrer.OwnerServer.OrderService.Infrastructure.Migrations
{
    public partial class sync : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonBlueprint_Discounts_PersonBlueprint_PersonBlueprintRoomRequirementAccommodationRequirementAccommodationOrderEntityId_P~",
                table: "PersonBlueprint_Discounts");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonBlueprint_PersonItems_PersonBlueprint_PersonBlueprintRoomRequirementAccommodationRequirementAccommodationOrderEntityId~",
                table: "PersonBlueprint_PersonItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonBlueprint_PersonItems_Discounts_PersonBlueprint_PersonItems_ItemRequirementPersonBlueprintRoomRequirementAccommodation~",
                table: "PersonBlueprint_PersonItems_Discounts");

            migrationBuilder.DropTable(
                name: "PersonBlueprint");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonBlueprint_PersonItems_Discounts",
                table: "PersonBlueprint_PersonItems_Discounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonBlueprint_PersonItems",
                table: "PersonBlueprint_PersonItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonBlueprint_Discounts",
                table: "PersonBlueprint_Discounts");

            migrationBuilder.RenameTable(
                name: "PersonBlueprint_PersonItems_Discounts",
                newName: "PersonRequirement_PersonItems_Discounts");

            migrationBuilder.RenameTable(
                name: "PersonBlueprint_PersonItems",
                newName: "PersonRequirement_PersonItems");

            migrationBuilder.RenameTable(
                name: "PersonBlueprint_Discounts",
                newName: "PersonRequirement_Discounts");

            migrationBuilder.RenameColumn(
                name: "ItemRequirementPersonBlueprintId",
                table: "PersonRequirement_PersonItems_Discounts",
                newName: "ItemRequirementPersonRequirementId");

            migrationBuilder.RenameColumn(
                name: "ItemRequirementPersonBlueprintRoomRequirementId",
                table: "PersonRequirement_PersonItems_Discounts",
                newName: "ItemRequirementPersonRequirementRoomRequirementId");

            migrationBuilder.RenameColumn(
                name: "ItemRequirementPersonBlueprintRoomRequirementAccommodationRequirementAccommodationOrderEntityId",
                table: "PersonRequirement_PersonItems_Discounts",
                newName: "ItemRequirementPersonRequirementRoomRequirementAccommodationRequirementAccommodationOrderEntityId");

            migrationBuilder.RenameColumn(
                name: "PersonBlueprintId",
                table: "PersonRequirement_PersonItems",
                newName: "PersonRequirementId");

            migrationBuilder.RenameColumn(
                name: "PersonBlueprintRoomRequirementId",
                table: "PersonRequirement_PersonItems",
                newName: "PersonRequirementRoomRequirementId");

            migrationBuilder.RenameColumn(
                name: "PersonBlueprintRoomRequirementAccommodationRequirementAccommodationOrderEntityId",
                table: "PersonRequirement_PersonItems",
                newName: "PersonRequirementRoomRequirementAccommodationRequirementAccommodationOrderEntityId");

            migrationBuilder.RenameColumn(
                name: "PersonBlueprintId",
                table: "PersonRequirement_Discounts",
                newName: "PersonRequirementId");

            migrationBuilder.RenameColumn(
                name: "PersonBlueprintRoomRequirementId",
                table: "PersonRequirement_Discounts",
                newName: "PersonRequirementRoomRequirementId");

            migrationBuilder.RenameColumn(
                name: "PersonBlueprintRoomRequirementAccommodationRequirementAccommodationOrderEntityId",
                table: "PersonRequirement_Discounts",
                newName: "PersonRequirementRoomRequirementAccommodationRequirementAccommodationOrderEntityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonRequirement_PersonItems_Discounts",
                table: "PersonRequirement_PersonItems_Discounts",
                columns: new[] { "ItemRequirementPersonRequirementRoomRequirementAccommodationRequirementAccommodationOrderEntityId", "ItemRequirementPersonRequirementRoomRequirementId", "ItemRequirementPersonRequirementId", "ItemRequirementId", "Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonRequirement_PersonItems",
                table: "PersonRequirement_PersonItems",
                columns: new[] { "PersonRequirementRoomRequirementAccommodationRequirementAccommodationOrderEntityId", "PersonRequirementRoomRequirementId", "PersonRequirementId", "Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonRequirement_Discounts",
                table: "PersonRequirement_Discounts",
                columns: new[] { "PersonRequirementRoomRequirementAccommodationRequirementAccommodationOrderEntityId", "PersonRequirementRoomRequirementId", "PersonRequirementId", "Id" });

            migrationBuilder.CreateTable(
                name: "PersonRequirement",
                columns: table => new
                {
                    RoomRequirementAccommodationRequirementAccommodationOrderEntityId = table.Column<int>(type: "int", nullable: false),
                    RoomRequirementId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonRequirement", x => new { x.RoomRequirementAccommodationRequirementAccommodationOrderEntityId, x.RoomRequirementId, x.Id });
                    table.ForeignKey(
                        name: "FK_PersonRequirement_RoomRequirement_RoomRequirementAccommodationRequirementAccommodationOrderEntityId_RoomRequirementId",
                        columns: x => new { x.RoomRequirementAccommodationRequirementAccommodationOrderEntityId, x.RoomRequirementId },
                        principalTable: "RoomRequirement",
                        principalColumns: new[] { "AccommodationRequirementAccommodationOrderEntityId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_PersonRequirement_Discounts_PersonRequirement_PersonRequirementRoomRequirementAccommodationRequirementAccommodationOrderEnti~",
                table: "PersonRequirement_Discounts",
                columns: new[] { "PersonRequirementRoomRequirementAccommodationRequirementAccommodationOrderEntityId", "PersonRequirementRoomRequirementId", "PersonRequirementId" },
                principalTable: "PersonRequirement",
                principalColumns: new[] { "RoomRequirementAccommodationRequirementAccommodationOrderEntityId", "RoomRequirementId", "Id" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonRequirement_PersonItems_PersonRequirement_PersonRequirementRoomRequirementAccommodationRequirementAccommodationOrderEn~",
                table: "PersonRequirement_PersonItems",
                columns: new[] { "PersonRequirementRoomRequirementAccommodationRequirementAccommodationOrderEntityId", "PersonRequirementRoomRequirementId", "PersonRequirementId" },
                principalTable: "PersonRequirement",
                principalColumns: new[] { "RoomRequirementAccommodationRequirementAccommodationOrderEntityId", "RoomRequirementId", "Id" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonRequirement_PersonItems_Discounts_PersonRequirement_PersonItems_ItemRequirementPersonRequirementRoomRequirementAccommo~",
                table: "PersonRequirement_PersonItems_Discounts",
                columns: new[] { "ItemRequirementPersonRequirementRoomRequirementAccommodationRequirementAccommodationOrderEntityId", "ItemRequirementPersonRequirementRoomRequirementId", "ItemRequirementPersonRequirementId", "ItemRequirementId" },
                principalTable: "PersonRequirement_PersonItems",
                principalColumns: new[] { "PersonRequirementRoomRequirementAccommodationRequirementAccommodationOrderEntityId", "PersonRequirementRoomRequirementId", "PersonRequirementId", "Id" },
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PersonRequirement_Discounts_PersonRequirement_PersonRequirementRoomRequirementAccommodationRequirementAccommodationOrderEnti~",
                table: "PersonRequirement_Discounts");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonRequirement_PersonItems_PersonRequirement_PersonRequirementRoomRequirementAccommodationRequirementAccommodationOrderEn~",
                table: "PersonRequirement_PersonItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PersonRequirement_PersonItems_Discounts_PersonRequirement_PersonItems_ItemRequirementPersonRequirementRoomRequirementAccommo~",
                table: "PersonRequirement_PersonItems_Discounts");

            migrationBuilder.DropTable(
                name: "PersonRequirement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonRequirement_PersonItems_Discounts",
                table: "PersonRequirement_PersonItems_Discounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonRequirement_PersonItems",
                table: "PersonRequirement_PersonItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PersonRequirement_Discounts",
                table: "PersonRequirement_Discounts");

            migrationBuilder.RenameTable(
                name: "PersonRequirement_PersonItems_Discounts",
                newName: "PersonBlueprint_PersonItems_Discounts");

            migrationBuilder.RenameTable(
                name: "PersonRequirement_PersonItems",
                newName: "PersonBlueprint_PersonItems");

            migrationBuilder.RenameTable(
                name: "PersonRequirement_Discounts",
                newName: "PersonBlueprint_Discounts");

            migrationBuilder.RenameColumn(
                name: "ItemRequirementPersonRequirementId",
                table: "PersonBlueprint_PersonItems_Discounts",
                newName: "ItemRequirementPersonBlueprintId");

            migrationBuilder.RenameColumn(
                name: "ItemRequirementPersonRequirementRoomRequirementId",
                table: "PersonBlueprint_PersonItems_Discounts",
                newName: "ItemRequirementPersonBlueprintRoomRequirementId");

            migrationBuilder.RenameColumn(
                name: "ItemRequirementPersonRequirementRoomRequirementAccommodationRequirementAccommodationOrderEntityId",
                table: "PersonBlueprint_PersonItems_Discounts",
                newName: "ItemRequirementPersonBlueprintRoomRequirementAccommodationRequirementAccommodationOrderEntityId");

            migrationBuilder.RenameColumn(
                name: "PersonRequirementId",
                table: "PersonBlueprint_PersonItems",
                newName: "PersonBlueprintId");

            migrationBuilder.RenameColumn(
                name: "PersonRequirementRoomRequirementId",
                table: "PersonBlueprint_PersonItems",
                newName: "PersonBlueprintRoomRequirementId");

            migrationBuilder.RenameColumn(
                name: "PersonRequirementRoomRequirementAccommodationRequirementAccommodationOrderEntityId",
                table: "PersonBlueprint_PersonItems",
                newName: "PersonBlueprintRoomRequirementAccommodationRequirementAccommodationOrderEntityId");

            migrationBuilder.RenameColumn(
                name: "PersonRequirementId",
                table: "PersonBlueprint_Discounts",
                newName: "PersonBlueprintId");

            migrationBuilder.RenameColumn(
                name: "PersonRequirementRoomRequirementId",
                table: "PersonBlueprint_Discounts",
                newName: "PersonBlueprintRoomRequirementId");

            migrationBuilder.RenameColumn(
                name: "PersonRequirementRoomRequirementAccommodationRequirementAccommodationOrderEntityId",
                table: "PersonBlueprint_Discounts",
                newName: "PersonBlueprintRoomRequirementAccommodationRequirementAccommodationOrderEntityId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonBlueprint_PersonItems_Discounts",
                table: "PersonBlueprint_PersonItems_Discounts",
                columns: new[] { "ItemRequirementPersonBlueprintRoomRequirementAccommodationRequirementAccommodationOrderEntityId", "ItemRequirementPersonBlueprintRoomRequirementId", "ItemRequirementPersonBlueprintId", "ItemRequirementId", "Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonBlueprint_PersonItems",
                table: "PersonBlueprint_PersonItems",
                columns: new[] { "PersonBlueprintRoomRequirementAccommodationRequirementAccommodationOrderEntityId", "PersonBlueprintRoomRequirementId", "PersonBlueprintId", "Id" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PersonBlueprint_Discounts",
                table: "PersonBlueprint_Discounts",
                columns: new[] { "PersonBlueprintRoomRequirementAccommodationRequirementAccommodationOrderEntityId", "PersonBlueprintRoomRequirementId", "PersonBlueprintId", "Id" });

            migrationBuilder.CreateTable(
                name: "PersonBlueprint",
                columns: table => new
                {
                    RoomRequirementAccommodationRequirementAccommodationOrderEntityId = table.Column<int>(type: "int", nullable: false),
                    RoomRequirementId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonBlueprint", x => new { x.RoomRequirementAccommodationRequirementAccommodationOrderEntityId, x.RoomRequirementId, x.Id });
                    table.ForeignKey(
                        name: "FK_PersonBlueprint_RoomRequirement_RoomRequirementAccommodationRequirementAccommodationOrderEntityId_RoomRequirementId",
                        columns: x => new { x.RoomRequirementAccommodationRequirementAccommodationOrderEntityId, x.RoomRequirementId },
                        principalTable: "RoomRequirement",
                        principalColumns: new[] { "AccommodationRequirementAccommodationOrderEntityId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_PersonBlueprint_Discounts_PersonBlueprint_PersonBlueprintRoomRequirementAccommodationRequirementAccommodationOrderEntityId_P~",
                table: "PersonBlueprint_Discounts",
                columns: new[] { "PersonBlueprintRoomRequirementAccommodationRequirementAccommodationOrderEntityId", "PersonBlueprintRoomRequirementId", "PersonBlueprintId" },
                principalTable: "PersonBlueprint",
                principalColumns: new[] { "RoomRequirementAccommodationRequirementAccommodationOrderEntityId", "RoomRequirementId", "Id" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonBlueprint_PersonItems_PersonBlueprint_PersonBlueprintRoomRequirementAccommodationRequirementAccommodationOrderEntityId~",
                table: "PersonBlueprint_PersonItems",
                columns: new[] { "PersonBlueprintRoomRequirementAccommodationRequirementAccommodationOrderEntityId", "PersonBlueprintRoomRequirementId", "PersonBlueprintId" },
                principalTable: "PersonBlueprint",
                principalColumns: new[] { "RoomRequirementAccommodationRequirementAccommodationOrderEntityId", "RoomRequirementId", "Id" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PersonBlueprint_PersonItems_Discounts_PersonBlueprint_PersonItems_ItemRequirementPersonBlueprintRoomRequirementAccommodation~",
                table: "PersonBlueprint_PersonItems_Discounts",
                columns: new[] { "ItemRequirementPersonBlueprintRoomRequirementAccommodationRequirementAccommodationOrderEntityId", "ItemRequirementPersonBlueprintRoomRequirementId", "ItemRequirementPersonBlueprintId", "ItemRequirementId" },
                principalTable: "PersonBlueprint_PersonItems",
                principalColumns: new[] { "PersonBlueprintRoomRequirementAccommodationRequirementAccommodationOrderEntityId", "PersonBlueprintRoomRequirementId", "PersonBlueprintId", "Id" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
