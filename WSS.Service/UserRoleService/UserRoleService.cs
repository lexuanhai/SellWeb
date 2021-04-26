//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using WSS.Core.Common.Infrastructure;
//using WSS.Core.Domain.Entities;
//using WSS.Core.Dto.AutoMapper;
//using WSS.Core.Dto.DataModel;
//using WSS.Core.Dto.SearchModel.UserSearch;
//using WSS.Core.Repository;
//using WSS.Core.Repository.Repositories;

//namespace WSS.Service.UserRoleService
//{
//    public interface IUserRoleService : IDisposable
//    {
//        bool SaveRoleForUser(List<UserRoleModel> userRoles, int idUser);
//        List<UserRoleModel> GetRoleByUserId(int userId);

//    }
//    public class UserRoleService : IUserRoleService
//    {
//        IUserRepository _userRepository;
//        IUserRoleRepository _userRoleRepository;
//        IUnitOfWork _unitOfWork;
//        public UserRoleService(
//            IUserRepository userRepository,
//            IUserRoleRepository userRoleRepository,
//            IUnitOfWork unitOfWork)
//        {
//            _userRepository = userRepository;
//            _userRoleRepository = userRoleRepository;
//            _unitOfWork = unitOfWork;
//        }
//        public bool SaveRoleForUser(List<UserRoleModel> userRoles, int idUser)
//        {

//            var userRoleByUsers = _userRoleRepository.FindAll().Where(r => r.UserId == idUser).ToList();
//            if (userRoleByUsers != null && userRoleByUsers.Count > 0)
//            {
//                _userRoleRepository.RemoveMultiple(userRoleByUsers);
//            }
//            if (userRoles.Count > 0 && idUser > 0)
//            {

//                foreach (var item in userRoles)
//                {
//                    item.CreatedDate = DateTime.Now;
//                    _userRoleRepository.Add(item.ToUserRole());
//                }
//                return true;
//            }
//            return false;
//        }

//        public List<UserRoleModel> GetRoleByUserId(int userId)
//        {
//            if (userId > 0)
//            {
//                var rolesByUserId = _userRoleRepository.FindAll().Where(r => r.UserId == userId).Select(r => r.ToModel()).ToList();
//                if (rolesByUserId != null && rolesByUserId.Count > 0)
//                {
//                    return rolesByUserId;
//                }
//            }
//            return new List<UserRoleModel>();
//        }
//        public void Dispose()
//        {
//            GC.SuppressFinalize(this);
//        }
//    }
//}
