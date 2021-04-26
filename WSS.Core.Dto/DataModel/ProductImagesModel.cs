﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WSS.Core.Dto.DataModel
{
  public class ProductImagesModel
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int ProductId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
