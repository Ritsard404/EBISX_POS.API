﻿// <auto-generated />
using System;
using EBISX_POS.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EBISX_POS.API.Migrations.Journal
{
    [DbContext(typeof(JournalContext))]
    [Migration("20250407084321_EBISX_JOURNAL")]
    partial class EBISX_JOURNAL
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("EBISX_POS.API.Models.Journal.AccountJournal", b =>
                {
                    b.Property<long>("UniqueId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("unique_id");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("UniqueId"));

                    b.Property<double?>("AccountBalance")
                        .HasColumnType("double")
                        .HasColumnName("AccountBalance");

                    b.Property<string>("AccountName")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("AccountName");

                    b.Property<string>("AccountNo")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("accountno");

                    b.Property<string>("AmountInWords")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("amountinwords");

                    b.Property<decimal?>("Area")
                        .HasColumnType("decimal(12,2)")
                        .HasColumnName("area");

                    b.Property<string>("Bank")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("bank");

                    b.Property<string>("BatchNo")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("batchno");

                    b.Property<string>("BlockNo")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("block_no");

                    b.Property<string>("Branch")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("Branch");

                    b.Property<decimal>("Cbu")
                        .HasColumnType("decimal(12,2)")
                        .HasColumnName("cbu");

                    b.Property<string>("Chassis")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("chassis");

                    b.Property<string>("CheckNumber")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("check_number");

                    b.Property<string>("Cleared")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("cleared");

                    b.Property<int?>("ClearingRef")
                        .HasColumnType("int")
                        .HasColumnName("clearingref");

                    b.Property<string>("Clerk")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("clerk");

                    b.Property<string>("CogsAccount")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("cogs_account");

                    b.Property<string>("Collector")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("collector");

                    b.Property<decimal>("CollectorFee")
                        .HasColumnType("decimal(12,2)")
                        .HasColumnName("collector_fee");

                    b.Property<decimal>("Commission")
                        .HasColumnType("decimal(12,2)")
                        .HasColumnName("commission");

                    b.Property<string>("CompanyId")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("company_id");

                    b.Property<int>("Consumption")
                        .HasColumnType("int")
                        .HasColumnName("consumption");

                    b.Property<int?>("Converted")
                        .HasColumnType("int")
                        .HasColumnName("converted");

                    b.Property<double?>("Cost")
                        .HasColumnType("double")
                        .HasColumnName("Cost");

                    b.Property<string>("CostCenter")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("costcenter");

                    b.Property<string>("CostCenterDesc")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("costcenterdesc");

                    b.Property<double?>("Credit")
                        .HasColumnType("double")
                        .HasColumnName("Credit");

                    b.Property<int>("CurrReading")
                        .HasColumnType("int")
                        .HasColumnName("curr_reading");

                    b.Property<decimal>("DailyInterest")
                        .HasColumnType("decimal(12,2)")
                        .HasColumnName("daily_interest");

                    b.Property<DateTime>("DateStart")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("date_start");

                    b.Property<double?>("Debit")
                        .HasColumnType("double")
                        .HasColumnName("Debit");

                    b.Property<decimal>("Delivered")
                        .HasColumnType("decimal(10,0)")
                        .HasColumnName("delivered");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("Description");

                    b.Property<double?>("DiscAmt")
                        .HasColumnType("double")
                        .HasColumnName("Discamt");

                    b.Property<double?>("DiscRate")
                        .HasColumnType("double")
                        .HasColumnName("Discrate");

                    b.Property<string>("Dispensed")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("dispensed");

                    b.Property<string>("DocRef")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("docref");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("end_date");

                    b.Property<string>("Engine")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("engine");

                    b.Property<DateTime>("EntryDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("Entry_Date");

                    b.Property<int?>("EntryLineNo")
                        .HasColumnType("int")
                        .HasColumnName("Entry_Line_No");

                    b.Property<string>("EntryName")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("Entry_Name");

                    b.Property<long?>("EntryNo")
                        .HasColumnType("bigint")
                        .HasColumnName("Entry_No");

                    b.Property<TimeSpan?>("EntryTime")
                        .HasColumnType("time(6)")
                        .HasColumnName("entry_time");

                    b.Property<string>("EntryType")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("Entry_Type");

                    b.Property<double?>("Exempt")
                        .HasColumnType("double")
                        .HasColumnName("exempt");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("expirydate");

                    b.Property<string>("Face")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("face");

                    b.Property<string>("Fgid")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("fgid");

                    b.Property<string>("GroupId")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("group_id");

                    b.Property<string>("Illumination")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("illumination");

                    b.Property<string>("IncomeAccount")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("income_account");

                    b.Property<decimal>("Insurance")
                        .HasColumnType("decimal(12,2)")
                        .HasColumnName("insurance");

                    b.Property<double?>("Interest")
                        .HasColumnType("double")
                        .HasColumnName("interest");

                    b.Property<int>("InterestDays")
                        .HasColumnType("int")
                        .HasColumnName("interest_days");

                    b.Property<int>("InterestPeriodStart")
                        .HasColumnType("int")
                        .HasColumnName("interest_period_start");

                    b.Property<decimal>("InterestRate")
                        .HasColumnType("decimal(12,2)")
                        .HasColumnName("interest_rate");

                    b.Property<string>("InventoryAccount")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("inventory_account");

                    b.Property<string>("ItemColor")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("itemcolor");

                    b.Property<string>("ItemDesc")
                        .HasColumnType("longtext")
                        .HasColumnName("ItemDesc");

                    b.Property<string>("ItemID")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("ItemID");

                    b.Property<string>("ItemType")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("itemtype");

                    b.Property<string>("JobDesc")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("job_desc");

                    b.Property<string>("JobNo")
                        .HasColumnType("longtext")
                        .HasColumnName("job_no");

                    b.Property<decimal?>("Length")
                        .HasColumnType("decimal(12,2)")
                        .HasColumnName("length");

                    b.Property<string>("LineType")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("linetype");

                    b.Property<int?>("LineTypeTransNo")
                        .HasColumnType("int")
                        .HasColumnName("linetype_transno");

                    b.Property<int>("LoanNoOfPayments")
                        .HasColumnType("int")
                        .HasColumnName("loan_no_of_payments");

                    b.Property<int>("LoanPaymentsInterval")
                        .HasColumnType("int")
                        .HasColumnName("loan_payments_interval");

                    b.Property<string>("LoanPaymentsTerm")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("loan_payments_term");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("location");

                    b.Property<string>("LotNo")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("lot_no");

                    b.Property<string>("Memo")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("memo");

                    b.Property<string>("Mobilization")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("mobilization");

                    b.Property<string>("NameDesc")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("name_desc");

                    b.Property<string>("NameType")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("name_type");

                    b.Property<double?>("NonVatable")
                        .HasColumnType("double")
                        .HasColumnName("nonvatable");

                    b.Property<double?>("Penalty")
                        .HasColumnType("double")
                        .HasColumnName("penalty");

                    b.Property<string>("PenaltyPeriod")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("penalty_period");

                    b.Property<double?>("PenaltyRate")
                        .HasColumnType("double")
                        .HasColumnName("penalty_rate");

                    b.Property<int?>("PenaltyTerm")
                        .HasColumnType("int")
                        .HasColumnName("penalty_term");

                    b.Property<decimal?>("Perimeter")
                        .HasColumnType("decimal(12,2)")
                        .HasColumnName("perimeter");

                    b.Property<int>("PrevReading")
                        .HasColumnType("int")
                        .HasColumnName("prev_reading");

                    b.Property<double?>("Price")
                        .HasColumnType("double")
                        .HasColumnName("Price");

                    b.Property<double?>("Principal")
                        .HasColumnType("double")
                        .HasColumnName("principal");

                    b.Property<string>("ProdNo")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("prodno");

                    b.Property<double?>("QtyBalanceInBaseUnit")
                        .HasColumnType("double")
                        .HasColumnName("QtyBalanceInBaseUnit");

                    b.Property<double?>("QtyIn")
                        .HasColumnType("double")
                        .HasColumnName("QtyIn");

                    b.Property<double?>("QtyOut")
                        .HasColumnType("double")
                        .HasColumnName("QtyOut");

                    b.Property<double?>("QtyPerBaseUnit")
                        .HasColumnType("double")
                        .HasColumnName("QtyPerBaseUnit");

                    b.Property<double?>("Received")
                        .HasColumnType("double")
                        .HasColumnName("received");

                    b.Property<string>("Reference")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("Reference");

                    b.Property<decimal?>("Requested")
                        .HasColumnType("decimal(12,2)")
                        .HasColumnName("requested");

                    b.Property<string>("RoomId")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("room_id");

                    b.Property<decimal>("Savings")
                        .HasColumnType("decimal(12,2)")
                        .HasColumnName("savings");

                    b.Property<string>("Serial")
                        .HasColumnType("longtext")
                        .HasColumnName("serial");

                    b.Property<int?>("SerialStatus")
                        .HasColumnType("int")
                        .HasColumnName("serialstatus");

                    b.Property<string>("ShipTo")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("ship_to");

                    b.Property<string>("ShipToName")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("ship_to_name");

                    b.Property<string>("Size")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("size");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("Status");

                    b.Property<double?>("SubTotal")
                        .HasColumnType("double")
                        .HasColumnName("sub_total");

                    b.Property<string>("TaxAccount")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("tax_account");

                    b.Property<string>("TaxId")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("tax_id");

                    b.Property<double?>("TaxRate")
                        .HasColumnType("double")
                        .HasColumnName("tax_rate");

                    b.Property<double?>("TaxTotal")
                        .HasColumnType("double")
                        .HasColumnName("tax_total");

                    b.Property<string>("TaxType")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("tax_type");

                    b.Property<int?>("TerminalNo")
                        .HasColumnType("int")
                        .HasColumnName("TerminalNo");

                    b.Property<double?>("TotalCost")
                        .HasColumnType("double")
                        .HasColumnName("TotalCost");

                    b.Property<double?>("TotalLoanAmount")
                        .HasColumnType("double")
                        .HasColumnName("total_loan_amount");

                    b.Property<double?>("TotalPrice")
                        .HasColumnType("double")
                        .HasColumnName("TotalPrice");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("Unit");

                    b.Property<string>("VatType")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("vattype");

                    b.Property<double?>("Vatable")
                        .HasColumnType("double")
                        .HasColumnName("vatable");

                    b.Property<DateTime>("VoucherDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("voucher_date");

                    b.Property<decimal>("WTaxAmount")
                        .HasColumnType("decimal(12,2)")
                        .HasColumnName("wtax_amount");

                    b.Property<decimal>("WTaxRate")
                        .HasColumnType("decimal(12,2)")
                        .HasColumnName("wtax_rate");

                    b.Property<decimal?>("Width")
                        .HasColumnType("decimal(12,2)")
                        .HasColumnName("width");

                    b.Property<double?>("ZeroRated")
                        .HasColumnType("double")
                        .HasColumnName("zerorated");

                    b.HasKey("UniqueId");

                    b.ToTable("accountjournal");
                });

            modelBuilder.Entity("EBISX_POS.API.Models.Journal.SalesJournal", b =>
                {
                    b.Property<long>("TransNo")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("TransNo");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<long>("TransNo"));

                    b.Property<string>("Age")
                        .HasColumnType("longtext")
                        .HasColumnName("age");

                    b.Property<string>("Agent")
                        .HasColumnType("longtext")
                        .HasColumnName("agent");

                    b.Property<DateTime?>("ApproveDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("approve_date");

                    b.Property<string>("ApprovedBy")
                        .HasColumnType("longtext")
                        .HasColumnName("approved_by");

                    b.Property<DateTime?>("AssignDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("assign_date");

                    b.Property<string>("AssignFirstName")
                        .HasColumnType("longtext")
                        .HasColumnName("assign_firstname");

                    b.Property<string>("AssignLastName")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("assign_lastname");

                    b.Property<DateTime?>("BeneficiaryBday")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("beneficiary_bday");

                    b.Property<string>("BeneficiaryContact")
                        .HasColumnType("longtext")
                        .HasColumnName("beneficiary_contact");

                    b.Property<string>("BeneficiaryGender")
                        .HasColumnType("longtext")
                        .HasColumnName("beneficiary_gender");

                    b.Property<string>("BeneficiaryRelationship")
                        .HasColumnType("longtext")
                        .HasColumnName("beneficiary_relationship");

                    b.Property<string>("Branch")
                        .HasColumnType("longtext")
                        .HasColumnName("Branch");

                    b.Property<string>("ChargeAccount")
                        .HasColumnType("longtext")
                        .HasColumnName("charge_account");

                    b.Property<DateTime?>("ClaimDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("claim_date");

                    b.Property<string>("Collector")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("collector");

                    b.Property<string>("CompanyId")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("company_id");

                    b.Property<string>("ContactNumber")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("contact_number");

                    b.Property<string>("ContactPerson")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("contact_person");

                    b.Property<string>("ContactRelationship")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("contact_relationship");

                    b.Property<string>("CostCenter")
                        .HasColumnType("longtext")
                        .HasColumnName("costcenter");

                    b.Property<string>("Customer")
                        .HasColumnType("longtext")
                        .HasColumnName("Customer");

                    b.Property<string>("CustomerGroup")
                        .HasColumnType("longtext")
                        .HasColumnName("customergroup");

                    b.Property<string>("CustomerName")
                        .HasColumnType("longtext")
                        .HasColumnName("CustomerName");

                    b.Property<string>("CustomerPO")
                        .HasColumnType("longtext")
                        .HasColumnName("customer_po");

                    b.Property<DateTime?>("DeceaseDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("decease_date");

                    b.Property<string>("DeceaseStatus")
                        .HasColumnType("longtext")
                        .HasColumnName("decease_status");

                    b.Property<string>("DeceasedAge")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("deceased_age");

                    b.Property<string>("DeliveryAddress")
                        .HasColumnType("longtext")
                        .HasColumnName("delivery_address");

                    b.Property<DateTime?>("DeliveryDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("delivery_date");

                    b.Property<string>("DineTake")
                        .HasColumnType("longtext")
                        .HasColumnName("dine_take");

                    b.Property<decimal?>("DiscountAmount")
                        .HasColumnType("decimal(12,2)")
                        .HasColumnName("discount_amount");

                    b.Property<string>("DiscountType")
                        .HasColumnType("longtext")
                        .HasColumnName("discount_type");

                    b.Property<string>("Dispensed")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("dispensed");

                    b.Property<string>("DocRef")
                        .HasColumnType("longtext")
                        .HasColumnName("docref");

                    b.Property<DateTime?>("DueDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("DueDate");

                    b.Property<string>("EmpDriver")
                        .HasColumnType("longtext")
                        .HasColumnName("emp_driver");

                    b.Property<decimal>("ExemptSales")
                        .HasColumnType("decimal(12,2)")
                        .HasColumnName("exemptsales");

                    b.Property<string>("ExternalRef")
                        .HasColumnType("longtext")
                        .HasColumnName("externalref");

                    b.Property<string>("FirstName")
                        .HasColumnType("longtext")
                        .HasColumnName("firstname");

                    b.Property<string>("Gender")
                        .HasColumnType("longtext")
                        .HasColumnName("gender");

                    b.Property<double?>("GrossTotal")
                        .HasColumnType("double")
                        .HasColumnName("GrossTotal");

                    b.Property<string>("ImageLocation")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("imagelocation");

                    b.Property<string>("IntermentPlace")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("interment_place");

                    b.Property<string>("InvTaxId")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("inv_tax_id");

                    b.Property<string>("InvTaxType")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("inv_tax_type");

                    b.Property<decimal>("InvVatRate")
                        .HasColumnType("decimal(12,2)")
                        .HasColumnName("inv_vat_rate");

                    b.Property<string>("InvoiceType")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("invoice_type");

                    b.Property<string>("JobDesc")
                        .HasColumnType("longtext")
                        .HasColumnName("job_desc");

                    b.Property<string>("JobNo")
                        .HasColumnType("longtext")
                        .HasColumnName("job_no");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext")
                        .HasColumnName("lastname");

                    b.Property<DateTime>("LastPaymentDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("last_payment_date");

                    b.Property<string>("Location")
                        .HasColumnType("longtext")
                        .HasColumnName("location");

                    b.Property<decimal>("NetPayable")
                        .HasColumnType("decimal(12,2)")
                        .HasColumnName("netpayable");

                    b.Property<int>("NoOfPayments")
                        .HasColumnType("int")
                        .HasColumnName("no_of_payments");

                    b.Property<string>("Notes")
                        .HasColumnType("longtext")
                        .HasColumnName("notes");

                    b.Property<TimeSpan?>("OrderTime")
                        .HasColumnType("time(6)")
                        .HasColumnName("order_time");

                    b.Property<string>("Passenger")
                        .HasColumnType("longtext")
                        .HasColumnName("passenger");

                    b.Property<string>("PayorAddress")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("payor_address");

                    b.Property<string>("PayorMobile")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("payor_mobile");

                    b.Property<string>("PayorName")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("payor_name");

                    b.Property<string>("PayorRelationship")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("payor_relationship");

                    b.Property<TimeSpan?>("PostTime")
                        .HasColumnType("time(6)")
                        .HasColumnName("post_time");

                    b.Property<string>("Reference")
                        .HasColumnType("longtext")
                        .HasColumnName("Reference");

                    b.Property<string>("Remarks")
                        .HasColumnType("longtext")
                        .HasColumnName("remarks");

                    b.Property<int?>("ReprintCount")
                        .HasColumnType("int")
                        .HasColumnName("reprint_count");

                    b.Property<string>("SaleTaxMethod")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("sale_tax_method");

                    b.Property<string>("SaleType")
                        .HasColumnType("longtext")
                        .HasColumnName("sale_type");

                    b.Property<string>("Status")
                        .HasColumnType("longtext")
                        .HasColumnName("Status");

                    b.Property<double?>("SubTotal")
                        .HasColumnType("double")
                        .HasColumnName("SubTotal");

                    b.Property<string>("TenderReference")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("tender_reference");

                    b.Property<string>("TenderType")
                        .HasColumnType("longtext")
                        .HasColumnName("tender_type");

                    b.Property<double?>("TotalTax")
                        .HasColumnType("double")
                        .HasColumnName("TotalTax");

                    b.Property<DateTime?>("TransDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("TransDate");

                    b.Property<decimal>("VatSales")
                        .HasColumnType("decimal(12,2)")
                        .HasColumnName("vatsales");

                    b.Property<string>("ViewingPlace")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("viewing_place");

                    b.Property<string>("Voyage")
                        .HasColumnType("longtext")
                        .HasColumnName("voyage");

                    b.Property<string>("WTaxAccount")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("wtaxaccount");

                    b.Property<decimal>("WTaxAmt")
                        .HasColumnType("decimal(12,2)")
                        .HasColumnName("wtaxamt");

                    b.Property<decimal>("WTaxRate")
                        .HasColumnType("decimal(12,2)")
                        .HasColumnName("wtaxrate");

                    b.Property<string>("ZeroExempt")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("zeroexempt");

                    b.Property<decimal>("ZeroSales")
                        .HasColumnType("decimal(12,2)")
                        .HasColumnName("zerosales");

                    b.HasKey("TransNo");

                    b.ToTable("salesjournal");
                });
#pragma warning restore 612, 618
        }
    }
}
