using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using WSS.Core.Common;

namespace WSS.Core.Domain.Entities
{
   public class Product:SameEntities
    {
        [MaxLength(500)]
        public string Name { get; set; }
        
        [MaxLength(500)]
        public string Head { get; set; }
        [MaxLength(500)]
        public string Description { get; set; }
        
        public decimal? Price { get; set; }
        public decimal? PromotionalPrice { get; set; }
        public int? Views { get; set; }
        public Status? Status { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool? IsDeteled { get; set; }

        [ForeignKey("CategoryId")]
        public int? CategoryId { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
    }
}
