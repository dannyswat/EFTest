using EFTest.Data;
using EFTest.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFTest.Repositories
{
    internal class ProductTypeRepository
    {
        private readonly DataContext context;

        public ProductTypeRepository(DataContext context)
        {
            this.context = context;
        }

        public Task<ProductType> Get(int id)
        {
            return context.ProductTypes.Where(e => e.Id == id).Include(e => e.Attributes).FirstOrDefaultAsync();
        }

        public Task<ProductType> GetNoTracking(int id)
        {
            return context.ProductTypes.Where(e => e.Id == id).AsNoTracking().Include(e => e.Attributes).FirstOrDefaultAsync();
        }

        public void Insert(ProductType entity)
        {
            context.ProductTypes.Add(entity);
        }

        public void Update(ProductType entity)
        {
        }

        public void Delete(ProductType entity)
        {
            context.ProductTypes.Remove(entity);
        }

        public void AddAttribute(ProductType entity, ProductAttribute attribute)
        {
        }

        public void RemoveAttribute(ProductType entity, ProductAttribute attribute)
        {
        }

        public void UpdateAttribute(ProductType entity, ProductAttribute attribute)
        {
        }

        public Task SaveChanges() => context.SaveChangesAsync();
    }
}
