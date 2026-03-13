using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Application.Features.DevandqaFeatures.DTOs.QCInputByQCFeatures;

namespace VietausWebAPI.Core.Application.Features.DevandqaFeatures.Helpers.QCInputByQCFeatures
{
    public interface IExportQCInputByQCExcel
    {
        byte[] ExportExcel(List<QCInputByQCExportRow> rows);
    }
}
