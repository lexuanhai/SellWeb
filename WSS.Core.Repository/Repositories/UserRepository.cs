using System;
using System.Collections.Generic;
using System.Text;
using WSS.Core.Domain.Entities;

namespace WSS.Core.Repository.Repositories
{
    public interface IUserRepository : IRepository<User>
    {

    }
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly AppDataBaseContext _context;
        public UserRepository(AppDataBaseContext context) : base(context)
        {
            _context = context;
        }
    }
}
