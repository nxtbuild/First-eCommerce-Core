using FirstCoreProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCoreProject.DAL.Infrastructure.IRepository
{
    public interface ICartRepository:IGenericRepository<Cart>
    {
        int IncrementCartItem(Cart cart, int count);
        int DecrementCartItem(Cart cart, int count);
    }
}
