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
using System.Threading.Tasks;

namespace WSS.Service.FunctionService
{
    public interface IFunctionService : IDisposable
    {
        bool SaveEntity(FunctionModel functionModel);
        FunctionModel GetFunctioneById(int id);
        PageResult<FunctionModel> GetSearchPaging(FunctionSearch search);
        List<FunctionModel> GetParentFunction();
        List<FunctionModel> GetAllByParentId(int id);
        List<FunctionModel> GetFunctionByRoleId(Guid? roleId);
        List<FunctionModel> GetAll();
        void Save();
        bool Delete(int id);
    }
    public class FunctionService : IFunctionService
    {
        IFunctionRepository _functionRepository;
        IPermissionRepository _permissionRepository;
        IUnitOfWork _unitOfWork;
        public FunctionService(
            IFunctionRepository functionRepository,
            IPermissionRepository permissionRepository,
            IUnitOfWork unitOfWork)
        {
            _functionRepository = functionRepository;
            _permissionRepository = permissionRepository;
            _unitOfWork = unitOfWork;
        }
        public FunctionModel GetFunctioneById(int id)
        {
            var function = new FunctionModel();
            if (id > 0)
            {
                function = _functionRepository.Queryable()
                  .Where(r => r.Id == id)
                  .AsNoTracking()
                  .FirstOrDefault()
                  .ToModel();
            }
            return function;
        }
        public bool SaveEntity(FunctionModel functionModel)
        {

            try
            {
                if (functionModel.Id > 0)
                {
                    var function = GetFunctioneById(functionModel.Id).ToFunction();
                    function.Id = functionModel.Id;
                    function.Name = functionModel.Name;
                    function.URL = functionModel.URL;
                    function.UpdatedDate = DateTime.Now;
                    function.ParentId = functionModel.ParentId != null && functionModel.ParentId > 0 ? functionModel.ParentId : null;
                    function.SortOrder = functionModel.SortOrder;
                    function.IconCss = functionModel.IconCss;
                    function.Status = functionModel.Status;
                    _functionRepository.Update(function);

                    return true;
                }
                else
                {
                    var function = new Function();
                    function.Id = functionModel.Id;
                    function.Name = functionModel.Name;
                    function.URL = functionModel.URL;
                    function.ParentId = functionModel.ParentId != null && functionModel.ParentId > 0 ? functionModel.ParentId : null;
                    function.SortOrder = functionModel.SortOrder;
                    function.IconCss = functionModel.IconCss;
                    function.Status = functionModel.Status;
                    function.CreatedDate = DateTime.Now;
                    _functionRepository.Add(function);

                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public PageResult<FunctionModel> GetSearchPaging(FunctionSearch search)
        {
            var query = _functionRepository.FindAll();

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
                .OrderByDescending(f => f.CreatedDate)
                .Skip((search.PageIndex - 1) * search.PageSize)
                .Take(search.PageSize)
                .Select(f => f.ToModel())
                .ToList();

            foreach (var item in data)
            {
                if (item.ParentId != null && item.ParentId > 0)
                {
                    var nameParent = data.Where(f => f.Id == item.ParentId).FirstOrDefault().Name;
                    item.NameParent = nameParent;
                }
            }

            var result = new PageResult<FunctionModel>()
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
                var role = _functionRepository.FindAll().Where(r => r.Id == id).FirstOrDefault();
                _functionRepository.Remove(role);
                return true;
            }
            return false;
        }
        public List<FunctionModel> GetParentFunction()
        {
            var lstParentFunction = _functionRepository.FindAll().Where(f => f.ParentId == null || f.ParentId == 0).Select(f => f.ToModel()).ToList();
            return lstParentFunction;
        }
        // Get Function theo parentId
        public List<FunctionModel> GetAllByParentId(int id)
        {
            var lstFunction = _functionRepository.FindAll().Where(f => f.ParentId == id && f.Status == Status.Active).Select(f => f.ToModel()).ToList();
            return lstFunction;
        }
        public List<FunctionModel> GetAll()
        {
            var lstFunction = _functionRepository.FindAll(f => f.Status == Status.Active).Select(f => f.ToModel()).ToList();
            var lstParentFunction = _functionRepository.FindAll(f => f.ParentId == null || f.ParentId == 0).Select(f => f.ToModel()).ToList();
            var lstFunctionSort = new List<FunctionModel>();
            foreach (var function in lstParentFunction)
            {
                lstFunctionSort.Add(function);

                foreach (var item in lstFunction)
                {
                    if (function.Id == item.ParentId)
                    {
                        lstFunctionSort.Add(item);
                    }
                }
            }
            return lstFunctionSort;
        }
        public List<FunctionModel> GetFunctionByRoleId(Guid? roleId)
        {        
            var functions = from tbl_Fuction in _functionRepository.FindAll()
                            join tbl_Permission in _permissionRepository.FindAll() on tbl_Fuction.Id equals tbl_Permission.FunctionId into dr_fp
                            from fp in dr_fp.DefaultIfEmpty()
                            where fp.RoleId == roleId && tbl_Fuction.Status == Status.Active
                            select tbl_Fuction.ToModel();

            if (functions != null)
                return functions.ToList();
            return null;
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
