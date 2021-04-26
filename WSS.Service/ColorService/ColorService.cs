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

namespace WSS.Service.ColorService
{
    public interface IColorService : IDisposable
    {
        bool SaveEntity(ColorModel colorModel);
        ColorModel GetColorById(int id);
        List<ColorModel> GetAll();
        PageResult<ColorModel> GetSearchPaging(RoleSearch search);
        void Save();
        bool Delete(int id);
    }
    public class ColorService : IColorService
    {
        IColorRepository _colorRepository;
        IUnitOfWork _unitOfWork;
        public ColorService(
            IColorRepository colorRepository,
        IUnitOfWork unitOfWork)
        {
            _colorRepository = colorRepository;
            _unitOfWork = unitOfWork;
        }
        public ColorModel GetColorById(int id)
        {
            var role = new ColorModel();
            if (id > 0)
            {
                role = _colorRepository.Queryable()
                  .Where(r => r.Id == id)
                  .AsNoTracking()
                  .FirstOrDefault()
                  .ToModel();
            }
            return role;
        }
        public bool SaveEntity(ColorModel colorModel)
        {

            try
            {
                if (colorModel.Id > 0)
                {
                    var color = GetColorById(colorModel.Id).ToColor();
                    color.Id = colorModel.Id;
                    color.Name = colorModel.Name;
                    color.Code = colorModel.Code;
                    color.UpdatedDate = DateTime.Now;
                    _colorRepository.Update(color);

                    return true;
                }
                else
                {
                    var color = new Color();
                    color.Id = colorModel.Id;
                    color.Name = colorModel.Name;
                    color.Code = colorModel.Code;
                    color.CreatedDate = DateTime.Now;
                    _colorRepository.Add(color);

                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public PageResult<ColorModel> GetSearchPaging(RoleSearch search)
        {
            var query = _colorRepository.FindAll();

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

            var result = new PageResult<ColorModel>()
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

        public List<ColorModel> GetAll()
        {
            var data = _colorRepository.FindAll().Select(r => r.ToModel()).ToList();
            if (data != null && data.Count > 0)
            {
                return data;
            }
            else
            {
                return new List<ColorModel>();
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
                var role = _colorRepository.FindAll().Where(r => r.Id == id).FirstOrDefault();
                _colorRepository.Remove(role);
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
