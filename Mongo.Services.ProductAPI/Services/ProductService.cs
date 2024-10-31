using AutoMapper;
using Mongo.Services.ProductAPI.Data;
using Mongo.Services.ProductAPI.Dto;
using Mongo.Services.ProductAPI.Models;
using Mongo.Services.ProductAPI.Models.Dto;
using Mongo.Services.ProductAPI.Services.IService;

namespace Mongo.Services.ProductAPI.Services
{
	public class ProductService : IProduct
	{
		private readonly IMapper _mapper;
		private readonly AppDbContext _appDb;
		private readonly ResponseDto responseDto;
        public ProductService(AppDbContext appDb, IMapper mapper)
        {
            _appDb = appDb;
			responseDto = new ResponseDto();
			_mapper = mapper;

		}
        public async Task<ResponseDto> CreateProduct(ProductDto dto)
		{
			var mapProduct = _mapper.Map<Product>(dto);
			var product = await _appDb.AddAsync<Product>(mapProduct);
			if (product == null) 
			{
				 responseDto.IsSuccess = false;
				return responseDto;


			}
			responseDto.IsSuccess = true;
			//responseDto.Result = product;
			await _appDb.SaveChangesAsync();
			return responseDto;
		}

		public async Task<ResponseDto> DeleteProduct(int id)
		{
			Product getData = _appDb.Products.SingleOrDefault(x => x.ProductId == id);
			if (getData == null) 
			{ 
				responseDto.IsSuccess = false;
				return responseDto;
			}
			  _appDb.Products.Remove(getData);
			await _appDb.SaveChangesAsync();
			responseDto.IsSuccess = true;
			return responseDto;

		}

		public async Task<ResponseDto> GetProductById(int id)
		{
			var getData =  _appDb.Products.SingleOrDefault(x => x.ProductId == id);
			responseDto.Result = getData;
			return responseDto;
		}

		public async Task<ResponseDto> GetProducts()
		{
			List<Product> productData =  _appDb.Products.ToList();
			responseDto.Result = productData;
			return responseDto;

		}

		public async Task<ResponseDto> UpdateProduct(ProductDto dto)
		{
		
			Product mapData = _mapper.Map<Product>(dto);
			_appDb.Products.Update(mapData);
			await _appDb.SaveChangesAsync();
			responseDto.IsSuccess = true;
			return responseDto;
		}
	}
}
