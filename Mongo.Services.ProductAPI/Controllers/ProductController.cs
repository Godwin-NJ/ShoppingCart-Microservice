using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mongo.Services.ProductAPI.Models.Dto;
using Mongo.Services.ProductAPI.Services.IService;

namespace Mongo.Services.ProductAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	//[Authorize]
	public class ProductController : ControllerBase
	{
        private readonly IProduct _product;
       
        public ProductController(IProduct product)
        {
            _product = product;
        }

        /// <summary>
        /// Create Product
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost("createproduct")]
		[Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto dto)
        {
            var createItem = await _product.CreateProduct(dto);
           return Ok(createItem);
        }  
        
        /// <summary>
        /// Delete Product
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpDelete("deleteproduct/{id}")]
		[Authorize(Roles = "ADMIN")]
		public async Task<IActionResult> DeleteProduct(int id)
        {
            var createItem = await _product.DeleteProduct(id);
           return Ok(createItem);
        }  
        
        /// <summary>
        /// Get single product
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("getproductbyid/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var createItem = await _product.GetProductById(id);
           return Ok(createItem);
        }  
        
        /// <summary>
        /// Get products
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet("getproducts")]
        public async Task<IActionResult> GetProducts()
        {
            var createItem = await _product.GetProducts();
           return Ok(createItem);
        } 
        
        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("updateproduct")]
		[Authorize(Roles = "ADMIN")]
		public async Task<IActionResult> UpdateProduct([FromBody] ProductDto dto)
		{
            var createItem = await _product.UpdateProduct(dto);
           return Ok(createItem);
        }

	}
}
