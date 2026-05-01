using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeliveryFee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_employee_work_profiles_Employees_EmployeeId1",
                schema: "hr",
                table: "employee_work_profiles");

            migrationBuilder.DropIndex(
                name: "IX_employee_work_profiles_EmployeeId1",
                schema: "hr",
                table: "employee_work_profiles");

            migrationBuilder.DropColumn(
                name: "EmployeeId1",
                schema: "hr",
                table: "employee_work_profiles");

            migrationBuilder.AddColumn<decimal>(
                name: "delivery_fee",
                schema: "DeliveryOrder",
                table: "DeliveryOrders",
                type: "numeric(22,4)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "delivery_fee",
                schema: "DeliveryOrder",
                table: "DeliveryOrders");

            migrationBuilder.AddColumn<Guid>(
                name: "EmployeeId1",
                schema: "hr",
                table: "employee_work_profiles",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_employee_work_profiles_EmployeeId1",
                schema: "hr",
                table: "employee_work_profiles",
                column: "EmployeeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_employee_work_profiles_Employees_EmployeeId1",
                schema: "hr",
                table: "employee_work_profiles",
                column: "EmployeeId1",
                principalSchema: "hr",
                principalTable: "Employees",
                principalColumn: "EmployeeID");
        }
    }
}
