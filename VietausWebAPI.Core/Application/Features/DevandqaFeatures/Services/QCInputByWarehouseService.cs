//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using VietausWebAPI.Core.Application.Features.DevandqaFeatures.DTOs.QCInputDTOs;
//using VietausWebAPI.Core.Application.Features.DevandqaFeatures.ServiceContracts;
//using VietausWebAPI.Core.Application.Features.Shared.Repositories_Contracts;
//using VietausWebAPI.Core.Application.Shared.Helper.JwtExport;
//using VietausWebAPI.Core.Application.Shared.Models.PageModels;
//using VietausWebAPI.Core.Domain.Entities.DevandqaSchema;

//namespace VietausWebAPI.Core.Application.Features.DevandqaFeatures.Services
//{
//    public class QCInputByWarehouseService : IQCInputByWarehouseService
//    {

//        private readonly IUnitOfWork _unitOfWork;
//        private readonly ICurrentUser _currentUser;

//        public QCInputByWarehouseService(IUnitOfWork unitOfWork, ICurrentUser currentUser)
//        {
//            _unitOfWork = unitOfWork;
//            _currentUser = currentUser;
//        }

//        // ======================================================================== Post ========================================================================

//        public async Task<OperationResult> AddAsync(PostQCInputByWarehouse qCInputByWarehouse, CancellationToken cancellationToken)
//        {
//            try
//            {
//                if (qCInputByWarehouse == null)
//                    return OperationResult.Fail("Dữ liệu trống.");

//                var companyId = _currentUser?.CompanyId ?? Guid.Empty;
//                var createdBy = _currentUser?.EmployeeId ?? Guid.Empty;
//                var now = DateTime.Now;

//                var qcInputByQC = new QCInputByWarehouse
//                {
//                    QCInputByQCId = Guid.CreateVersion7(),
//                    MaterialId = qCInputByWarehouse.MaterialId,
//                    CSNameSnapshot = qCInputByWarehouse.CSNameSnapshot,
//                    CSExternalIdSnapshot = qCInputByWarehouse.CSExternalIdSnapshot,
//                    MaterialExternalIdSnapshot = qCInputByWarehouse.MaterialExternalIdSnapshot,
//                    MaterialNameSnapshot = qCInputByWarehouse.MaterialNameSnapshot,
//                    LotNo = qCInputByWarehouse.LotNo
//                };

//                await _unitOfWork.QCInputByWarehouseRepository.AddAsync(qcInputByQC, cancellationToken);

//                await _unitOfWork.SaveChangesAsync();
//                return OperationResult.Ok("Thêm phiếu QC vào kho thành công.");
//            }
//            catch (Exception ex)
//            {
//                return OperationResult.Fail($"Lỗi khi thêm phiếu QC vào kho: {ex.Message}");
//            }
//        }
//    }
//}
