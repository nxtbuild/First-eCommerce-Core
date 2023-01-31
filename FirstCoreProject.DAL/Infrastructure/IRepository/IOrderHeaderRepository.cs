using FirstCoreProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCoreProject.DAL.Infrastructure.IRepository
{
    public interface IOrderHeaderRepository : IGenericRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader);

        void UpdateStatus(int Id, string orderStatus, string? paymentStatus = null);  // Id=OrderId

        void PaymentStatus(int Id, string SessionId, string paymentId );  // Id=OrderId

    }
}
