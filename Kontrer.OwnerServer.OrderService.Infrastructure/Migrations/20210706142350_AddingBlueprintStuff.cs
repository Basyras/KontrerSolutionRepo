using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Kontrer.OwnerServer.OrderService.Infrastructure.Migrations
{
    public partial class AddingBlueprintStuff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RoomEndDate",
                table: "RoomRequirement",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "RoomStartDate",
                table: "RoomRequirement",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RoomType",
                table: "RoomRequirement",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Requirment_From",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Requirment_To",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Orders_AccommodationItems",
                columns: table => new
                {
                    AccommodationRequirementAccommodationOrderEntityId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CostPerOne_Currency = table.Column<int>(type: "int", nullable: true),
                    CostPerOne_Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Count = table.Column<int>(type: "int", nullable: false),
                    CanParentApplyDiscount = table.Column<bool>(type: "bit", nullable: false),
                    TaxPercentageToAdd = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders_AccommodationItems", x => new { x.AccommodationRequirementAccommodationOrderEntityId, x.Id });
                    table.ForeignKey(
                        name: "FK_Orders_AccommodationItems_Orders_AccommodationRequirementAccommodationOrderEntityId",
                        column: x => x.AccommodationRequirementAccommodationOrderEntityId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders_Discounts",
                columns: table => new
                {
                    AccommodationRequirementAccommodationOrderEntityId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscountId = table.Column<int>(type: "int", nullable: true),
                    DiscountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPercentageDiscount = table.Column<bool>(type: "bit", nullable: false),
                    AmountDiscount_Currency = table.Column<int>(type: "int", nullable: true),
                    AmountDiscount_Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PercentageDiscount = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders_Discounts", x => new { x.AccommodationRequirementAccommodationOrderEntityId, x.Id });
                    table.ForeignKey(
                        name: "FK_Orders_Discounts_Orders_AccommodationRequirementAccommodationOrderEntityId",
                        column: x => x.AccommodationRequirementAccommodationOrderEntityId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "RoomRequirement_Discounts",
                columns: table => new
                {
                    RoomRequirementAccommodationRequirementAccommodationOrderEntityId = table.Column<int>(type: "int", nullable: false),
                    RoomRequirementId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscountId = table.Column<int>(type: "int", nullable: true),
                    DiscountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPercentageDiscount = table.Column<bool>(type: "bit", nullable: false),
                    AmountDiscount_Currency = table.Column<int>(type: "int", nullable: true),
                    AmountDiscount_Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PercentageDiscount = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomRequirement_Discounts", x => new { x.RoomRequirementAccommodationRequirementAccommodationOrderEntityId, x.RoomRequirementId, x.Id });
                    table.ForeignKey(
                        name: "FK_RoomRequirement_Discounts_RoomRequirement_RoomRequirementAccommodationRequirementAccommodationOrderEntityId_RoomRequirementId",
                        columns: x => new { x.RoomRequirementAccommodationRequirementAccommodationOrderEntityId, x.RoomRequirementId },
                        principalTable: "RoomRequirement",
                        principalColumns: new[] { "AccommodationRequirementAccommodationOrderEntityId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomRequirement_RoomItems",
                columns: table => new
                {
                    RoomRequirementAccommodationRequirementAccommodationOrderEntityId = table.Column<int>(type: "int", nullable: false),
                    RoomRequirementId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CostPerOne_Currency = table.Column<int>(type: "int", nullable: true),
                    CostPerOne_Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Count = table.Column<int>(type: "int", nullable: false),
                    CanParentApplyDiscount = table.Column<bool>(type: "bit", nullable: false),
                    TaxPercentageToAdd = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomRequirement_RoomItems", x => new { x.RoomRequirementAccommodationRequirementAccommodationOrderEntityId, x.RoomRequirementId, x.Id });
                    table.ForeignKey(
                        name: "FK_RoomRequirement_RoomItems_RoomRequirement_RoomRequirementAccommodationRequirementAccommodationOrderEntityId_RoomRequirementId",
                        columns: x => new { x.RoomRequirementAccommodationRequirementAccommodationOrderEntityId, x.RoomRequirementId },
                        principalTable: "RoomRequirement",
                        principalColumns: new[] { "AccommodationRequirementAccommodationOrderEntityId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders_AccommodationItems_Discounts",
                columns: table => new
                {
                    ItemRequirementAccommodationRequirementAccommodationOrderEntityId = table.Column<int>(type: "int", nullable: false),
                    ItemRequirementId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscountId = table.Column<int>(type: "int", nullable: true),
                    DiscountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPercentageDiscount = table.Column<bool>(type: "bit", nullable: false),
                    AmountDiscount_Currency = table.Column<int>(type: "int", nullable: true),
                    AmountDiscount_Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PercentageDiscount = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders_AccommodationItems_Discounts", x => new { x.ItemRequirementAccommodationRequirementAccommodationOrderEntityId, x.ItemRequirementId, x.Id });
                    table.ForeignKey(
                        name: "FK_Orders_AccommodationItems_Discounts_Orders_AccommodationItems_ItemRequirementAccommodationRequirementAccommodationOrderEntit~",
                        columns: x => new { x.ItemRequirementAccommodationRequirementAccommodationOrderEntityId, x.ItemRequirementId },
                        principalTable: "Orders_AccommodationItems",
                        principalColumns: new[] { "AccommodationRequirementAccommodationOrderEntityId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonBlueprint_Discounts",
                columns: table => new
                {
                    PersonBlueprintRoomRequirementAccommodationRequirementAccommodationOrderEntityId = table.Column<int>(type: "int", nullable: false),
                    PersonBlueprintRoomRequirementId = table.Column<int>(type: "int", nullable: false),
                    PersonBlueprintId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscountId = table.Column<int>(type: "int", nullable: true),
                    DiscountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPercentageDiscount = table.Column<bool>(type: "bit", nullable: false),
                    AmountDiscount_Currency = table.Column<int>(type: "int", nullable: true),
                    AmountDiscount_Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PercentageDiscount = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonBlueprint_Discounts", x => new { x.PersonBlueprintRoomRequirementAccommodationRequirementAccommodationOrderEntityId, x.PersonBlueprintRoomRequirementId, x.PersonBlueprintId, x.Id });
                    table.ForeignKey(
                        name: "FK_PersonBlueprint_Discounts_PersonBlueprint_PersonBlueprintRoomRequirementAccommodationRequirementAccommodationOrderEntityId_P~",
                        columns: x => new { x.PersonBlueprintRoomRequirementAccommodationRequirementAccommodationOrderEntityId, x.PersonBlueprintRoomRequirementId, x.PersonBlueprintId },
                        principalTable: "PersonBlueprint",
                        principalColumns: new[] { "RoomRequirementAccommodationRequirementAccommodationOrderEntityId", "RoomRequirementId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonBlueprint_PersonItems",
                columns: table => new
                {
                    PersonBlueprintRoomRequirementAccommodationRequirementAccommodationOrderEntityId = table.Column<int>(type: "int", nullable: false),
                    PersonBlueprintRoomRequirementId = table.Column<int>(type: "int", nullable: false),
                    PersonBlueprintId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ItemId = table.Column<int>(type: "int", nullable: true),
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CostPerOne_Currency = table.Column<int>(type: "int", nullable: true),
                    CostPerOne_Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Count = table.Column<int>(type: "int", nullable: false),
                    CanParentApplyDiscount = table.Column<bool>(type: "bit", nullable: false),
                    TaxPercentageToAdd = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonBlueprint_PersonItems", x => new { x.PersonBlueprintRoomRequirementAccommodationRequirementAccommodationOrderEntityId, x.PersonBlueprintRoomRequirementId, x.PersonBlueprintId, x.Id });
                    table.ForeignKey(
                        name: "FK_PersonBlueprint_PersonItems_PersonBlueprint_PersonBlueprintRoomRequirementAccommodationRequirementAccommodationOrderEntityId~",
                        columns: x => new { x.PersonBlueprintRoomRequirementAccommodationRequirementAccommodationOrderEntityId, x.PersonBlueprintRoomRequirementId, x.PersonBlueprintId },
                        principalTable: "PersonBlueprint",
                        principalColumns: new[] { "RoomRequirementAccommodationRequirementAccommodationOrderEntityId", "RoomRequirementId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomRequirement_RoomItems_Discounts",
                columns: table => new
                {
                    ItemRequirementRoomRequirementAccommodationRequirementAccommodationOrderEntityId = table.Column<int>(type: "int", nullable: false),
                    ItemRequirementRoomRequirementId = table.Column<int>(type: "int", nullable: false),
                    ItemRequirementId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscountId = table.Column<int>(type: "int", nullable: true),
                    DiscountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPercentageDiscount = table.Column<bool>(type: "bit", nullable: false),
                    AmountDiscount_Currency = table.Column<int>(type: "int", nullable: true),
                    AmountDiscount_Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PercentageDiscount = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomRequirement_RoomItems_Discounts", x => new { x.ItemRequirementRoomRequirementAccommodationRequirementAccommodationOrderEntityId, x.ItemRequirementRoomRequirementId, x.ItemRequirementId, x.Id });
                    table.ForeignKey(
                        name: "FK_RoomRequirement_RoomItems_Discounts_RoomRequirement_RoomItems_ItemRequirementRoomRequirementAccommodationRequirementAccommod~",
                        columns: x => new { x.ItemRequirementRoomRequirementAccommodationRequirementAccommodationOrderEntityId, x.ItemRequirementRoomRequirementId, x.ItemRequirementId },
                        principalTable: "RoomRequirement_RoomItems",
                        principalColumns: new[] { "RoomRequirementAccommodationRequirementAccommodationOrderEntityId", "RoomRequirementId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonBlueprint_PersonItems_Discounts",
                columns: table => new
                {
                    ItemRequirementPersonBlueprintRoomRequirementAccommodationRequirementAccommodationOrderEntityId = table.Column<int>(type: "int", nullable: false),
                    ItemRequirementPersonBlueprintRoomRequirementId = table.Column<int>(type: "int", nullable: false),
                    ItemRequirementPersonBlueprintId = table.Column<int>(type: "int", nullable: false),
                    ItemRequirementId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DiscountId = table.Column<int>(type: "int", nullable: true),
                    DiscountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPercentageDiscount = table.Column<bool>(type: "bit", nullable: false),
                    AmountDiscount_Currency = table.Column<int>(type: "int", nullable: true),
                    AmountDiscount_Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PercentageDiscount = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonBlueprint_PersonItems_Discounts", x => new { x.ItemRequirementPersonBlueprintRoomRequirementAccommodationRequirementAccommodationOrderEntityId, x.ItemRequirementPersonBlueprintRoomRequirementId, x.ItemRequirementPersonBlueprintId, x.ItemRequirementId, x.Id });
                    table.ForeignKey(
                        name: "FK_PersonBlueprint_PersonItems_Discounts_PersonBlueprint_PersonItems_ItemRequirementPersonBlueprintRoomRequirementAccommodation~",
                        columns: x => new { x.ItemRequirementPersonBlueprintRoomRequirementAccommodationRequirementAccommodationOrderEntityId, x.ItemRequirementPersonBlueprintRoomRequirementId, x.ItemRequirementPersonBlueprintId, x.ItemRequirementId },
                        principalTable: "PersonBlueprint_PersonItems",
                        principalColumns: new[] { "PersonBlueprintRoomRequirementAccommodationRequirementAccommodationOrderEntityId", "PersonBlueprintRoomRequirementId", "PersonBlueprintId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders_AccommodationItems_Discounts");

            migrationBuilder.DropTable(
                name: "Orders_Discounts");

            migrationBuilder.DropTable(
                name: "PersonBlueprint_Discounts");

            migrationBuilder.DropTable(
                name: "PersonBlueprint_PersonItems_Discounts");

            migrationBuilder.DropTable(
                name: "RoomRequirement_Discounts");

            migrationBuilder.DropTable(
                name: "RoomRequirement_RoomItems_Discounts");

            migrationBuilder.DropTable(
                name: "Orders_AccommodationItems");

            migrationBuilder.DropTable(
                name: "PersonBlueprint_PersonItems");

            migrationBuilder.DropTable(
                name: "RoomRequirement_RoomItems");

            migrationBuilder.DropTable(
                name: "PersonBlueprint");

            migrationBuilder.DropColumn(
                name: "RoomEndDate",
                table: "RoomRequirement");

            migrationBuilder.DropColumn(
                name: "RoomStartDate",
                table: "RoomRequirement");

            migrationBuilder.DropColumn(
                name: "RoomType",
                table: "RoomRequirement");

            migrationBuilder.DropColumn(
                name: "Requirment_From",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Requirment_To",
                table: "Orders");
        }
    }
}
