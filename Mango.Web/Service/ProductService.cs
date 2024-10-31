using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;


namespace Mango.Web.Service
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;
        public ProductService(IBaseService baseService) 
        {
            _baseService = baseService;
        }
        public async Task<ResponseDto> CreateProduct(ProductDto dto)
        {
            var resp = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = dto,
                Url = StaticDetails.ProductApiBaseUrl + $"/api/Product/createproduct"
            });
            return resp;
        }

        public async Task<ResponseDto> DeleteProduct(int id)
        {
            var resp = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.DELETE,
                Url = StaticDetails.ProductApiBaseUrl + $"/api/Product/deleteproduct/{id}"
            });
            return resp;
        }

        public async Task<ResponseDto> GetProducts()
        {
          var resp =  await  _baseService.SendAsync(new RequestDto()
            {
              ApiType = StaticDetails.ApiType.GET,
              Url = StaticDetails.ProductApiBaseUrl + "/api/Product/getproducts"
		  });
            return resp;
        }


        public async Task<ResponseDto> GetProductById(int id)
        {
            var resp = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.GET,
                Url = StaticDetails.ProductApiBaseUrl + $"/api/Product/getproductbyid/{id}"
            });
            return resp;
        }

        public async Task<ResponseDto> UpdateProduct(ProductDto dto)
        {
            var resp = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = StaticDetails.ApiType.PUT,
                Data = dto,
                Url = StaticDetails.ProductApiBaseUrl + $"/api/Product/updateproduct"
            });
            return resp;
        }
    }
}
