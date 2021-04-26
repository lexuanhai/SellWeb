using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WSS.Core.Common;

namespace WSS.Core.Domain.Entities
{
    public class Bill : SameEntities
    {
        public BillStatus BillStatus { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        [ForeignKey("UserId")]
        public Guid UserId { get; set; }            
        public virtual AppUser Users { get; set; }
        public virtual ICollection<BillDetail> BillDetails { set; get; }
    }
}
