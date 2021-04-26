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
using WSS.Core.Dto.SearchModel.UserSearch;
using WSS.Core.Repository;
using WSS.Core.Repository.Repositories;
namespace WSS.Service.UserService
{
    public interface IUserService : IDisposable
    {
        Task<bool> AddAsync(UserModel userModel);
        Task<List<UserModel>> GetAllAsync();
        Task<UserModel> GetById(string id);
        Task<bool> DeleteAsync(string id);
        Task<bool> UpdateAsync(UserModel userVm);
    }
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> AddAsync(UserModel userModel)
        {
            var user = new AppUser()
            {
                UserName = userModel.UserName,
                Avatar = userModel.Avatar,
                Email = userModel.Email,
                FullName = userModel.FullName,
                PhoneNumber = userModel.PhoneNumber
            };
            var result = await _userManager.CreateAsync(user, userModel.Password);            
            if (result.Succeeded && userModel.Roles != null && userModel.Roles.Count > 0)
            {
                var appUser = await _userManager.FindByNameAsync(user.UserName);
                if (appUser != null)
                    await _userManager.AddToRolesAsync(appUser, userModel.Roles);
            }
            return true;
        }

        public async Task<UserModel> GetById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            var userVm = user.ToModel(); 
            userVm.Roles = roles.ToList();
            return userVm;
        }
        public async Task<List<UserModel>> GetAllAsync()
        {
            return await _userManager.Users.Select(u=>u.ToModel()).ToListAsync();
        }
        public async Task<bool> DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return true;
                }
            }
            return false;
        }
        public async Task<bool> UpdateAsync(UserModel userVm)
        {
            var user = await _userManager.FindByIdAsync(userVm.Id.ToString());
            //Remove current roles in db
            var currentRoles = await _userManager.GetRolesAsync(user);

            var result = await _userManager.AddToRolesAsync(user,
                userVm.Roles.Except(currentRoles).ToArray());

            if (result.Succeeded)
            {
                string[] needRemoveRoles = currentRoles.Except(userVm.Roles).ToArray();
                await _userManager.RemoveFromRolesAsync(user, needRemoveRoles);

                user.FullName = userVm.FullName;
                user.Email = userVm.Email;
                user.PhoneNumber = userVm.PhoneNumber;
                var status = await _userManager.UpdateAsync(user);
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
