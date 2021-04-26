using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using WSS.Core.Domain.Entities;
using WSS.Core.Dto.DataModel;

namespace WSS.Core.Dto.AutoMapper
{
    public static class MappingExtensions
    {
        public static MapperConfiguration MapperConfiguration;

        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return AutoMapperConfig.Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return AutoMapperConfig.Mapper.Map(source, destination);
        }              

        public static UserModel ToModel(this AppUser entity)
        {
            return entity.MapTo<AppUser, UserModel>();
        }
        public static AppUser ToUser(this UserModel entity)
        {
            return entity.MapTo<UserModel, AppUser>();
        }
        public static RoleModel ToModel(this AppRole entity)
        {
            return entity.MapTo<AppRole, RoleModel>();
        }
        public static AppRole ToRole(this RoleModel entity)
        {
            return entity.MapTo<RoleModel, AppRole>();
        }
        public static FunctionModel ToModel(this Function entity)
        {
            return entity.MapTo<Function, FunctionModel>();
        }
        public static Function ToFunction(this FunctionModel entity)
        {
            return entity.MapTo<FunctionModel, Function>();
        }
        public static PermissionModel ToModel(this Permission entity)
        {
            return entity.MapTo<Permission, PermissionModel>();
        }
        public static Permission ToPermission(this PermissionModel entity)
        {
            return entity.MapTo<PermissionModel, Permission>();
        }
        public static UserRoleModel ToModel(this UserRole entity)
        {
            return entity.MapTo<UserRole, UserRoleModel>();
        }
        public static UserRole ToUserRole(this UserRoleModel entity)
        {
            return entity.MapTo<UserRoleModel, UserRole>();
        }
        public static ProductCategoryModel ToModel(this ProductCategory entity)
        {
            return entity.MapTo<ProductCategory, ProductCategoryModel>();
        }
        public static ProductCategory ToProductCategory(this ProductCategoryModel entity)
        {
            return entity.MapTo<ProductCategoryModel, ProductCategory>();
        }
        public static ProductModel ToModel(this Product entity)
        {
            return entity.MapTo<Product, ProductModel>();
        }
        public static Product ToProduct(this ProductModel entity)
        {
            return entity.MapTo<ProductModel, Product>();
        }
        public static ProductImagesModel ToModel(this ProductImages entity)
        {
            return entity.MapTo<ProductImages, ProductImagesModel>();
        }
        public static ProductImages ToProductImages(this ProductImagesModel entity)
        {
            return entity.MapTo<ProductImagesModel, ProductImages>();
        }
        public static ProductQuantityModel ToModel(this ProductQuantity entity)
        {
            return entity.MapTo<ProductQuantity, ProductQuantityModel>();
        }
        public static ProductQuantity ToProductQuantity(this ProductQuantityModel entity)
        {
            return entity.MapTo<ProductQuantityModel, ProductQuantity>();
        }
        public static ColorModel ToModel(this Color entity)
        {
            return entity.MapTo<Color, ColorModel>();
        }
        public static Color ToColor(this ColorModel entity)
        {
            return entity.MapTo<ColorModel, Color>();
        }

        public static SizeModel ToModel(this Size entity)
        {
            return entity.MapTo<Size, SizeModel>();
        }
        public static Size ToColor(this SizeModel entity)
        {
            return entity.MapTo<SizeModel, Size>();
        }

        public static BillModel ToModel(this Bill entity)
        {
            return entity.MapTo<Bill, BillModel>();
        }
        public static Bill ToBill(this BillModel entity)
        {
            return entity.MapTo<BillModel, Bill>();
        }

        public static BillDetailModel ToModel(this BillDetail entity)
        {
            return entity.MapTo<BillDetail, BillDetailModel>();
        }
        public static BillDetail ToBillDetail(this BillDetailModel entity)
        {
            return entity.MapTo<BillDetailModel, BillDetail>();
        }
    }
}
