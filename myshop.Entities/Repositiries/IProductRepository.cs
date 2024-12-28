using System;
using myshop.Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myshop.Entities.Repositiries
{
   public interface IProductRepository : IGenericRepository<Product>
    {
        void Update(Product product);
    }
}
