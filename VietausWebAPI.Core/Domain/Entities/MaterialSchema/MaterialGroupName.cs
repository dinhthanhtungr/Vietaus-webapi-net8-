using DocumentFormat.OpenXml.Drawing.Diagrams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.Domain.Entities.MaterialSchema
{
    public class MaterialGroupName
    {
        public Guid MaterialGroupNameId { get; set; }
        public Guid MaterialId { get; set; }
        public string MaterialGroupNameText { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public virtual Material Material { get; set; } = null!;
    }
}
