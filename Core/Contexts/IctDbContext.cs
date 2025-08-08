using ICTDashboard.Auth.Models;
using ICTDashboard.Profile.Models;
using Microsoft.EntityFrameworkCore;

namespace ICTDashboard.Core.Contexts;

public class IctDbContext : DbContext
{
    public IctDbContext(DbContextOptions<IctDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        b.Entity<User>(e =>
        {
            e.HasIndex(x => x.Email).IsUnique();
            e.HasIndex(x => x.Username).IsUnique();

            e.Property(u => u.Role).HasConversion<string>();

            e.HasOne(x => x.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<UserProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        b.Entity<UserProfile>(e =>
        {
            e.HasKey(p => p.UserId);
            e.Property(p => p.PictureUrl).HasMaxLength(255);
            e.Property(p => p.Bio).HasMaxLength(500);
        });
    }
}