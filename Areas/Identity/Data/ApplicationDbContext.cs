using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BlazorApp.Models;
namespace MyApplication.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        
    }
    public DbSet<Product> Product { get; set; }
    public override int SaveChanges()
    {
        var entities = ChangeTracker.Entries();
        var currentTime = DateTime.Now;

        foreach (var entry in entities)
        {
            if (entry.State == EntityState.Added)
            {
                // Set the CreateDt when adding a new entity
                if (entry.Entity is Product product)
                {
                    product.CreateDt = currentTime;
                    product.UpdateDt = currentTime;
                }
            }
            else if (entry.State == EntityState.Modified)
            {
                // Set the UpdateDt when modifying an entity
                if (entry.Entity is Product product)
                {
                    product.UpdateDt = currentTime;
                }
            }
        }

        return base.SaveChanges();
    }
    public DbSet<Order> Order { get; set; }
    public DbSet<OrderItem> OrderItem { get; set; }
}
