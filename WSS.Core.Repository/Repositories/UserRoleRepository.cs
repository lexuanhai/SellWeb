using System;
using System.Collections.Generic;
using System.Text;
using WSS.Core.Domain.Entities;

namespace WSS.Core.Repository.Repositories
{
    public interface IUserRoleRepository : IRepository<UserRole>
    {

    }
    public class UserRoleRepository : Repository<UserRole>, IUserRoleRepository
    {
        private readonly AppDataBaseContext _context;
        public UserRoleRepository(AppDataBaseContext context) : base(context)
        {
            _context = context;
        }
    }
}
