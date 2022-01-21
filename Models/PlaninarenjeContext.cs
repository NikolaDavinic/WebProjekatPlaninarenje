using Microsoft.EntityFrameworkCore;
namespace Models
{
    public class PlaninarenjeContext : DbContext
    {
        public PlaninarenjeContext(DbContextOptions options):base(options) {}
        
        public DbSet<Planinar> Planinari { get; set; }

        public DbSet<PlaninarskoDrustvo> PlaninarskaDrustva { get; set; }

        public DbSet<Dogadjaj> Dogadjaji { get; set; }

        public DbSet<Planina> Planine { get; set; }

        public DbSet<Organizuje> PlaninarskDrustvaDogadjaji { get; set; }

        public DbSet<IdeNaDogadjaj> PlaninariDogadjaji { get; set; }
    }
}