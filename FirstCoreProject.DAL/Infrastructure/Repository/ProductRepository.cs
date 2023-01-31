using FirstCoreProject.DAL.Infrastructure.IRepository;
using FirstCoreProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCoreProject.DAL.Infrastructure.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Product product)
        {
            var results = _context.Products.FirstOrDefault(x => x.Id == product.Id);

            if (results != null)
            {
                results.Name = product.Name;
                results.Price = product.Price; ;
                results.Description = product.Description;  

                if(product.ImageUrl!=null)
                {
                    results.ImageUrl=product.ImageUrl;
                }

            }

        }
    }
}
