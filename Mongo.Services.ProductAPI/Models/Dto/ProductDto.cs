﻿using System.ComponentModel.DataAnnotations;

namespace Mongo.Services.ProductAPI.Models.Dto
{
	public class ProductDto
	{
		public int ProductId { get; set; }	
		public string Name { get; set; }	
		public decimal Price { get; set; }
		public string Description { get; set; }
		public string CategoryName { get; set; }
		public string ImageUrl { get; set; }
	}
}
