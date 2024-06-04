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
        //public DbSet<Reserve_Info> Reserve_Infos { get; set; }
        public DbSet<Reien> Reiens { get; set; }
        public DbSet<CemeteryInfo> CemeteryInfos { get; set; }
        public DbSet<Cemetery> Cemeteries { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Section> Sections { get; set; }
    }
}
