using Microsoft.EntityFrameworkCore;
using SimpleArchitectureEntities.Models;

namespace SimpleArchitectureDAL.Infrastructure
{
    public class SimpleArchitectureContext : DbContext
    {
        public SimpleArchitectureContext(DbContextOptions<SimpleArchitectureContext> options) : base(options)
        {
        }
        public virtual DbSet<Group> Groups { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasKey(e => e.IdGroup)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("Groups");

                entity.HasIndex(e => e.IdGroup)
                    .HasName("PK_Groups")
                    .IsUnique();                
            });
        }
    }
}
