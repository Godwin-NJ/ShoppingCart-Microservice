using AutoMapper;
using Azure;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.DTO;
using Mango.Services.CouponAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Mango.Services.CouponAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CouponController : ControllerBase
    {
        private readonly AppDbContext _appDb;
        private readonly ResponseDto _response;
        private IMapper _mapper;
        public CouponController(AppDbContext appDb, IMapper mapper) 
        {
            _appDb = appDb;
            _response = new ResponseDto();
            _mapper = mapper;   
        }


        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Coupon> objList = _appDb.Coupons.ToList();
                _response.Result = _mapper.Map<IEnumerable<CouponDto>>(objList);               
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet("{id}")]
        public ResponseDto Get(int id)
        {
            try
            {
                var obj = _appDb.Coupons.SingleOrDefault(x => x.CouponId == id);
                _response.Result =  _mapper.Map<CouponDto>(obj);            
                //return objList;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }  
        
        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDto Get(string code)
        {
            try
            {
                var obj = _appDb.Coupons.FirstOrDefault(x => x.CouponCode.ToLower() == code.ToLower());
                if (obj == null) _response.IsSuccess = false;
                _response.Result =  _mapper.Map<CouponDto>(obj);            
                //return objList;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        } 
        
        [HttpPost]
        [Route("createCoupon")]
        [Authorize(Roles ="ADMIN")]
        public ResponseDto CreateCoupon([FromBody] CouponDto dto)
        {
            try
            {
                Coupon objData = _mapper.Map<Coupon>(dto);
                _appDb.Coupons.Add(objData);
                _appDb.SaveChanges();            
                _response.Result =  _mapper.Map<CouponDto>(objData);            
            
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
        
        [HttpPut]
        [Route("updateCoupon")]
		[Authorize(Roles = "ADMIN")]
		public ResponseDto UpdateCoupon([FromBody] CouponDto dto)
        {
            try
            {
                           
                Coupon objData = _mapper.Map<Coupon>(dto);
                _appDb.Coupons.Update(objData);
                _appDb.SaveChanges();            
                _response.Result =  _mapper.Map<CouponDto>(objData);            
            
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }  
        
        [HttpDelete]
        [Route("DeleteCoupon/{id}")]
		[Authorize(Roles = "ADMIN")]
		public ResponseDto DeleteCoupon(int id)
        {
            try
            {
                //var getCouponData = Get(id);
                var getCouponData = _appDb.Coupons.SingleOrDefault(x => x.CouponId == id);
              
              Coupon data = _mapper.Map<Coupon>(getCouponData);
                _appDb.Coupons.Remove(data);
                _appDb.SaveChanges();                                   
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
