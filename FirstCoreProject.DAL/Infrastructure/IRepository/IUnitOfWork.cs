using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCoreProject.DAL.Infrastructure.IRepository
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }

        IProductRepository Product { get; }

        ICartRepository Cart { get; }
        IApplicationUserRepository ApplicationUser { get; }

        IOrderDetailRepository OrderDetail { get; }
        IOrderHeaderRepository OrderHeader { get; }


        void Save();
    }
}
