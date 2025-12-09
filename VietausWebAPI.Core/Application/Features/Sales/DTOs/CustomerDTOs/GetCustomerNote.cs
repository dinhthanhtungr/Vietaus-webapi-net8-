using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs
{
    public class GetCustomerNote
    {
        public Guid? NoteId { get; set; }                 // có thì sửa, không thì thêm
        public string Content { get; set; } = string.Empty;
    }
}
