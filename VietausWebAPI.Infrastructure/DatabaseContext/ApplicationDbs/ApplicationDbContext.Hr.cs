using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.HrSchema;

namespace VietausWebAPI.Infrastructure.DatabaseContext.ApplicationDbs
{
    public partial class ApplicationDbContext
    {
        public virtual DbSet<EmployeeProfile> EmployeeProfiles { get; set; } = default!;
        public virtual DbSet<EmployeeWorkProfile> EmployeeWorkProfiles { get; set; } = default!;
        public virtual DbSet<EmployeeContract> EmployeeContracts { get; set; } = default!;
        public virtual DbSet<EmployeeBankAccount> EmployeeBankAccounts { get; set; } = default!;
        public virtual DbSet<EmployeeInsuranceProfile> EmployeeInsuranceProfiles { get; set; } = default!;
        public virtual DbSet<EmployeeRelative> EmployeeRelatives { get; set; } = default!;
        public virtual DbSet<EmployeeDocument> EmployeeDocuments { get; set; } = default!;
        public virtual DbSet<JobTitle> JobTitles { get; set; } = default!;


        public virtual DbSet<PayrollPeriod> PayrollPeriods { get; set; } = default!;
        public virtual DbSet<SalaryComponentDefinition> SalaryComponentDefinitions { get; set; } = default!;
        public virtual DbSet<PayrollEmployeeRun> PayrollEmployeeRuns { get; set; } = default!;
        public virtual DbSet<PayrollEmployeeRunDetail> PayrollEmployeeRunDetails { get; set; } = default!;
        public virtual DbSet<EmployeeInsuranceContribution> EmployeeInsuranceContributions { get; set; } = default!;
        public virtual DbSet<EmployeeInsuranceBook> EmployeeInsuranceBooks { get; set; } = default!;
        public virtual DbSet<EmployeeInsuranceClaim> EmployeeInsuranceClaims { get; set; } = default!;




        public virtual DbSet<Employee> Employees { get; set; } = default!;
        public virtual DbSet<Part> Parts { get; set; } = default!;

    }
}
