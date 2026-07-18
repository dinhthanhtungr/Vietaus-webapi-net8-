using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Entities.HrSchema;

namespace VietausWebAPI.Core.Domain.Entities.InternalMailSchema
{
    /// <summary>
    /// Trang thai doc chi tiet theo tung message va tung nhan vien; khac voi LastReadAt la moc doc nhanh theo conversation.
    /// </summary>
    public class InternalMessageReadState
    {
        public Guid InternalMessageId { get; set; }
        public InternalMessage Message { get; set; } = default!;

        public Guid EmployeeId { get; set; }
        public Employee Employee { get; set; } = default!;

        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
    }
}
