using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBISX_POS.API.Migrations
{
    /// <inheritdoc />
    public partial class ebisx_pos : Migration
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
                name: "PosTerminalInfo",
                columns: table => new
                {
                    PosSerialNumber = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    MinNumber = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccreditationNumber = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PtuNumber = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateIssued = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ValidUntil = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    RegisteredName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OperatedBy = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    VatTinNumber = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ResetCounterNo = table.Column<int>(type: "int", nullable: false),
                    ZCounterNo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PosTerminalInfo", x => x.PosSerialNumber);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SaleType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Account = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleType", x => x.Id);
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
                name: "Timestamp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TsIn = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    TsOut = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    CashInDrawerAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    CashOutDrawerAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    CashierUserEmail = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ManagerInUserEmail = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ManagerOutUserEmail = table.Column<string>(type: "varchar(255)", nullable: true)
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
                name: "AlternativePayments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Reference = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Amount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
                    SaleTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlternativePayments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlternativePayments_SaleType_SaleTypeId",
                        column: x => x.SaleTypeId,
                        principalTable: "SaleType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CouponPromo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PromoCode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CouponCode = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PromoAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    CouponItemQuantity = table.Column<int>(type: "int", nullable: true),
                    IsAvailable = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ExpirationTime = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    OrderId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouponPromo", x => x.Id);
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
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    CouponPromoId = table.Column<int>(type: "int", nullable: true)
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
                        name: "FK_Menu_CouponPromo_CouponPromoId",
                        column: x => x.CouponPromoId,
                        principalTable: "CouponPromo",
                        principalColumn: "Id");
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
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OrderType = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TotalAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    CashTendered = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    DueAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    TotalTendered = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    ChangeAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    VatSales = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    VatExempt = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    VatAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: false),
                    IsCancelled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsReturned = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsRead = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsPending = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DiscountType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DiscountAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    EligiblePwdScCount = table.Column<int>(type: "int", nullable: true),
                    EligiblePwdScNames = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    OSCAIdsNum = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PromoId = table.Column<int>(type: "int", nullable: true),
                    CashierUserEmail = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Order_CouponPromo_PromoId",
                        column: x => x.PromoId,
                        principalTable: "CouponPromo",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Order_User_CashierUserEmail",
                        column: x => x.CashierUserEmail,
                        principalTable: "User",
                        principalColumn: "UserEmail",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EntryId = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ItemQTY = table.Column<int>(type: "int", nullable: true),
                    ItemPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    IsVoid = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsPwdDiscounted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsSeniorDiscounted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    MenuId = table.Column<int>(type: "int", nullable: true),
                    DrinkId = table.Column<int>(type: "int", nullable: true),
                    AddOnId = table.Column<int>(type: "int", nullable: true),
                    MealId = table.Column<long>(type: "bigint", nullable: true),
                    OrderId = table.Column<long>(type: "bigint", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "ManagerLog",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TimestampId = table.Column<int>(type: "int", nullable: true),
                    ManagerUserEmail = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Action = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WithdrawAmount = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    OrderId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagerLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ManagerLog_Order_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Order",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ManagerLog_Timestamp_TimestampId",
                        column: x => x.TimestampId,
                        principalTable: "Timestamp",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ManagerLog_User_ManagerUserEmail",
                        column: x => x.ManagerUserEmail,
                        principalTable: "User",
                        principalColumn: "UserEmail",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AlternativePayments_OrderId",
                table: "AlternativePayments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_AlternativePayments_SaleTypeId",
                table: "AlternativePayments",
                column: "SaleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CouponPromo_OrderId",
                table: "CouponPromo",
                column: "OrderId");

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
                name: "IX_ManagerLog_ManagerUserEmail",
                table: "ManagerLog",
                column: "ManagerUserEmail");

            migrationBuilder.CreateIndex(
                name: "IX_ManagerLog_OrderId",
                table: "ManagerLog",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagerLog_TimestampId",
                table: "ManagerLog",
                column: "TimestampId");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_AddOnTypeId",
                table: "Menu",
                column: "AddOnTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_CategoryId",
                table: "Menu",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_CouponPromoId",
                table: "Menu",
                column: "CouponPromoId");

            migrationBuilder.CreateIndex(
                name: "IX_Menu_DrinkTypeId",
                table: "Menu",
                column: "DrinkTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CashierUserEmail",
                table: "Order",
                column: "CashierUserEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Order_PromoId",
                table: "Order",
                column: "PromoId");

            migrationBuilder.CreateIndex(
                name: "IX_Timestamp_CashierUserEmail",
                table: "Timestamp",
                column: "CashierUserEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Timestamp_ManagerInUserEmail",
                table: "Timestamp",
                column: "ManagerInUserEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Timestamp_ManagerOutUserEmail",
                table: "Timestamp",
                column: "ManagerOutUserEmail");

            migrationBuilder.AddForeignKey(
                name: "FK_AlternativePayments_Order_OrderId",
                table: "AlternativePayments",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CouponPromo_Order_OrderId",
                table: "CouponPromo",
                column: "OrderId",
                principalTable: "Order",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CouponPromo_Order_OrderId",
                table: "CouponPromo");

            migrationBuilder.DropTable(
                name: "AlternativePayments");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "ManagerLog");

            migrationBuilder.DropTable(
                name: "PosTerminalInfo");

            migrationBuilder.DropTable(
                name: "SaleType");

            migrationBuilder.DropTable(
                name: "Menu");

            migrationBuilder.DropTable(
                name: "Timestamp");

            migrationBuilder.DropTable(
                name: "AddOnType");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "DrinkType");

            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "CouponPromo");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
