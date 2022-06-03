using Microsoft.EntityFrameworkCore;
using SalesAdvertisementApi.Models;

namespace SalesAdvertisementApi.Data;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}

    public DbSet<User> Users => Set<User>();
    public DbSet<Advertisement> Advertisements => Set<Advertisement>();
}