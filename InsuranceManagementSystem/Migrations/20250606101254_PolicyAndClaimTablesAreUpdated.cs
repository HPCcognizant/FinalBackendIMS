using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InsuranceManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class PolicyAndClaimTablesAreUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ValidityPeriod",
                table: "Policies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AddColumn<decimal>(
                name: "IssuredValue",
                table: "Policies",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "EndDate",
                table: "CustomerPolicies",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<decimal>(
                name: "PayableAmount",
                table: "CustomerPolicies",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "PaymentFrequency",
                table: "CustomerPolicies",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "StartDate",
                table: "CustomerPolicies",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "AdminReason",
                table: "Claims",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClaimReason",
                table: "Claims",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IssuredValue",
                table: "Policies");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "CustomerPolicies");

            migrationBuilder.DropColumn(
                name: "PayableAmount",
                table: "CustomerPolicies");

            migrationBuilder.DropColumn(
                name: "PaymentFrequency",
                table: "CustomerPolicies");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "CustomerPolicies");

            migrationBuilder.DropColumn(
                name: "AdminReason",
                table: "Claims");

            migrationBuilder.DropColumn(
                name: "ClaimReason",
                table: "Claims");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "ValidityPeriod",
                table: "Policies",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1),
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
