﻿using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBISX_POS.API.Migrations
{
    /// <inheritdoc />
    public partial class EBISX_POS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AddOnType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AddOnTypeName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddOnType", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CtgryName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "DrinkType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DrinkTypeName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DrinkType", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserEmail = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserFName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserLName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserRole = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserEmail);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Menu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MenuName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MenuPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    MenuImagePath = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Size = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MenuIsAvailable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HasDrink = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HasAddOn = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsAddOn = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DrinkTypeId = table.Column<int>(type: "int", nullable: true),
                    AddOnTypeId = table.Column<int>(type: "int", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menu_AddOnType_AddOnTypeId",
                        column: x => x.AddOnTypeId,
                        principalTable: "AddOnType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Menu_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Menu_DrinkType_DrinkTypeId",
                        column: x => x.DrinkTypeId,
                        principalTable: "DrinkType",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderType = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TotalAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    IsCancelled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsPending = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CashierUserEmail = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ManagerUserEmail = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_User_CashierUserEmail",
                        column: x => x.CashierUserEmail,
                        principalTable: "User",
                        principalColumn: "UserEmail",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_User_ManagerUserEmail",
                        column: x => x.ManagerUserEmail,
                        principalTable: "User",
                        principalColumn: "UserEmail");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Timestamp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TsIn = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    TsOut = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    TsBreakOut = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    TsBreakIn = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    CashierUserEmail = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ManagerInUserEmail = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ManagerOutUserEmail = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ManagerBreakInUserEmail = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ManagerBreakOutUserEmail = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timestamp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Timestamp_User_CashierUserEmail",
                        column: x => x.CashierUserEmail,
                        principalTable: "User",
                        principalColumn: "UserEmail",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Timestamp_User_ManagerBreakInUserEmail",
                        column: x => x.ManagerBreakInUserEmail,
                        principalTable: "User",
                        principalColumn: "UserEmail");
                    table.ForeignKey(
                        name: "FK_Timestamp_User_ManagerBreakOutUserEmail",
                        column: x => x.ManagerBreakOutUserEmail,
                        principalTable: "User",
                        principalColumn: "UserEmail");
                    table.ForeignKey(
                        name: "FK_Timestamp_User_ManagerInUserEmail",
                        column: x => x.ManagerInUserEmail,
                        principalTable: "User",
                        principalColumn: "UserEmail",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Timestamp_User_ManagerOutUserEmail",
                        column: x => x.ManagerOutUserEmail,
                        principalTable: "User",
                        principalColumn: "UserEmail");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ItemQTY = table.Column<int>(type: "int", nullable: true),
                    ItemPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    IsVoid = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    MenuId = table.Column<int>(type: "int", nullable: true),
                    DrinkId = table.Column<int>(type: "int", nullable: true),
                    AddOnId = table.Column<int>(type: "int", nullable: true),
                    MealId = table.Column<int>(type: "int", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    VoidedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Item_Item_MealId",
                        column: x => x.MealId,
                        principalTable: "Item",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Item_Menu_AddOnId",
                        column: x => x.AddOnId,
                        principalTable: "Menu",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Item_Menu_DrinkId",
                        column: x => x.DrinkId,
                        principalTable: "Menu",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Item_Menu_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menu",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Item_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Item_AddOnId",
                table: "Item",
                column: "AddOnId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_DrinkId",
                table: "Item",
                column: "DrinkId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_MealId",
                table: "Item",
                column: "MealId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_MenuId",
                table: "Item",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_Item_OrderId",
                table: "Item",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_AddOnTypeId",
                table: "Menu",
                column: "AddOnTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_CategoryId",
                table: "Menu",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_DrinkTypeId",
                table: "Menu",
                column: "DrinkTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CashierUserEmail",
                table: "Order",
                column: "CashierUserEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ManagerUserEmail",
                table: "Order",
                column: "ManagerUserEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Timestamp_CashierUserEmail",
                table: "Timestamp",
                column: "CashierUserEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Timestamp_ManagerBreakInUserEmail",
                table: "Timestamp",
                column: "ManagerBreakInUserEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Timestamp_ManagerBreakOutUserEmail",
                table: "Timestamp",
                column: "ManagerBreakOutUserEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Timestamp_ManagerInUserEmail",
                table: "Timestamp",
                column: "ManagerInUserEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Timestamp_ManagerOutUserEmail",
                table: "Timestamp",
                column: "ManagerOutUserEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "Timestamp");

            migrationBuilder.DropTable(
                name: "Menu");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "AddOnType");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "DrinkType");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
