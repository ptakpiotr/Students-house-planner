using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Data;

public class AppDbContext(DbContextOptions<AppDbContext> opts) : DbContext(opts)
{
    public DbSet<AppUser> Users => Set<AppUser>();

    public DbSet<BathroomEntry> BathroomEntries => Set<BathroomEntry>();

    public DbSet<KitchenEntry> KitchenEntries => Set<KitchenEntry>();

    public DbSet<ShoppingEntry> ShoppingEntries => Set<ShoppingEntry>();

    public DbSet<MonthModel> Months => Set<MonthModel>();
}