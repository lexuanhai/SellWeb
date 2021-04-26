using System;
using System.Collections.Generic;
using System.Text;
using WSS.Core.Domain.Entities;

namespace WSS.Core.Repository.Repositories
{
    public interface IProductQuantityRepository : IRepository<ProductQuantity>
    {

    }
    public class ProductQuantityRepository : Repository<ProductQuantity>, IProductQuantityRepository
    {
        private readonly AppDataBaseContext _context;
        public ProductQuantityRepository(AppDataBaseContext context) : base(context)
        {
            _context = context;
        }
    }
}
