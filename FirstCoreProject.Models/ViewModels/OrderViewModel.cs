using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCoreProject.Models.ViewModels
{
    public class OrderViewModel
    {
        // For Get Order Details

        public OrderHeader OrderHeader { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; }   // Get Order List Of Product
    }
}
