using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IProductService
    {
		Task<ResponseDto> GetProducts();
		Task<ResponseDto> GetProductById(int id);
		Task<ResponseDto> CreateProduct(ProductDto dto);
		Task<ResponseDto> UpdateProduct(ProductDto dto);
		Task<ResponseDto> DeleteProduct(int id);
	}
}
