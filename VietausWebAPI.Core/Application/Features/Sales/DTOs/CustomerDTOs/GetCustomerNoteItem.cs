using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.CustomerEnum;

namespace VietausWebAPI.Core.Application.Features.Sales.DTOs.CustomerDTOs
{
    public class GetCustomerNoteItem
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public Guid AuthorEmployeeId { get; set; }
        public string? AuthorName { get; set; }

        public Guid AuthorGroupId { get; set; }
        public string? AuthorGroupName { get; set; }

        public NoteVisibility Visibility { get; set; } = NoteVisibility.Group;
        public bool IsApprovedShare { get; set; }
    }
}
