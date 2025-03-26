using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietausWebAPI.Core.DTO.PostDTO
{
    public class MaterialSuppliersDTO
    {
        public string? supplierID {  get; set; }
        public string? supplierName { get; set; }
        public string? phone { get; set; }
        public string? address { get; set; }
        public string? note { get; set; }
    }
}
