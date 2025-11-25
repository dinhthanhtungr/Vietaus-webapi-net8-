using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.FormulaFeatures;
//using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.QAQCFeature;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts;

namespace VietausWebAPI.Infrastructure.DataUnitOfWork
{
    public sealed partial class UnitOfWork
    {
        //public IProductInspectionRepository ProductInspectionRepository { get; }
        //public IQCDetailRepository IQCDetailRepository { get; }

        //public IProductStandardRepository ProductStandardRepository { get; }
        //public IProductTestRepository ProductTestRepository { get; }
        public IManufacturingFormulaVersionRepository ManufacturingFormulaVersionRepository { get; }
        public IFormulaRepository FormulaRepository { get; }
        public IFormulaMaterialRepository FormulaMaterialRepository { get; }

        public IProductRepository ProductRepository { get; }
        public ISampleRequestRepository SampleRequestRepository { get; }
    }
}
