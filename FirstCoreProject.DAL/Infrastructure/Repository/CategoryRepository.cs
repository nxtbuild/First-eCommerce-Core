using FirstCoreProject.DAL.Infrastructure.IRepository;
using FirstCoreProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCoreProject.DAL.Infrastructure.Repository
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public void Update(Category category)
        {
            var results = _context.Categories.FirstOrDefault(x => x.Id == category.Id);

            if (results != null)
            {
                results.Name=   category.Name;
                results.DisplayOrder= category.DisplayOrder;    
                
            }

        }
    }
}
