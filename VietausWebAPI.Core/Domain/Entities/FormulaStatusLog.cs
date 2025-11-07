//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace VietausWebAPI.Core.Domain.Entities
//{
//    public class FormulaStatusLog
//    {
//        public Guid LogId { get; set; }
//        public Guid FormulaId { get; set; }

//        public string? OldStatus { get; set; }
//        public string? NewStatus { get; set; }

//        // Tên người thực hiện (snapshot)
//        public string? CreateNameSnapShot { get; set; }

//        public Guid? CreatedBy { get; set; }
//        public DateTime? CreatedDate { get; set; }

//        // Navigations
//        public virtual Formula Formula { get; set; } = null!;
//        public virtual Employee? CreatedByNavigation { get; set; }
//    }
//}
