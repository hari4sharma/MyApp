using Microsoft.EntityFrameworkCore;

namespace WebAppWeb.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            :base(options)
        {
                
        }

        public DbSet<WebAppWeb.Models.Category> Categories { get; set; }
    }
}
