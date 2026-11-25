using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EcobinDeliveryService.Repostiory.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "deliveries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    SellerId = table.Column<long>(type: "bigint", nullable: false),
                    CollectorId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    SelectedScheduleId = table.Column<long>(type: "bigint", nullable: true),
                    PickupAddress = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_deliveries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "delivery_schedule_options",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DeliveryId = table.Column<long>(type: "bigint", nullable: false),
                    StartAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    EndAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedByUserId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_delivery_schedule_options", x => x.Id);
                    table.ForeignKey(
                        name: "FK_delivery_schedule_options_deliveries_DeliveryId",
                        column: x => x.DeliveryId,
                        principalTable: "deliveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "delivery_status_histories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DeliveryId = table.Column<long>(type: "bigint", nullable: false),
                    PreviousStatus = table.Column<int>(type: "integer", nullable: true),
                    NewStatus = table.Column<int>(type: "integer", nullable: false),
                    ChangedByUserId = table.Column<long>(type: "bigint", nullable: false),
                    ChangedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    Remarks = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_delivery_status_histories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_delivery_status_histories_deliveries_DeliveryId",
                        column: x => x.DeliveryId,
                        principalTable: "deliveries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_deliveries_CollectorId",
                table: "deliveries",
                column: "CollectorId");

            migrationBuilder.CreateIndex(
                name: "IX_deliveries_ProductId",
                table: "deliveries",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_deliveries_SelectedScheduleId",
                table: "deliveries",
                column: "SelectedScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_deliveries_SellerId",
                table: "deliveries",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_delivery_schedule_options_DeliveryId",
                table: "delivery_schedule_options",
                column: "DeliveryId");

            migrationBuilder.CreateIndex(
                name: "IX_delivery_status_histories_DeliveryId",
                table: "delivery_status_histories",
                column: "DeliveryId");

            migrationBuilder.AddForeignKey(
                name: "FK_deliveries_delivery_schedule_options_SelectedScheduleId",
                table: "deliveries",
                column: "SelectedScheduleId",
                principalTable: "delivery_schedule_options",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_deliveries_delivery_schedule_options_SelectedScheduleId",
                table: "deliveries");

            migrationBuilder.DropTable(
                name: "delivery_status_histories");

            migrationBuilder.DropTable(
                name: "delivery_schedule_options");

            migrationBuilder.DropTable(
                name: "deliveries");
        }
    }
}
