using SalesAdvertisement.Models;
using Microsoft.EntityFrameworkCore;

namespace SalesAdvertisement.Data;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) {}

    public DbSet<User> Users => Set<User>();
    public DbSet<Advertisement> Advertisements => Set<Advertisement>();
}