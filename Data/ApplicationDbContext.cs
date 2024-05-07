using Microsoft.EntityFrameworkCore;
using YasiroRegrave.Model;

namespace YasiroRegrave.Data
{

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Vender> Venders { get; set; }
    }
}
