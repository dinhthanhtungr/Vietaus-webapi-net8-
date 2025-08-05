using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.HR.DTOs
{
    public class AccountDTOs
    {
        public Guid Id { get; set; }
        public string? personName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
    }
}
