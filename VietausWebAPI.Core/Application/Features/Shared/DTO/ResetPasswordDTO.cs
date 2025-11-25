using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Features.Shared.DTO
{
    public class ResetPasswordDTO
    {
        public string? userName { get; set; }
        public string? newPassword { get; set; }
    }
}
