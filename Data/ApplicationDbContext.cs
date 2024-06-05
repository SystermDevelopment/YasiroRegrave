using Microsoft.EntityFrameworkCore;
using YasiroRegrave.Model;

namespace YasiroRegrave.Data
{

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        
        public DbSet<Vender> Venders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Reien> Reiens { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<AreaCoord> AreaCoords { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<ReserveInfo> ReserveInfos { get; set; }
        public DbSet<SectionCoord> SectionCoords { get; set; }
        public DbSet<Cemetery> Cemeteries { get; set; }
        public DbSet<CemeteryInfo> CemeteryInfos { get; set; }
    }
}
