using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WSS.Core.Common;

namespace WSS.Core.Domain.Entities
{
    public class BillDetail : SameEntities
    {
        public int? ProductId { get; set; }     
        public int? SizeId { get; set; }
        public int? ColorId { get; set; }
        public int? BillId { get; set; }
        public Decimal? Price { get; set; }
        public int? Quantity { get; set; }
        [ForeignKey("SizeId")]
        public virtual Size Sizes { get; set; }
        [ForeignKey("ColorId")]
        public virtual Color Colors { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Products { get; set; }
        [ForeignKey("BillId")]
        public virtual Bill Bills { get; set; }

    }
}
