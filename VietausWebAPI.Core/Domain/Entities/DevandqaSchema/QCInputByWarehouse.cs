//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using VietausWebAPI.Core.Domain.Entities.CustomerSchema;
//using VietausWebAPI.Core.Domain.Entities.MaterialSchema;

//namespace VietausWebAPI.Core.Domain.Entities.DevandqaSchema
//{
//    public class QCInputByWarehouse
//    {
//        public Guid QCInputByWarehouseId { get; set; }
//        public Guid MaterialId { get; set; }
//        public Guid QCInputByQCId { get; set; }

//        public string? CSNameSnapshot { get; set; }
//        public string? CSExternalIdSnapshot { get; set; }

//        public string? MaterialExternalIdSnapshot { get; set; }
//        public string? MaterialNameSnapshot { get; set; }

//        public string? LotNo { get; set; }

//        public DateTime CreatedDate { get; set; }
//        public Guid CreatedBy { get; set; }

//        public virtual Material? Material { get; set; }
//        public virtual QCInputByQC? QCInputByQC { get; set; }

//    }
//}
