using Microsoft.EntityFrameworkCore.Metadata;
using MyApp.Data;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.DataAccessLayer.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public ICategoryRepository Category { get; private set; }

        public IProductRepository Product { get; private set; }

        public IApplicationUser ApplicationUser { get; protected set; }

        public ICartReposotory CartReposotory { get; protected set; }

        private readonly ApplicationDBContext _context;

        public UnitOfWork(ApplicationDBContext context)
        {
            _context = context;
            Category = new CategoryRepository(_context);
            Product = new ProductRepository(_context);
            CartReposotory = new CartRepository(_context);
            ApplicationUser = new ApplicationUserRepository(_context);
        }

        public void save()
        {
            _context.SaveChanges();
        }
    }
}
