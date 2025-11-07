using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VietausWebAPI.Core.Domain.Enums.Logs;

namespace VietausWebAPI.Core.Application.Features.TimelineFeature.DTOs.EventLogDtos
{
    public class EventLogModels
    {
        public Guid employeeId { get; set; }
        public Guid sourceId { get; set; }
        public string? sourceCode { get; set; }
        public string? status { get; set; }
        public EventType eventType { get; set; }
        public string? note { get; set; }
    }
}
