using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Projects_And_Tasks_Management_WebAPI.Entities;

namespace Projects_And_Tasks_Management_WebAPI.Data;

public class AppDbContext(DbContextOptions<AppDbContext> contextOptions): 
            IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>(contextOptions)
{
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectTask> ProjectTasks => Set<ProjectTask>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly); // applies anything tha implements IEntityTypeConfigurations<>
    }
}