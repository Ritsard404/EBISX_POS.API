using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EBISX_POS.API.Migrations.Journal
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "accountjournal",
                columns: table => new
                {
                    unique_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Entry_Type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Entry_No = table.Column<long>(type: "bigint", nullable: true),
                    Entry_Line_No = table.Column<int>(type: "int", nullable: true),
                    Entry_Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Entry_Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    group_id = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    AccountName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Reference = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Branch = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TerminalNo = table.Column<int>(type: "int", nullable: true),
                    Debit = table.Column<double>(type: "double", nullable: true),
                    Credit = table.Column<double>(type: "double", nullable: true),
                    AccountBalance = table.Column<double>(type: "double", nullable: true),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cleared = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    clearingref = table.Column<int>(type: "int", nullable: true),
                    costcenter = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    accountno = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    costcenterdesc = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    linetype = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    linetype_transno = table.Column<int>(type: "int", nullable: true),
                    ItemID = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ItemDesc = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Unit = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    QtyIn = table.Column<double>(type: "double", nullable: true),
                    QtyOut = table.Column<double>(type: "double", nullable: true),
                    QtyPerBaseUnit = table.Column<double>(type: "double", nullable: true),
                    QtyBalanceInBaseUnit = table.Column<double>(type: "double", nullable: true),
                    Cost = table.Column<double>(type: "double", nullable: true),
                    Price = table.Column<double>(type: "double", nullable: true),
                    Discrate = table.Column<double>(type: "double", nullable: true),
                    Discamt = table.Column<double>(type: "double", nullable: true),
                    TotalCost = table.Column<double>(type: "double", nullable: true),
                    TotalPrice = table.Column<double>(type: "double", nullable: true),
                    received = table.Column<double>(type: "double", nullable: true),
                    delivered = table.Column<decimal>(type: "decimal(10,0)", nullable: false),
                    tax_id = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tax_account = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tax_type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tax_rate = table.Column<double>(type: "double", nullable: true),
                    tax_total = table.Column<double>(type: "double", nullable: true),
                    sub_total = table.Column<double>(type: "double", nullable: true),
                    serial = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    chassis = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    engine = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    itemtype = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    serialstatus = table.Column<int>(type: "int", nullable: true),
                    expirydate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    batchno = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    itemcolor = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    converted = table.Column<int>(type: "int", nullable: true),
                    vattype = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    vatable = table.Column<double>(type: "double", nullable: true),
                    exempt = table.Column<double>(type: "double", nullable: true),
                    nonvatable = table.Column<double>(type: "double", nullable: true),
                    zerorated = table.Column<double>(type: "double", nullable: true),
                    income_account = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cogs_account = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    inventory_account = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    job_no = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    job_desc = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name_type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    docref = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name_desc = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    length = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    width = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    area = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    perimeter = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    fgid = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    illumination = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    size = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    face = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    end_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    location = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    principal = table.Column<double>(type: "double", nullable: true),
                    interest = table.Column<double>(type: "double", nullable: true),
                    penalty = table.Column<double>(type: "double", nullable: true),
                    total_loan_amount = table.Column<double>(type: "double", nullable: true),
                    penalty_rate = table.Column<double>(type: "double", nullable: true),
                    penalty_term = table.Column<int>(type: "int", nullable: true),
                    penalty_period = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    bank = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    check_number = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    amountinwords = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    voucher_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ship_to = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ship_to_name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    clerk = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    requested = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    entry_time = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    prodno = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dispensed = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    prev_reading = table.Column<int>(type: "int", nullable: false),
                    curr_reading = table.Column<int>(type: "int", nullable: false),
                    consumption = table.Column<int>(type: "int", nullable: false),
                    memo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    mobilization = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    room_id = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_start = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    wtax_rate = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    wtax_amount = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    block_no = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    lot_no = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    interest_days = table.Column<int>(type: "int", nullable: false),
                    daily_interest = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    interest_rate = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    interest_period_start = table.Column<int>(type: "int", nullable: false),
                    loan_no_of_payments = table.Column<int>(type: "int", nullable: false),
                    loan_payments_interval = table.Column<int>(type: "int", nullable: false),
                    loan_payments_term = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    savings = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    cbu = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    collector = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    insurance = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    commission = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    collector_fee = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    company_id = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accountjournal", x => x.unique_id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "salesjournal",
                columns: table => new
                {
                    TransNo = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    sale_type = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    TransDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    costcenter = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Reference = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    docref = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    externalref = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Customer = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CustomerName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    customergroup = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Branch = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SubTotal = table.Column<double>(type: "double", nullable: true),
                    TotalTax = table.Column<double>(type: "double", nullable: true),
                    GrossTotal = table.Column<double>(type: "double", nullable: true),
                    Status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    job_no = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    job_desc = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    reprint_count = table.Column<int>(type: "int", nullable: true),
                    notes = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    remarks = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    delivery_address = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    location = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    voyage = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    passenger = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    age = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    gender = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    agent = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    delivery_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    emp_driver = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    approved_by = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    approve_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    order_time = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    post_time = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    discount_type = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    discount_amount = table.Column<decimal>(type: "decimal(12,2)", nullable: true),
                    tender_type = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    customer_po = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dine_take = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dispensed = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tender_reference = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    contact_person = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    contact_number = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    inv_vat_rate = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    sale_tax_method = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    inv_tax_id = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    inv_tax_type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    wtaxaccount = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    wtaxrate = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    wtaxamt = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    netpayable = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    imagelocation = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    invoice_type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    collector = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    last_payment_date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    no_of_payments = table.Column<int>(type: "int", nullable: false),
                    firstname = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    lastname = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    charge_account = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    beneficiary_bday = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    beneficiary_relationship = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    beneficiary_gender = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    beneficiary_contact = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    assign_firstname = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    assign_lastname = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    assign_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    claim_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    decease_status = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    decease_date = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    company_id = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    vatsales = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    zerosales = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    exemptsales = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    zeroexempt = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    payor_name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    payor_address = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    payor_relationship = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    payor_mobile = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    viewing_place = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    interment_place = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    contact_relationship = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    deceased_age = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_salesjournal", x => x.TransNo);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accountjournal");

            migrationBuilder.DropTable(
                name: "salesjournal");
        }
    }
}
