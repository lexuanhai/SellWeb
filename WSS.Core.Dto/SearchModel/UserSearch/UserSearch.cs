using System;
using System.Collections.Generic;
using System.Text;

namespace WSS.Core.Dto.SearchModel.UserSearch
{
   public class UserSearch : Search
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? Birthday { get; set; }
        public string Address { get; set; }
    }
}
