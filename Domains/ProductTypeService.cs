using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using EFTest.Data;
using EFTest.Entities;
using EFTest.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace EFTest.Domains
{
    internal class ProductTypeService
    {
        private readonly ProductTypeRepository repository;
        private readonly DataContext db;
        private IDbContextTransaction transaction;
        public ProductTypeService(ProductTypeRepository repository, DataContext db)
        {
            this.repository = repository;
            this.db = db;
        }

        public async Task MakeTransaction(Func<Task> action)
        {
            using (var ts = db.Database.BeginTransaction())
            {
                try
                {
                    await action();
                    ts.Commit();
                }
                catch
                {
                    ts.Rollback();
                }
            }
        }

        public async Task<ProductType> Get(int id)
        {
            return await repository.Get(id);
        }

        public async Task<ProductType> GetNoTracking(int id)
        {
            using (DataContext newDb = new DataContext())
            {
                return await newDb.ProductTypes.Where(e => e.Id == id).Include(e => e.Attributes).FirstOrDefaultAsync();
            }
        }

        public async Task Add(ProductType entity)
        {
            repository.Insert(entity);

            foreach (var attr in entity.Attributes)
                repository.AddAttribute(entity, attr);

            await repository.SaveChanges();
        }

        public async Task Update(ProductType entity)
        {
            var orig = await repository.Get(entity.Id);

            // Update entity
            orig.Name = entity.Name;

            List<ProductAttribute> added = new List<ProductAttribute>();
            List<ProductAttribute> removed = new List<ProductAttribute>();

            foreach (var attr in entity.Attributes)
                if (!orig.Attributes.Any(e => e.Id == attr.Id))
                    added.Add(attr);

            foreach (var origAttr in orig.Attributes)
            {
                var attr = entity.Attributes.FirstOrDefault(e => e.Id == origAttr.Id);
                if (attr != null)
                {
                    origAttr.Name = attr.Name;
                    repository.UpdateAttribute(entity, origAttr);
                }
                else
                    removed.Add(origAttr);
            }

            foreach (var attr in removed)
            {
                repository.RemoveAttribute(entity, attr);
                orig.RemoveAttribute(attr);
            }
            foreach (var attr in added)
            {
                orig.AddAttribute(attr);
                repository.AddAttribute(entity, attr);
            }

            await repository.SaveChanges();
        }

        public async Task Delete(int id)
        {
            var orig = await repository.Get(id);
            repository.Delete(orig);
            await repository.SaveChanges();
        }
    }
}
