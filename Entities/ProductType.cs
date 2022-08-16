using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFTest.Entities
{
    public class ProductType
    {
        public int Id { get; private set; }

        public string Name { get; set; }

        public bool IsDisabled { get; private set; }

        public IEnumerable<ProductAttribute> Attributes { get => attributes; private set => value.ToList(); }

        private List<ProductAttribute> attributes = new List<ProductAttribute>();

        public void AddAttribute(ProductAttribute attribute)
        {
            attributes.Add(attribute);
        }

        public void RemoveAttribute(ProductAttribute attribute)
        {
            attributes.Remove(attribute);
        }
    }
}
