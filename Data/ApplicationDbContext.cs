using Microsoft.EntityFrameworkCore;
using YasiroRegrave.Model;
using YasiroRegrave.Models;

namespace YasiroRegrave.Data
{

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Vender> Venders { get; set; }
        //public DbSet<Reserve_Info> Reserve_Infos { get; set; }
        public DbSet<Reien> Reiens { get; set; }

        public DbSet<CemeteryInfo> CemeteryInfos { get; set; }

    }
}
