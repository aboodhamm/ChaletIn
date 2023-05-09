using Chaletin.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Chaletin.Areas.Identity.Data;

public class ChaletinDbContext : IdentityDbContext<User>
{
    public ChaletinDbContext(DbContextOptions<ChaletinDbContext> options)
        : base(options)
    {
    }
    public DbSet<Farm> Farm { get; set; }
    public DbSet<Booking> Booking{ get; set; }
    public DbSet<Comments> Comments { get; set; }
    public DbSet<ContactMessage> ContactMessage { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Farm>()
            .Property(e => e.Images)
            .HasConversion(
                v => string.Join(',', v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
    }
}
