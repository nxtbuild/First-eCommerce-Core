using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCoreProject.Models.ViewModels
{
    public class CartViewModel
    {
        public IEnumerable<Cart> ListOfCart { get; set; }

        public OrderHeader OrderHeader { get; set; }
    }
}
