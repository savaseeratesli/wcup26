using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class WCupDbContext : DbContext
{
    public WCupDbContext(DbContextOptions<WCupDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<MatchPrediction> MatchPredictions => Set<MatchPrediction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<MatchPrediction>()
            .HasOne(mp => mp.User)
            .WithMany(u => u.Predictions)
            .HasForeignKey(mp => mp.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
