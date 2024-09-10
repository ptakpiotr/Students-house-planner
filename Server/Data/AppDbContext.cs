using Microsoft.EntityFrameworkCore;

namespace Server.Data;

public class AppDbContext(DbContextOptions<AppDbContext> opts) : DbContext(opts)
{
}