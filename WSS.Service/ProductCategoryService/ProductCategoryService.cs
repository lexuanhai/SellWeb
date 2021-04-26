using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSS.Core.Common.Infrastructure;
using WSS.Core.Domain.Entities;
using WSS.Core.Dto.AutoMapper;
using WSS.Core.Dto.DataModel;
using WSS.Core.Dto.SearchModel.FunctionSearch;
using WSS.Core.Common;
using WSS.Core.Dto.SearchModel.RoleSearch;
using WSS.Core.Dto.SearchModel.UserSearch;
using WSS.Core.Repository;
using WSS.Core.Repository.Repositories;
using WSS.Core.Dto.SearchModel.ProductSearch;

namespace WSS.Service.ProductCategoryService
{
    public interface IProductCategoryService : IDisposable
    {
        bool SaveEntity(ProductCategoryModel productCategoryModel);
        ProductCategoryModel GetProductCategoryById(int id);
        PageResult<ProductCategoryModel> GetSearchPaging(ProductSearch search);
        List<ProductCategoryModel> GetParentProductCategory();
        List<ProductCategoryModel> GetAllCategory();
        void Save();
        bool Delete(int id);
    }
    public class ProductCategoryService : IProductCategoryService
    {
        IProductCategoryRepository _productCategoryRepository;
        IUnitOfWork _unitOfWork;
        public ProductCategoryService(
            IProductCategoryRepository productCategoryRepository,
            IUnitOfWork unitOfWork)
        {
            _productCategoryRepository = productCategoryRepository;
            _unitOfWork = unitOfWork;
        }
        public ProductCategoryModel GetProductCategoryById(int id)
        {
            var productCategory = new ProductCategoryModel();
            if (id > 0)
            {
                productCategory = _productCategoryRepository.Queryable()
                  .Where(r => r.Id == id)
                  .AsNoTracking()
                  .FirstOrDefault()
                  .ToModel();
            }
            return productCategory;
        }
        public bool SaveEntity(ProductCategoryModel productCategoryModel)
        {

            try
            {
                if (productCategoryModel.Id > 0)
                {
                    var productCategory = GetProductCategoryById(productCategoryModel.Id).ToProductCategory();
                    productCategory.Id = productCategoryModel.Id;
                    productCategory.Name = productCategoryModel.Name;
                    productCategory.UpdatedDate = DateTime.Now;
                    productCategory.ParentId = productCategoryModel.ParentId;
                    productCategory.Status = productCategoryModel.Status;
                    _productCategoryRepository.Update(productCategory);

                    return true;
                }
                else
                {
                    var productCategory = new ProductCategory();
                    productCategory.Id = productCategoryModel.Id;
                    productCategory.Name = productCategoryModel.Name;
                    productCategory.ParentId = productCategoryModel.ParentId;
                    productCategory.Status = productCategoryModel.Status;
                    productCategory.CreatedDate = DateTime.Now;
                    _productCategoryRepository.Add(productCategory);

                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public PageResult<ProductCategoryModel> GetSearchPaging(ProductSearch search)
        {
            var query = _productCategoryRepository.FindAll();

            //if (search.Id > 0)
            //{
            //    query = query.Where(r => r.Id == search.Id);
            //}

            //if (!string.IsNullOrEmpty(search.Name))
            //{
            //    query = query.Where(r => r.Name.Contains(search.Name));
            //}
            //if (!string.IsNullOrEmpty(search.Description))
            //{
            //    query = query.Where(r => r.Description.Contains(search.Description));
            //}
            
            var Total = query.Count();

            var data = query               
                .OrderBy(f => f.CreatedDate)
                .Skip((search.PageIndex - 1) * search.PageSize)
                .Take(search.PageSize)
                .Select(f => f.ToModel())
                .ToList();

            foreach(var item in data)
            {
                if (item.ParentId != null && item.ParentId > 0)
                {
                    var nameParent = data.Where(f => f.Id == item.ParentId).FirstOrDefault().Name;
                    item.NameParent = nameParent;
                }
            }

            var result = new PageResult<ProductCategoryModel>()
            {
                Results = data,
                PageIndex = search.PageIndex,
                PageSize = search.PageSize,
                Total = Total,
            };
            return result;
        }

        public bool Delete(int id)
        {
            if (id > 0)
            {
                var role = _productCategoryRepository.FindAll().Where(r => r.Id == id).FirstOrDefault();
                _productCategoryRepository.Remove(role);
                return true;
            }
            return false;
        }
        public List<ProductCategoryModel> GetParentProductCategory()
        {
            var lstParentProductCateogry = _productCategoryRepository.FindAll().Where(f => f.ParentId == null || f.ParentId == 0).Select(f=>f.ToModel()).ToList();
            return lstParentProductCateogry;
        }

        public List<ProductCategoryModel> GetAllCategory()
        {
            var data = _productCategoryRepository.FindAll().Select(p => p.ToModel()).ToList();
            return data;
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
