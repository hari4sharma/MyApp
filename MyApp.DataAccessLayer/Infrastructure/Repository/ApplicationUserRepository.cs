using MyApp.Data;
using MyApp.DataAccessLayer.Infrastructure.IRepository;
using MyApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.DataAccessLayer.Infrastructure.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUser
    {
        private readonly ApplicationDBContext _context;
        public ApplicationUserRepository(ApplicationDBContext context) : base(context)
        {
            _context = context;
        }

    }
}
