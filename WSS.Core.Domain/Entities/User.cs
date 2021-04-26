using System;
using Microsoft.AspNetCore.Identity;
namespace WSS.Core.Domain.Entities
{
    public class User:IdentityUser
    {
        public string FullName { get; set; }
        public DateTime? BirthDay { set; get; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        
    }
}
