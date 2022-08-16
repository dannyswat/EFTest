using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFTest.Entities
{
    public class ProductAttribute
    {
        public int Id { get; private set; }

        public string Name { get; set; }

        public int ProductTypeId { get; private set; }

        public ProductType ProductType { get; set; }

        public bool IsDisabled { get; private set; }
    }
}
