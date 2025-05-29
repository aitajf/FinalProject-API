using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTOs.Admin.Products
{
	public class ProductImageDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsMain { get; set; }
	}
}
