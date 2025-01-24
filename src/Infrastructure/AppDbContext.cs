using Microsoft.EntityFrameworkCore;

using PicPayChallenge.Infrastructure.EntityConfigurations;
using PicPayChallenge.Models;

namespace PicPayChallenge.Infrastructure;

public class AppDbContext(IConfiguration configuration) : DbContext
{
    public DbSet<Account> Accounts { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("PostgreSQL"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder);
        modelBuilder.ApplyConfiguration(new AccountEntityTypeConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
