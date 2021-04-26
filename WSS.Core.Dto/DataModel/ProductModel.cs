using System;
using System.Collections.Generic;
using System.Text;
using WSS.Core.Common;

namespace WSS.Core.Dto.DataModel
{
   public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CategoryId { get; set; }
        public string Head { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public decimal? PromotionalPrice { get; set; }
        public int? Views { get; set; }
        public Status Status { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool? IsDeteled { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public ProductCategoryModel ProductCategories { get; set; }
        public ProductCategoryModel ProductCategoryParent { get; set; }
        public List<ProductQuantityModel> ProductQuantities { get; set; }
        public List<ProductImagesModel> ProductImages { get; set; }
    }
}
