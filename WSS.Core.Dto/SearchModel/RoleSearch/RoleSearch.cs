using System;
using System.Collections.Generic;
using System.Text;

namespace WSS.Core.Dto.SearchModel.RoleSearch
{
   public class RoleSearch:Search
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
