using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Application.Shared.Models.Datas
{
    public class HasData
    {
        public bool HasHistory { get; set; }
        public int Count { get; set; }
        public DateTime? LatestCreatedAt { get; set; }
    }
}