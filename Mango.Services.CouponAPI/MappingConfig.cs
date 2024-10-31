using AutoMapper;
using Mango.Services.CouponAPI.DTO;
using Mango.Services.CouponAPI.Models;

namespace Mango.Services.CouponAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(Config =>
            {
                Config.CreateMap<Coupon, CouponDto>();
                Config.CreateMap<CouponDto, Coupon>();
            });
            return mappingConfig;
        }
    }
}
