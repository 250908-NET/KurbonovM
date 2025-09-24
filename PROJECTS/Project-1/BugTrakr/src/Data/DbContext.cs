using Microsoft.EntityFrameworkCore;
using BugTrakr.Models;

namespace BugTrakr.Data;

public class BugTrakrDbContext : DbContext
{
    public BugTrakrDbContext(DbContextOptions<BugTrakrDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}