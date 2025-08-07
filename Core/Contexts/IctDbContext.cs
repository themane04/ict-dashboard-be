using ICTDashboard.Auth.Models;
using Microsoft.EntityFrameworkCore;

namespace ICTDashboard.Core.Contexts;

public class IctDbContext : DbContext
{
    public IctDbContext(DbContextOptions<IctDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
}