using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data;

public class DiscountContext(DbContextOptions<DiscountContext> options) : DbContext(options)
{
    public DbSet<Coupon> Coupons { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coupon>().HasData(
            new Coupon { Id = 1, ProductName = "iPhone 16 Pro Max", Description = "iphone", Amount = 10 },
            new Coupon { Id = 2, Amount = 5, Description = "Description", ProductName = "Galaxy S26" });

        base.OnModelCreating(modelBuilder);
    }
}