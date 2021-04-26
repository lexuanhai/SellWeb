using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSS.Core.Common.Infrastructure;
using WSS.Core.Domain.Entities;
using WSS.Core.Dto.AutoMapper;
using WSS.Core.Dto.DataModel;
using WSS.Core.Dto.SearchModel.RoleSearch;
using WSS.Core.Dto.SearchModel.UserSearch;
using WSS.Core.Repository;
using WSS.Core.Repository.Repositories;

namespace WSS.Service.ProductQuantityService
{
    public interface IProductQuantityService : IDisposable
    {
        List<ProductQuantityModel> GetQuantityByProductId(int productId);
        bool SaveEntity(List<ProductQuantityModel> quantitys);
        List<ProductQuantityModel> ProductQuantitiesByProductId(int productId);
        void Save();
        //bool Delete(int id);
    }
    public class ProductQuantityService : IProductQuantityService
    {
        IProductQuantityRepository _productQuantityRepository;
        IUnitOfWork _unitOfWork;
        public ProductQuantityService(
              IProductQuantityRepository productQuantityRepository,
            IUnitOfWork unitOfWork)
        {
            _productQuantityRepository = productQuantityRepository;
            _unitOfWork = unitOfWork;
        }
        public void Save()
        {
            _unitOfWork.Commit();
        }
        public List<ProductQuantityModel> GetQuantityByProductId(int productId)
        {
            var model = new List<ProductQuantityModel>();
            if (productId > 0)
            {
                var productQuantity = _productQuantityRepository.FindAll().Where(q => q.ProductId == productId).Select(q => q.ToModel()).ToList();
                if (productQuantity != null && productQuantity.Count > 0)
                {
                    model = productQuantity;
                }
            }
            return model;
        }
        public List<ProductQuantityModel> ProductQuantitiesByProductId(int productId)
        {
            var productQuantities = new List<ProductQuantityModel>();
            if (productId > 0)            
                productQuantities = _productQuantityRepository.FindAll().Where(pq => pq.ProductId == productId).Select(pq => pq.ToModel()).ToList();
            
            return productQuantities;
        }

        public bool SaveEntity(List<ProductQuantityModel> quantitys)
       {
            if (quantitys != null && quantitys.Count > 0)
            {
                var productId = quantitys[0].ProductId;

                if (productId > 0)
                {
                    var oldQuantity = _productQuantityRepository.FindAll().Where(q => q.ProductId == productId).ToList();
                    if (oldQuantity != null && oldQuantity.Count > 0)
                    {
                        _productQuantityRepository.RemoveMultiple(oldQuantity);
                    }
                   
                    foreach (var item in quantitys)
                    {
                        _productQuantityRepository.Add(item.ToProductQuantity());
                    }

                    Save();
                    return true;
                  
                }
            }
         
            return false;
       }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
