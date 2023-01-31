using FirstCoreProject.DAL.Infrastructure.IRepository;
using FirstCoreProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCoreProject.DAL.Infrastructure.Repository
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderDetailRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

      

        public void Update(OrderDetail orderDetail)
        {
            _context.OrderDetails.Update(orderDetail);  
        }
    }
}
