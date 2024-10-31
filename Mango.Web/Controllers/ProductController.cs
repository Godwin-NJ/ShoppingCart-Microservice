using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService  _productService;
        public ProductController(IProductService productService) 
        {
			_productService = productService;
        }
        /// <summary>
        /// Coupon Index
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto> productList = new();
            ResponseDto? resp = await _productService.GetProducts();

            if (resp != null && resp.IsSuccess)
            {
                productList = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(resp.Result));
            }
            else
            {
                TempData["error"] = resp?.Message;
            }

            return View(productList);
        }

        public async Task<IActionResult> CreateProduct()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductDto dto)
        {
            if (ModelState.IsValid)
            {
                ResponseDto? resp = await _productService.CreateProduct(dto);
                
                if (resp != null && resp.IsSuccess)
                {
                  return  RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = resp?.Message;
                }

            }
            return View(dto);
        }

        public async Task<IActionResult> DeleteProduct(int id)
        {
			ResponseDto? resp = await _productService.GetProductById(id);

			if (resp != null && resp.IsSuccess)
			{				
				ProductDto? data = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(resp.Result));

              return View(data);
			}
            else
            {
                TempData["error"] = resp?.Message;
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(ProductDto dto)
        {
			ResponseDto? resp = await _productService.DeleteProduct(dto.ProductId);

			if (resp != null && resp.IsSuccess)
			{
				TempData["success"] = "Product deleted successful";
				return RedirectToAction(nameof(ProductIndex)); 
			}
            else
            {
                TempData["error"] = resp?.Message;
            }
            return View(dto); 
        }

		public async Task<IActionResult> ProductEdit(int id)
		{
			ResponseDto? resp = await _productService.GetProductById(id);

			if (resp != null && resp.IsSuccess)
			{
				ProductDto? data = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(resp.Result));

				return View(data);
			}
			else
			{
				TempData["error"] = resp?.Message;
			}
			return NotFound();
		}

		[HttpPost]
		public async Task<IActionResult> ProductEdit(ProductDto dto)
		{
			ResponseDto? resp = await _productService.UpdateProduct(dto);

			if (resp != null && resp.IsSuccess)
			{
				TempData["success"] = "Product updated successfully";
				return RedirectToAction(nameof(ProductIndex));
			}
			else
			{
				TempData["error"] = resp?.Message;
			}
			return View(dto);
		}
	}
}
