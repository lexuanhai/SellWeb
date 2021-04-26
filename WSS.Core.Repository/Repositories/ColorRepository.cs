using System;
using System.Collections.Generic;
using System.Text;
using WSS.Core.Domain.Entities;

namespace WSS.Core.Repository.Repositories
{
    public interface IColorRepository : IRepository<Color>
    {

    }
    public class ColorRepository : Repository<Color>, IColorRepository
    {
        private readonly AppDataBaseContext _context;
        public ColorRepository(AppDataBaseContext context) : base(context)
        {
            _context = context;
        }
    }
}
