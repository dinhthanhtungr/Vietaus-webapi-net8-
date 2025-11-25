using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.FormulaFeatures;
//using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.QAQCFeature;
using VietausWebAPI.Core.Application.Features.Labs.RepositoriesContracts.SampleRequestFeature;
using VietausWebAPI.Core.Application.Features.Manufacturing.RepositoriesContracts;

namespace VietausWebAPI.Core.Repositories_Contracts
{
    public partial interface IUnitOfWork
    {
        //// QA/QC
        //IProductInspectionRepository ProductInspectionRepository { get; }
        //IQCDetailRepository IQCDetailRepository { get; }

        //// Formula
        //IProductStandardRepository ProductStandardRepository { get; }
        //IProductTestRepository ProductTestRepository { get; }
        IManufacturingFormulaVersionRepository ManufacturingFormulaVersionRepository { get; }
        IFormulaRepository FormulaRepository { get; }
        IFormulaMaterialRepository FormulaMaterialRepository { get; }

        // Product & Sample
        IProductRepository ProductRepository { get; }
        ISampleRequestRepository SampleRequestRepository { get; }
    }
}
