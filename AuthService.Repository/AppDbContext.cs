using AuthService.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Repository;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
}
