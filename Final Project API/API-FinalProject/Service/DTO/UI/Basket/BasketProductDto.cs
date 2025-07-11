﻿

namespace Service.DTOs.UI.Basket
{
    public class BasketProductDto
    {
		public int ProductId { get; set; }
		public string ProductImage { get; set; }
		public string ProductName { get; set; }
		public decimal Price { get; set; }
		public int ColorId { get; set; }
		public string ColorName { get; set; }
		public int Quantity { get; set; }
	}
}
