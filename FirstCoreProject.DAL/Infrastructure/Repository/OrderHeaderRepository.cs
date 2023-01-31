using FirstCoreProject.DAL.Infrastructure.IRepository;
using FirstCoreProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCoreProject.DAL.Infrastructure.Repository
{
    public class OrderHeaderRepository : GenericRepository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderHeaderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

       

        public void Update(OrderHeader orderHeader)
        {

            _context.OrderHeaders.Update(orderHeader);

           

        }

        public void UpdateStatus(int Id, string orderStatus, string? paymentStatus = null)
        {
            var order = _context.OrderHeaders.FirstOrDefault(x => x.Id == Id);

            if(order!=null)
            {
                order.OrderStatus = orderStatus;
            }


            if(paymentStatus!=null)
            {
                order.PaymentStatus = paymentStatus;
            }
        }

        public void PaymentStatus(int Id, string SessionId, string paymentId)
        {
            var orderHeader = _context.OrderHeaders.FirstOrDefault(x => x.Id == Id);
            orderHeader.DateOfPayment = DateTime.Now;
            orderHeader.PaymentIntentId = paymentId;
            orderHeader.SessionId=SessionId;

        }
    }
}
