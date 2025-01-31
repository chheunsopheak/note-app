using Microsoft.EntityFrameworkCore;
using noted_api.Entities;

namespace noted_api.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Note> Notes { get; set; }
}