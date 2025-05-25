using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Services.Interfaces;

namespace Service.Services
{
    public class ProductService : IProductService
    {
        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
