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

namespace WSS.Service.SizeService
{
    public interface ISizeService : IDisposable
    {
        bool SaveEntity(SizeModel sizeModel);
        SizeModel GetSizeById(int id);
        List<SizeModel> GetAll();
        PageResult<SizeModel> GetSearchPaging(RoleSearch search);
        void Save();
        bool Delete(int id);
    }
    public class SizeService : ISizeService
    {
        ISizeRepository _sizeRepository;
        IUnitOfWork _unitOfWork;
        public SizeService(
            ISizeRepository sizeRepository,
        IUnitOfWork unitOfWork)
        {
            _sizeRepository = sizeRepository;
            _unitOfWork = unitOfWork;
        }
        public SizeModel GetSizeById(int id)
        {
            var size = new SizeModel();
            if (id > 0)
            {
                size = _sizeRepository.Queryable()
                  .Where(r => r.Id == id)
                  .AsNoTracking()
                  .FirstOrDefault()
                  .ToModel();
            }
            return size;
        }
        public bool SaveEntity(SizeModel sizeModel)
        {
            try
            {
                if (sizeModel.Id > 0)
                {
                    var size = GetSizeById(sizeModel.Id).ToColor();
                    size.Id = sizeModel.Id;
                    size.Name = sizeModel.Name;
                    size.UpdatedDate = DateTime.Now;
                    _sizeRepository.Update(size);

                    return true;
                }
                else
                {
                    var size = new Size();
                    size.Id = sizeModel.Id;
                    size.Name = sizeModel.Name;
                    size.CreatedDate = DateTime.Now;
                    _sizeRepository.Add(size);

                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public PageResult<SizeModel> GetSearchPaging(RoleSearch search)
        {
            var query = _sizeRepository.FindAll();

            if (search.Id > 0)
            {
                query = query.Where(r => r.Id == search.Id);
            }

            if (!string.IsNullOrEmpty(search.Name))
            {
                query = query.Where(r => r.Name.Contains(search.Name));
            }
            //if (!string.IsNullOrEmpty(search.Description))
            //{
            //    query = query.Where(r => r.Description.Contains(search.Description));
            //}
            var Total = query.Count();

            var data = query
                .OrderByDescending(U => U.CreatedDate)
                .Skip((search.PageIndex - 1) * search.PageSize)
                .Take(search.PageSize)
                .Select(u => u.ToModel())
                .ToList();

            var result = new PageResult<SizeModel>()
            {
                Results = data,
                PageIndex = search.PageIndex,
                PageSize = search.PageSize,
                Total = Total,
            };
            return result;
        }

        //public List<PermissionModel> GetListFunctionWithRole(int roleId)
        //{
        //    var functions = _functionRepository.FindAll();
        //    var permissions = _permissionRepository.FindAll();

        //    var query = from f in functions
        //                join p in permissions on f.Id equals p.FunctionId into fp
        //                from p in fp.DefaultIfEmpty()
        //                where p != null && p.RoleId == roleId
        //                select new PermissionModel()
        //                {
        //                    RoleId = roleId,
        //                    FunctionId = f.Id,
        //                    CanCreate = p != null ? p.CanCreate : false,
        //                    CanDelete = p != null ? p.CanDelete : false,
        //                    CanRead = p != null ? p.CanRead : false,
        //                    CanUpdate = p != null ? p.CanUpdate : false
        //                };
        //    return query.ToList();
        //}

        public List<SizeModel> GetAll()
        {
            var data = _sizeRepository.FindAll().Select(r => r.ToModel()).ToList();
            if (data != null && data.Count > 0)
            {
                return data;
            }
            else
            {
                return new List<SizeModel>();
            }
        }

        //public void SavePermission(List<PermissionModel> permissionVms, int roleId)
        //{
        //    var permissions = permissionVms.Select(p => p.ToPermission());
        //    var oldPermission = _permissionRepository.FindAll().Where(x => x.RoleId == roleId).ToList();
        //    if (oldPermission.Count > 0)
        //    {
        //        _permissionRepository.RemoveMultiple(oldPermission);
        //    }
        //    foreach (var permission in permissions)
        //    {
        //        _permissionRepository.Add(permission);
        //    }
        //    _unitOfWork.Commit();
        //}

        public bool Delete(int id)
        {
            if (id > 0)
            {
                var role = _sizeRepository.FindAll().Where(r => r.Id == id).FirstOrDefault();
                _sizeRepository.Remove(role);
                return true;
            }
            return false;
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
