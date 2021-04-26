using System;
using System.Collections.Generic;
using System.Text;
using WSS.Core.Domain.Entities;

namespace WSS.Core.Repository.Repositories
{
    public interface IFunctionRepository : IRepository<Function>
    {

    }
    public class FunctionRepository : Repository<Function>, IFunctionRepository
    {
        private readonly AppDataBaseContext _context;
        public FunctionRepository(AppDataBaseContext context) : base(context)
        {
            _context = context;
        }
    }
}
