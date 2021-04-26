using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WSS.Core.Domain.Entities;
using WSS.Core.Dto.DataModel;

namespace WSS.Core.Dto.AutoMapper
{
    public class AutoMapperConfig
    {
        #region Properties
        private static MapperConfiguration _mapperConfiguration;
        private static IMapper _mapper;

        /// <summary>
        /// Mapper
        /// </summary>
        public static IMapper Mapper => _mapper;

        /// <summary>
        /// Mapper configuration
        /// </summary>
        public static MapperConfiguration MapperConfiguration => _mapperConfiguration;
        #endregion

        #region Init
        /// <summary>
        /// Initialize mapper
        /// </summary>
        public static void Init()
        {
            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                //cfg.CreateMap<User, UserModel>();
                cfg.CreateMap<UserModel, User>();
                cfg.CreateMap<AppRole, RoleModel>();
                cfg.CreateMap<RoleModel, AppRole>();
                cfg.CreateMap<Function, FunctionModel>();
                cfg.CreateMap<FunctionModel, Function>();
                cfg.CreateMap<Permission, PermissionModel>();
                cfg.CreateMap<PermissionModel, Permission>();
                cfg.CreateMap<UserRoleModel, UserRole>();
                cfg.CreateMap<UserRole, UserRoleModel>();
                cfg.CreateMap<ProductCategoryModel, ProductCategory>();
                cfg.CreateMap<ProductCategory, ProductCategoryModel>();
                cfg.CreateMap<ProductModel, Product>();
                cfg.CreateMap<Product, ProductModel>();
                cfg.CreateMap<ProductImagesModel, ProductImages>();
                cfg.CreateMap<ProductImages, ProductImagesModel>();
                cfg.CreateMap<ProductQuantityModel, ProductQuantity>();
                cfg.CreateMap<ProductQuantity, ProductQuantityModel>();
                cfg.CreateMap<ColorModel, Color>();
                cfg.CreateMap<Color, ColorModel>();
                cfg.CreateMap<SizeModel, Size>();
                cfg.CreateMap<Size, SizeModel>();
                cfg.CreateMap<Bill, BillModel>();
                cfg.CreateMap<BillModel, Bill>();
                cfg.CreateMap<BillDetailModel, BillDetail>();
                cfg.CreateMap<BillDetail, BillDetailModel>();
            });

            _mapper = _mapperConfiguration.CreateMapper();
        }
        #endregion
    }
}
