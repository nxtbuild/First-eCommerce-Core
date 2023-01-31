using FirstCoreProject.DAL.Infrastructure.IRepository;
using FirstCoreProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCoreProject.DAL.Infrastructure.Repository
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        private readonly ApplicationDbContext _context;
        public CartRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public int DecrementCartItem(Cart cart, int count)
        {
            cart.Count -= count;
            return cart.Count;
        }

        public int IncrementCartItem(Cart cart, int count)
        {
            cart.Count += count;
            return cart.Count;
        }
    }
}
