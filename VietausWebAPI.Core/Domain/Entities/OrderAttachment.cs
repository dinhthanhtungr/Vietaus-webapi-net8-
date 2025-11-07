//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using VietausWebAPI.Core.Domain.Enums.Attachment;

//namespace VietausWebAPI.Core.Domain.Entities
//{
//    public class OrderAttachment
//    {
//        public Guid AttachmentId { get; set; }
//        public Guid MerchandiseOrderId { get; set; }              // FK

//        public AttachmentSlot Slot { get; set; }   // <--- thêm slot

//        public string FileName { get; set; } = null!;
//        public long SizeBytes { get; set; }
//        public string StoragePath { get; set; } = null!; // đường dẫn/URL blob
//        public DateTime CreateDate { get; set; }
//        public Guid? CreateBy { get; set; }

//        public virtual MerchandiseOrder? MerchandiseOrder { get; set; }
//        public virtual Employee? CreatedByNavigation { get; set; }
//    }
//}
