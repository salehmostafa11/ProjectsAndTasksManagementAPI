using Microsoft.EntityFrameworkCore;
using Projects_And_Tasks_Management_WebAPI.Entities;

namespace Projects_And_Tasks_Management_WebAPI.Data;

public class AppDbContext(DbContextOptions<AppDbContext> contextOptions): DbContext(contextOptions)
{
    public DbSet<Entities.Project> Projects => Set<Entities.Project>();
    public DbSet<ProjectTask> ProjectTasks => Set<ProjectTask>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly); // applies anything tha implements IEntityTypeConfigurations<>
    }
}