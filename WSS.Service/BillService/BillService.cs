using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WSS.Core.Common.Infrastructure;
using WSS.Core.Domain.Entities;
using WSS.Core.Dto.AutoMapper;
using WSS.Core.Dto.DataModel;
using WSS.Core.Dto.SearchModel.ProductSearch;
using WSS.Core.Repository;
using WSS.Core.Repository.Repositories;

namespace WSS.Service.BillService
{
    public interface IBillService : IDisposable
    {
        void Create(BillModel billVm);
        void Save();
    }
    public class BillService : IBillService
    {
        IProductRepository _productRepository;
        IBillRepository _billRepository;
        IUnitOfWork _unitOfWork;
        public BillService(
              IBillRepository billRepository,
              IProductRepository productRepository,
              IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _billRepository = billRepository;
            _unitOfWork = unitOfWork;
        }     
       
        public void Save()
        {
            _unitOfWork.Commit();
        }
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        public void Create(BillModel billVm)
        {
            var order = billVm.ToBill();
            var orderDetails = billVm.BillDetails;
            foreach (var detail in orderDetails)
            {
                var product = _productRepository.Queryable().Where(p=>p.Id == detail.ProductId).FirstOrDefault();
                detail.Price = product.Price;
            }
            order.BillDetails = orderDetails;
            _billRepository.Add(order);
        }
    }
}
