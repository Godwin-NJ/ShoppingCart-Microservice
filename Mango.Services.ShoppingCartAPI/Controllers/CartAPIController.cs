using AutoMapper;
using Mango.Services.ShoppingCartAPI.Data;
using Mango.Services.ShoppingCartAPI.Model;
using Mango.Services.ShoppingCartAPI.Model.Dto;
using Mango.Services.ShoppingCartAPI.Service.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;
using System.Xml;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [Route("api/cart")]
    [ApiController]
    public class CartAPIController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _appDbContext;
        private readonly ResponseDto _responseDto;
        private readonly IProductService _productService;
        private readonly ICouponService _couponService;
        public CartAPIController(IMapper mapper, AppDbContext appDbContext, 
            IProductService productService, ICouponService couponService)
        {
            _mapper = mapper;
            _appDbContext = appDbContext;
            _responseDto = new ResponseDto();
            _productService = productService;
            _couponService = couponService;
        }


        [HttpPost("CartUpsert")]
        public async Task<ResponseDto> CartUpsert(CartDto dto)
        {
            try
            {
                var cartHeaderFromDb = await _appDbContext.CartHeaders.AsNoTracking().FirstOrDefaultAsync
                    (u => u.UserId == dto.CartHeader.UserId);
                if (cartHeaderFromDb == null) 
                { 
                    //create headers and details
                    CartHeader cartHeader = _mapper.Map<CartHeader>(dto.CartHeader);
                    _appDbContext.CartHeaders.Add(cartHeader);
                    await _appDbContext.SaveChangesAsync();
                    dto.CartDetails.First().CartHeaderId = cartHeader.CartHeaderId;
                    _appDbContext.CartDetails.Add(_mapper.Map<CartDetails>(dto.CartDetails.First()));
                     await _appDbContext.SaveChangesAsync();
                }

                else
                {
                    //if header is not null 
                    //check if details has same product
                    var cartDetailsFromDb = await _appDbContext.CartDetails.AsNoTracking().FirstOrDefaultAsync(d =>
                    d.ProductId == dto.CartDetails.FirstOrDefault().ProductId 
                    && d.CartHeaderId == cartHeaderFromDb.CartHeaderId);

                    if(cartDetailsFromDb == null)
                    {
                        //create cartDeatils 
                        dto.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        _appDbContext.CartDetails.Add(_mapper.Map<CartDetails>(dto.CartDetails.First()));
                        await _appDbContext.SaveChangesAsync();

                    }
                    else
                    {
                        //update count in cart details
                        dto.CartDetails.First().Count += cartDetailsFromDb.Count;
                        dto.CartDetails.First().CartHeaderId = cartDetailsFromDb.CartHeaderId;
                        dto.CartDetails.First().CartDetailsId = cartDetailsFromDb.CartDetailsId;
                        _appDbContext.CartDetails.Update(_mapper.Map<CartDetails>(dto.CartDetails.First()));
                        await _appDbContext.SaveChangesAsync();
                    }
                    
                }
                _responseDto.Result = dto;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message;
                _responseDto.IsSuccess = false;
              
            }
          return  _responseDto;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ResponseDto> GetCart(string userId)
        {
            try
            {
                CartDto cart = new()
                {
                    CartHeader = _mapper.Map<CartHeaderDto>(_appDbContext.CartHeaders.First(x => x.UserId == userId)),
                };
                cart.CartDetails = _mapper.Map<IEnumerable<CartDetailsDto>>(_appDbContext.CartDetails.Where
                    (x => x.CartHeaderId == cart.CartHeader.CartHeaderId));

                List<ProductDto> Products = await _productService.GetProducts();

                foreach (var item in cart.CartDetails) 
                {
                    item.Product = Products.FirstOrDefault(x => x.ProductId == item.ProductId);
                    cart.CartHeader.CartTotal += (item.Count * item.Product.Price);
                }

                //apply coupon if any
                if (!string.IsNullOrEmpty(cart.CartHeader.CouponCode))
                {
                    CouponDto coupon = await _couponService.GetCoupon(cart.CartHeader.CouponCode);

                    if (coupon != null && cart.CartHeader.CartTotal > coupon.MinAmount)
                    {
                        cart.CartHeader.CartTotal -= coupon.DiscountAmount;
                        cart.CartHeader.Discount = coupon.DiscountAmount;
                    }
                    _responseDto.Result = cart;
                }
            }
            catch (Exception ex)
            {

                _responseDto.Message = ex.Message;
                _responseDto.IsSuccess = false;
            }
            return _responseDto;
        }

        //[HttpPost("applycoupon")]
        //public async Task<ResponseDto> ApplyCoupon(string couponId)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {
        //        _responseDto.IsSuccess = false;
        //        _responseDto.Message = ex.Message;
               
        //    }
        //}

        [HttpPost]
        [Route("applycoupon")]
        public async Task<ResponseDto> ApplyCoupon([FromBody]CartDto dto)
        {
            try
            {
                var cartFromDb = await _appDbContext.CartHeaders.SingleOrDefaultAsync(x => x.UserId == dto.CartHeader.UserId);
                cartFromDb.CouponCode = dto.CartHeader.CouponCode;
                _appDbContext.CartHeaders.Update(cartFromDb);
                await _appDbContext.SaveChangesAsync();
                _responseDto.Result = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;

            }
            return _responseDto;
        }

        [HttpPost]
        [Route("EmailCartRequest")]
        public async Task<ResponseDto> EmailCartRequest([FromBody] CartDto dto)
        {
            try
            {
                var cartFromDb = await _appDbContext.CartHeaders.SingleOrDefaultAsync(x => x.UserId == dto.CartHeader.UserId);
                cartFromDb.CouponCode = dto.CartHeader.CouponCode;
                _appDbContext.CartHeaders.Update(cartFromDb);
                await _appDbContext.SaveChangesAsync();
                _responseDto.Result = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;

            }
            return _responseDto;
        }

        [HttpPost("removecoupon")]
        public async Task<ResponseDto> RemoveCoupon([FromBody] CartDto dto)
        {
            try
            {
                var cartFromDb = await _appDbContext.CartHeaders.SingleOrDefaultAsync(x => x.UserId == dto.CartHeader.UserId);
                cartFromDb.CouponCode = "";
                _appDbContext.CartHeaders.Update(cartFromDb);
                await _appDbContext.SaveChangesAsync();
                _responseDto.Result = true;
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.Message = ex.Message;

            }
            return _responseDto;
        }


        [HttpPost("RemoveCart")]
        public async Task<ResponseDto> RemoveCart([FromBody]int cartDeatilsID)
        {
            try
            {
                CartDetails cartDetails =  _appDbContext.CartDetails.First
                    (u => u.CartDetailsId == cartDeatilsID);
                int totalCountOfCartItem = _appDbContext.CartDetails.Where
                    (x => x.CartHeaderId == cartDetails.CartHeaderId).Count();

                if(totalCountOfCartItem == 1)
                {
                    var cartHeaderToRemove = await _appDbContext.CartHeaders.FirstOrDefaultAsync
                        (x => x.CartHeaderId == cartDetails.CartHeaderId);
                    _appDbContext.CartHeaders.Remove(cartHeaderToRemove);
                }
              
                await _appDbContext.SaveChangesAsync ();
                //_responseDto.Result = dto;
                _responseDto.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _responseDto.Message = ex.Message;
                _responseDto.IsSuccess = false;

            }
            return _responseDto;
        }

    }
}
