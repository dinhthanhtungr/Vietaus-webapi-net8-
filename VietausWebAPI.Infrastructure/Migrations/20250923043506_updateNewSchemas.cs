using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietausWebAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateNewSchemas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Customer");

            migrationBuilder.EnsureSchema(
                name: "Material");

            migrationBuilder.EnsureSchema(
                name: "SampleRequests");

            migrationBuilder.EnsureSchema(
                name: "Others");

            migrationBuilder.RenameTable(
                name: "Units",
                schema: "inventory",
                newName: "Units",
                newSchema: "Material");

            migrationBuilder.RenameTable(
                name: "Suppliers",
                schema: "inventory",
                newName: "Suppliers",
                newSchema: "Material");

            migrationBuilder.RenameTable(
                name: "SupplierContacts",
                schema: "inventory",
                newName: "SupplierContacts",
                newSchema: "Material");

            migrationBuilder.RenameTable(
                name: "SupplierAddresses",
                schema: "inventory",
                newName: "SupplierAddresses",
                newSchema: "Material");

            migrationBuilder.RenameTable(
                name: "SampleRequests",
                schema: "labs",
                newName: "SampleRequests",
                newSchema: "SampleRequests");

            migrationBuilder.RenameTable(
                name: "SampleRequestImages",
                schema: "labs",
                newName: "SampleRequestImages",
                newSchema: "SampleRequests");

            migrationBuilder.RenameTable(
                name: "PurchaseOrderStatusHistory",
                schema: "inventory",
                newName: "PurchaseOrderStatusHistory",
                newSchema: "Orders");

            migrationBuilder.RenameTable(
                name: "PurchaseOrdersSchedules",
                schema: "inventory",
                newName: "PurchaseOrdersSchedules",
                newSchema: "Others");

            migrationBuilder.RenameTable(
                name: "PurchaseOrders",
                schema: "inventory",
                newName: "PurchaseOrders",
                newSchema: "Orders");

            migrationBuilder.RenameTable(
                name: "PurchaseOrderDetails",
                schema: "inventory",
                newName: "PurchaseOrderDetails",
                newSchema: "Orders");

            migrationBuilder.RenameTable(
                name: "Products",
                schema: "labs",
                newName: "Products",
                newSchema: "SampleRequests");

            migrationBuilder.RenameTable(
                name: "ProductChangedHistory",
                schema: "labs",
                newName: "ProductChangedHistory",
                newSchema: "SampleRequests");

            migrationBuilder.RenameTable(
                name: "PriceHistory",
                schema: "inventory",
                newName: "PriceHistory",
                newSchema: "Material");

            migrationBuilder.RenameTable(
                name: "Materials_Suppliers",
                schema: "inventory",
                newName: "Materials_Suppliers",
                newSchema: "Material");

            migrationBuilder.RenameTable(
                name: "Materials",
                schema: "inventory",
                newName: "Materials",
                newSchema: "Material");

            migrationBuilder.RenameTable(
                name: "FormulaStatusLog",
                schema: "labs",
                newName: "FormulaStatusLog",
                newSchema: "SampleRequests");

            migrationBuilder.RenameTable(
                name: "Formulas",
                schema: "labs",
                newName: "Formulas",
                newSchema: "SampleRequests");

            migrationBuilder.RenameTable(
                name: "FormulaMaterials",
                schema: "labs",
                newName: "FormulaMaterials",
                newSchema: "SampleRequests");

            migrationBuilder.RenameTable(
                name: "DetailCustomerTransfer",
                schema: "sales",
                newName: "DetailCustomerTransfer",
                newSchema: "Customer");

            migrationBuilder.RenameTable(
                name: "CustomerTransferLog",
                schema: "sales",
                newName: "CustomerTransferLog",
                newSchema: "Customer");

            migrationBuilder.RenameTable(
                name: "CustomerAssignment",
                schema: "sales",
                newName: "CustomerAssignment",
                newSchema: "Customer");

            migrationBuilder.RenameTable(
                name: "Customer",
                schema: "sales",
                newName: "Customer",
                newSchema: "Customer");

            migrationBuilder.RenameTable(
                name: "Contacts",
                schema: "sales",
                newName: "Contacts",
                newSchema: "Customer");

            migrationBuilder.RenameTable(
                name: "Categories",
                schema: "inventory",
                newName: "Categories",
                newSchema: "Material");

            migrationBuilder.RenameTable(
                name: "Address",
                schema: "sales",
                newName: "Address",
                newSchema: "Customer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "sales");

            migrationBuilder.EnsureSchema(
                name: "inventory");

            migrationBuilder.EnsureSchema(
                name: "labs");

            migrationBuilder.RenameTable(
                name: "Units",
                schema: "Material",
                newName: "Units",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "Suppliers",
                schema: "Material",
                newName: "Suppliers",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "SupplierContacts",
                schema: "Material",
                newName: "SupplierContacts",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "SupplierAddresses",
                schema: "Material",
                newName: "SupplierAddresses",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "SampleRequests",
                schema: "SampleRequests",
                newName: "SampleRequests",
                newSchema: "labs");

            migrationBuilder.RenameTable(
                name: "SampleRequestImages",
                schema: "SampleRequests",
                newName: "SampleRequestImages",
                newSchema: "labs");

            migrationBuilder.RenameTable(
                name: "PurchaseOrderStatusHistory",
                schema: "Orders",
                newName: "PurchaseOrderStatusHistory",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "PurchaseOrdersSchedules",
                schema: "Others",
                newName: "PurchaseOrdersSchedules",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "PurchaseOrders",
                schema: "Orders",
                newName: "PurchaseOrders",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "PurchaseOrderDetails",
                schema: "Orders",
                newName: "PurchaseOrderDetails",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "Products",
                schema: "SampleRequests",
                newName: "Products",
                newSchema: "labs");

            migrationBuilder.RenameTable(
                name: "ProductChangedHistory",
                schema: "SampleRequests",
                newName: "ProductChangedHistory",
                newSchema: "labs");

            migrationBuilder.RenameTable(
                name: "PriceHistory",
                schema: "Material",
                newName: "PriceHistory",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "Materials_Suppliers",
                schema: "Material",
                newName: "Materials_Suppliers",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "Materials",
                schema: "Material",
                newName: "Materials",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "FormulaStatusLog",
                schema: "SampleRequests",
                newName: "FormulaStatusLog",
                newSchema: "labs");

            migrationBuilder.RenameTable(
                name: "Formulas",
                schema: "SampleRequests",
                newName: "Formulas",
                newSchema: "labs");

            migrationBuilder.RenameTable(
                name: "FormulaMaterials",
                schema: "SampleRequests",
                newName: "FormulaMaterials",
                newSchema: "labs");

            migrationBuilder.RenameTable(
                name: "DetailCustomerTransfer",
                schema: "Customer",
                newName: "DetailCustomerTransfer",
                newSchema: "sales");

            migrationBuilder.RenameTable(
                name: "CustomerTransferLog",
                schema: "Customer",
                newName: "CustomerTransferLog",
                newSchema: "sales");

            migrationBuilder.RenameTable(
                name: "CustomerAssignment",
                schema: "Customer",
                newName: "CustomerAssignment",
                newSchema: "sales");

            migrationBuilder.RenameTable(
                name: "Customer",
                schema: "Customer",
                newName: "Customer",
                newSchema: "sales");

            migrationBuilder.RenameTable(
                name: "Contacts",
                schema: "Customer",
                newName: "Contacts",
                newSchema: "sales");

            migrationBuilder.RenameTable(
                name: "Categories",
                schema: "Material",
                newName: "Categories",
                newSchema: "inventory");

            migrationBuilder.RenameTable(
                name: "Address",
                schema: "Customer",
                newName: "Address",
                newSchema: "sales");
        }
    }
}
