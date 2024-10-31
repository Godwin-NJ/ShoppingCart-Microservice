using Mongo.Services.ProductAPI.Dto;
using Mongo.Services.ProductAPI.Models;
using Mongo.Services.ProductAPI.Models.Dto;

namespace Mongo.Services.ProductAPI.Services.IService
{
	public interface IProduct
	{
		Task<ResponseDto> GetProducts();
		Task<ResponseDto> GetProductById(int id);
		Task<ResponseDto> CreateProduct(ProductDto dto);
		Task<ResponseDto> UpdateProduct(ProductDto dto);
		Task<ResponseDto> DeleteProduct(int id);
	}
}
