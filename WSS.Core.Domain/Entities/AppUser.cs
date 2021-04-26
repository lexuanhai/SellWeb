using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace WSS.Core.Domain.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        //public AppUser() { }
        //public AppUser(Guid id, string fullName, string userName,
        //    string email, string phoneNumber, string avatar)
        //{
        //    Id = id;
        //    FullName = fullName;
        //    UserName = userName;
        //    Email = email;
        //    PhoneNumber = phoneNumber;
        //    Avatar = avatar;
        //}
        public string FullName { get; set; }
        public DateTime? BirthDay { set; get; }
        public string Password { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }            
    }
}
