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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
