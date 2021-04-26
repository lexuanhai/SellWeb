using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSS.Core.Common.Infrastructure;
using WSS.Core.Domain.Entities;
using WSS.Core.Dto.AutoMapper;
using WSS.Core.Dto.DataModel;
using WSS.Core.Dto.SearchModel.RoleSearch;
using WSS.Core.Dto.SearchModel.UserSearch;
using WSS.Core.Repository;
using WSS.Core.Repository.Repositories;

namespace WSS.Service.RoleService
{
    public interface IRoleService : IDisposable
    {
        Task<bool> AddAsync(RoleModel roleVm);
        Task<List<RoleModel>> GetAllAsync();
        Task<bool> UpdateAsync(RoleModel roleVm);
        Task<bool> IsExist(string roleName);
        Task<RoleModel> GetRoleById(Guid id);
        Task<bool> DeleteAsync(Guid id);
    }
    public class RoleService : IRoleService
    {
        private RoleManager<AppRole> _roleManager;
        private IUnitOfWork _unitOfWork;
        public RoleService(
            RoleManager<AppRole> roleManager, IUnitOfWork unitOfWork)
        {
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }
        public async Task<bool> AddAsync(RoleModel roleVm)
        {
            var role = new AppRole()
            {
                Name = roleVm.Name,
                Description = roleVm.Description
            };
            var result = await _roleManager.CreateAsync(role);
            return result.Succeeded;
        }
        public async Task<List<RoleModel>> GetAllAsync()
        {
            return await _roleManager.Roles.Select(r=>r.ToModel()).ToListAsync();
        }
        public async Task<bool> UpdateAsync(RoleModel roleVm)
        {
            var role = await _roleManager.FindByIdAsync(roleVm.Id.ToString());
            role.Description = roleVm.Description;
            role.Name = roleVm.Name;
            var result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;
        }
        public async Task<bool> IsExist(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                return true;
            }
            return false;
        }
        public async Task<RoleModel> GetRoleById(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            return role.ToModel();
        }
        public async Task<bool> DeleteAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role != null)
            {
               var status = await _roleManager.DeleteAsync(role);
                if (status.Succeeded)
                {
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
