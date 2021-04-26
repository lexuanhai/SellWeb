using System;
using System.Collections.Generic;
using System.Text;
using WSS.Core.Domain.Entities;

namespace WSS.Core.Repository.Repositories
{
    public interface IProductCategoryRepository : IRepository<ProductCategory>
    {

    }
    public class ProductCategoryRepository : Repository<ProductCategory>, IProductCategoryRepository
    {
        private readonly AppDataBaseContext _context;
        public ProductCategoryRepository(AppDataBaseContext context) : base(context)
        {
            _context = context;
        }
    }
}
