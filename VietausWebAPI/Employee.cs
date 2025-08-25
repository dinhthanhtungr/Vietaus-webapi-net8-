using System;
using System.Collections.Generic;

namespace VietausWebAPI.WebAPI;

public partial class Employee
{
    public Guid EmployeeId { get; set; }

    public string ExternalId { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string? Gender { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public string? Identifier { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Address { get; set; }

    public Guid? PartId { get; set; }

    public Guid? LevelId { get; set; }

    public DateTime? DateHired { get; set; }

    public string? Status { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual ICollection<ApprovalHistory> ApprovalHistories { get; set; } = new List<ApprovalHistory>();

    public virtual ICollection<Branch> BranchCreatedByNavigations { get; set; } = new List<Branch>();

    public virtual ICollection<Branch> BranchUpdatedByNavigations { get; set; } = new List<Branch>();

    public virtual ICollection<Company> CompanyCreatedByNavigations { get; set; } = new List<Company>();

    public virtual ICollection<Company> CompanyUpdatedByNavigations { get; set; } = new List<Company>();

    public virtual ICollection<CustomerAssignment> CustomerAssignmentCreatedByNavigations { get; set; } = new List<CustomerAssignment>();

    public virtual ICollection<CustomerAssignment> CustomerAssignmentEmployees { get; set; } = new List<CustomerAssignment>();

    public virtual ICollection<CustomerAssignment> CustomerAssignmentUpdatedByNavigations { get; set; } = new List<CustomerAssignment>();

    public virtual ICollection<Customer> CustomerCreatedByNavigations { get; set; } = new List<Customer>();

    public virtual ICollection<CustomerTransferLog> CustomerTransferLogCreatedByNavigations { get; set; } = new List<CustomerTransferLog>();

    public virtual ICollection<CustomerTransferLog> CustomerTransferLogFromEmployees { get; set; } = new List<CustomerTransferLog>();

    public virtual ICollection<CustomerTransferLog> CustomerTransferLogToEmployees { get; set; } = new List<CustomerTransferLog>();

    public virtual ICollection<Customer> CustomerUpdatedByNavigations { get; set; } = new List<Customer>();

    public virtual ICollection<Formula> FormulaCreatedByNavigations { get; set; } = new List<Formula>();

    public virtual ICollection<Formula> FormulaSentByNavigations { get; set; } = new List<Formula>();

    public virtual ICollection<Formula> FormulaUpdatedByNavigations { get; set; } = new List<Formula>();

    public virtual ICollection<Formula> FormulaVerifiedByNavigations { get; set; } = new List<Formula>();

    public virtual ICollection<Group> GroupCreatedByNavigations { get; set; } = new List<Group>();

    public virtual ICollection<Group> GroupUpdatedByNavigations { get; set; } = new List<Group>();

    public virtual ICollection<Material> MaterialCreatedByNavigations { get; set; } = new List<Material>();

    public virtual ICollection<Material> MaterialUpdatedByNavigations { get; set; } = new List<Material>();

    public virtual ICollection<MaterialsSupplier> MaterialsSuppliers { get; set; } = new List<MaterialsSupplier>();

    public virtual ICollection<MemberInGroup> MemberInGroups { get; set; } = new List<MemberInGroup>();

    public virtual ICollection<MerchandiseOrder> MerchandiseOrderCreatedByNavigations { get; set; } = new List<MerchandiseOrder>();

    public virtual ICollection<MerchandiseOrder> MerchandiseOrderManagerBies { get; set; } = new List<MerchandiseOrder>();

    public virtual ICollection<MerchandiseOrder> MerchandiseOrderUpdatedByNavigations { get; set; } = new List<MerchandiseOrder>();

    public virtual Part? Part { get; set; }

    public virtual ICollection<PriceHistory1> PriceHistory1CreatedByNavigations { get; set; } = new List<PriceHistory1>();

    public virtual ICollection<PriceHistory1> PriceHistory1UpdatedByNavigations { get; set; } = new List<PriceHistory1>();

    public virtual ICollection<PriceHistory> PriceHistoryCreatedByNavigations { get; set; } = new List<PriceHistory>();

    public virtual ICollection<PriceHistory> PriceHistoryUpdatedByNavigations { get; set; } = new List<PriceHistory>();

    public virtual ICollection<ProductChangedHistory> ProductChangedHistories { get; set; } = new List<ProductChangedHistory>();

    public virtual ICollection<Product> ProductCreatedByNavigations { get; set; } = new List<Product>();

    public virtual ICollection<ProductStandard> ProductStandardCreatedByNavigations { get; set; } = new List<ProductStandard>();

    public virtual ICollection<ProductStandard> ProductStandardUpdatedByNavigations { get; set; } = new List<ProductStandard>();

    public virtual ICollection<Product> ProductUpdatedByNavigations { get; set; } = new List<Product>();

    public virtual ICollection<PurchaseOrder> PurchaseOrderCreatedByNavigations { get; set; } = new List<PurchaseOrder>();

    public virtual ICollection<PurchaseOrderStatusHistory> PurchaseOrderStatusHistories { get; set; } = new List<PurchaseOrderStatusHistory>();

    public virtual ICollection<PurchaseOrder> PurchaseOrderUpdatedByNavigations { get; set; } = new List<PurchaseOrder>();

    public virtual ICollection<SampleRequest> SampleRequestCreatedByNavigations { get; set; } = new List<SampleRequest>();

    public virtual ICollection<SampleRequest> SampleRequestManagerByNavigations { get; set; } = new List<SampleRequest>();

    public virtual ICollection<SampleRequest> SampleRequestUpdatedByNavigations { get; set; } = new List<SampleRequest>();

    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();

    public virtual ICollection<SupplyRequest> SupplyRequestCreatedByNavigations { get; set; } = new List<SupplyRequest>();

    public virtual ICollection<SupplyRequest> SupplyRequestUpdatedByNavigations { get; set; } = new List<SupplyRequest>();

    public virtual ICollection<Unit> Units { get; set; } = new List<Unit>();
}
