using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WSS.Core.Domain.Entities
{
   public class ProductSize:SameEntities
    {
        public int? ProductId { get; set; }
        public int? SizeId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Products { get; set; }
        [ForeignKey("SizeId")]
        public virtual Size Sizes { get; set; }
    }
}
