using System;
using System.Collections.Generic;
using System.Text;

namespace WSS.Core.Dto.DataModel
{
   public class ProductQuantityModel
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public int? ColorId { get; set; }
        public int? SizeId { get; set; }
        public int TotalImport { get; set; }
        public int TotalSell { get; set; }
        public int TotalStock { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
