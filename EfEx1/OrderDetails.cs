using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfEx1
{
    public class OrderDetails
    {
        public int UnitPrice { get; set; }
        public int OrderQuantity { get; set; }
        public int OrderDiscount { get; set; }
        public int ProductId { get; set; }
        public int OrderId { get; set; }
        public Products Product { get; set; }
        public Orders Order { get; set; }
    }
}
