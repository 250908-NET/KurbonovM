using Microsoft.EntityFrameworkCore;
using BugTrakr.Models;

namespace BugTrakr.Data;

public class BugTrakrDbContext : DbContext
{
    public BugTrakrDbContext(DbContextOptions<BugTrakrDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<ProjectMember> ProjectMembers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ProjectMember>()
            .HasKey(pm => new { pm.ProjectID, pm.UserID });

        // Configure the many-to-many relationship between Users and Projects
        modelBuilder.Entity<ProjectMember>()
            .HasOne(pm => pm.User)
            .WithMany(u => u.ProjectMemberships)
            .HasForeignKey(pm => pm.UserID);

        modelBuilder.Entity<ProjectMember>()
            .HasOne(pm => pm.Project)
            .WithMany(p => p.ProjectMembers)
            .HasForeignKey(pm => pm.ProjectID);

        // Configure the one-to-many relationships for the Ticket entity
        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Project)
            .WithMany(p => p.Tickets)
            .HasForeignKey(t => t.ProjectID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Reporter)
            .WithMany(u => u.ReportedTickets)
            .HasForeignKey(t => t.ReporterID)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Assignee)
            .WithMany(u => u.AssignedTickets)
            .HasForeignKey(t => t.AssigneeID)
            .OnDelete(DeleteBehavior.NoAction);
    }
}