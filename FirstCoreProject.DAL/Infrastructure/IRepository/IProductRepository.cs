using FirstCoreProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCoreProject.DAL.Infrastructure.IRepository
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        void Update(Product product);
    }
}
