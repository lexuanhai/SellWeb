//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using WSS.Core.Common.Infrastructure;
//using WSS.Core.Domain.Entities;
//using WSS.Core.Dto.AutoMapper;
//using WSS.Core.Dto.DataModel;
//using WSS.Core.Dto.SearchModel.RoleSearch;
//using WSS.Core.Dto.SearchModel.UserSearch;
//using WSS.Core.Repository;
//using WSS.Core.Repository.Repositories;

//namespace WSS.Service.RoleService
//{
//    public interface IRoleService : IDisposable
//    {
//        Task<bool> AddAsync(RoleModel roleVm);
//        Task<List<RoleModel>> GetAllAsync();
//        Task<bool> UpdateAsync(RoleModel roleVm);
//        Task<bool> IsExist(string roleName);
//        Task<RoleModel> GetRoleById(Guid id);
//        Task<bool> DeleteAsync(Guid id);
//        List<PermissionModel> GetListFunctionWithRole(Guid roleId);
//        bool SavePermission(List<PermissionModel> permissionModels, Guid roleId);
//        List<RoleModel> GetRoleByUserName(Guid? userName);
//        Task<AppRole> GetRoleByName(string roleName);
//        Task<bool> CheckPermission(int functionId, string action, string[] roles);
//        Task<string> GetRoleId(AppRole role);
//    }
//    public class RoleService : IRoleService
//    {
//        private RoleManager<AppRole> _roleManager;
//        private readonly IAppRoleRepository _appRoleRepository;
//        private IUnitOfWork _unitOfWork;
//        private readonly IFunctionRepository _functionRepository;
//        private readonly IPermissionRepository _permissionRepository;
//        private readonly IUserRoleRepository _userRoleRepository;
//        public RoleService(
//            RoleManager<AppRole> roleManager, 
//            IUnitOfWork unitOfWork,
//            IFunctionRepository functionRepository,
//            IPermissionRepository permissionRepository,
//            IUserRoleRepository userRoleRepository,
//            IAppRoleRepository appRoleRepository)
//        {
//            _roleManager = roleManager;
//            _unitOfWork = unitOfWork;
//            _functionRepository = functionRepository;
//            _permissionRepository = permissionRepository;
//            _userRoleRepository = userRoleRepository;
//            _appRoleRepository = appRoleRepository;
//        }
//        public async Task<bool> AddAsync(RoleModel roleVm)
//        {
//            var role = new AppRole()
//            {
//                Name = roleVm.Name,
//                Description = roleVm.Description
//            };
//            var result = await _roleManager.CreateAsync(role);
//            return result.Succeeded;
//        }
//        public async Task<List<RoleModel>> GetAllAsync()
//        {
//            return await _roleManager.Roles.Select(r=>r.ToModel()).ToListAsync();            
//        }
//        public async Task<bool> UpdateAsync(RoleModel roleVm)
//        {
//            var role = await _roleManager.FindByIdAsync(roleVm.Id.ToString());
//            role.Description = roleVm.Description;
//            role.Name = roleVm.Name;
//            var result = await _roleManager.UpdateAsync(role);
//            return result.Succeeded;
//        }
//        public async Task<bool> IsExist(string roleName)
//        {
//            var role = await _roleManager.FindByNameAsync(roleName);
//            if (role != null)
//            {
//                return true;
//            }
//            return false;
//        }
//        public async Task<RoleModel> GetRoleById(Guid id)
//        {
//            var role = await _roleManager.FindByIdAsync(id.ToString());
//            return role.ToModel();
//        }
//        public async Task<bool> DeleteAsync(Guid id)
//        {
//            var role = await _roleManager.FindByIdAsync(id.ToString());
//            if (role != null)
//            {
//               var status = await _roleManager.DeleteAsync(role);
//                if (status.Succeeded)
//                {
//                    return true;
//                }
//            }
//            return false;          
//        }
//        public List<PermissionModel> GetListFunctionWithRole(Guid roleId)
//        {
//            var functions = _functionRepository.FindAll();
//            var permissions = _permissionRepository.FindAll();
//            var query = from f in functions
//                        join p in permissions on f.Id equals p.FunctionId into fp
//                        from p in fp.DefaultIfEmpty()
//                        where p != null && p.RoleId == roleId
//                        select new PermissionModel()
//                        {
//                            RoleId = roleId,
//                            FunctionId = f.Id,
//                            CanCreate = p != null ? p.CanCreate.Value : false,
//                            CanUpdate = p != null ? p.CanUpdate.Value : false,
//                            CanDelete = p != null ? p.CanDelete.Value : false,
//                            CanRead = p != null ? p.CanRead.Value : false
//                        };
//            return query.ToList();
//        }
//        public bool SavePermission(List<PermissionModel> permissionModels, Guid roleId)
//        {
//            bool status = false;
//            try
//            {
//                var permissions = permissionModels.Select(p => p.ToPermission()).ToList();
//                var oldPermission = _permissionRepository.FindAll().Where(x => x.RoleId == roleId).ToList();
//                if (oldPermission.Count > 0)
//                {
//                    _permissionRepository.RemoveMultiple(oldPermission);
//                }
//                foreach (var permission in permissions)
//                {
//                    _permissionRepository.Add(permission);
//                }
//                _unitOfWork.Commit();
//                status = true;
//                return status;
//            }
//            catch (Exception ex)
//            {
//                status = false;
//                throw;
//            }
//            return status;
//        }
//        public Task<bool> CheckPermission(int functionId, string action, string[] roles)
//        {
//            var functions = _functionRepository.FindAll();
//            var permissions = _permissionRepository.FindAll();
//            var query = from f in functions
//                        join p in permissions on f.Id equals p.FunctionId
//                        join r in _roleManager.Roles on p.RoleId equals r.Id
//                        where roles.Contains(r.Name) && f.Id == functionId
//                        && ((p.CanCreate.Value && action == "Create")
//                        || (p.CanUpdate.Value && action == "Update")
//                        || (p.CanDelete.Value && action == "Delete")
//                        || (p.CanRead.Value && action == "Read"))
//                        select p;
//            return query.AnyAsync();
//        }
//        public Task<AppRole> GetRoleByName(string roleName)
//        {           
//            var role =  _roleManager.FindByNameAsync(roleName);
//            return role;           
//        }
//        public Task<string> GetRoleId(AppRole role)
//        {
//            var roleId = _roleManager.GetRoleIdAsync(role);
//            return roleId;
//        }
//        public List<RoleModel> GetRoleByUserName(Guid? userId)
//        {            
//            if (userId.HasValue)
//            {
//                var lstRole = from tbl_role in _appRoleRepository.FindAll()
//                              join tbl_userRole in _userRoleRepository.FindAll() on tbl_role.Id equals tbl_userRole.RoleId into role_user
//                              from ru in role_user.DefaultIfEmpty()
//                              where ru.UserId == userId.Value
//                              select tbl_role.ToModel();
//                if (lstRole != null && lstRole.Count() > 0)
//                    return lstRole.ToList();
//            }
//            return null;
//        }

//        public void Dispose()
//        {
//            GC.SuppressFinalize(this);
//        }
//    }
//}
