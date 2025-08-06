using Microsoft.EntityFrameworkCore;
using TvMazeScraper.Domain.Entities;

namespace TvMazeScraper.Persistence
{
    public class TvMazeScraperDbContext : DbContext
    {
        public TvMazeScraperDbContext(DbContextOptions<TvMazeScraperDbContext> options)
            : base(options)
        {
        }

        public DbSet<Show> Shows { get; set; }
        public DbSet<CastMember> CastMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Show>(entity =>
            {
                entity.HasKey(s => s.Id);

                entity.Property(s => s.Name)
                      .IsRequired()
                      .HasMaxLength(200);

                entity.Property(s => s.Premiered)
                      .IsRequired(false);

                entity.HasMany(s => s.CastMembers)
                      .WithOne(cm => cm.Show)
                      .HasForeignKey(cm => cm.ShowId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CastMember>(entity =>
            {
                entity.HasKey(cm => cm.Id);

                entity.Property(cm => cm.Name)
                      .IsRequired()
                      .HasMaxLength(150);

                entity.Property(cm => cm.Birthday)
                      .IsRequired(false);
            });
        }
    }
}
