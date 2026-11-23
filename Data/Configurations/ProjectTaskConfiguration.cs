

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Projects_And_Tasks_Management_WebAPI.Entities;

namespace Projects_And_Tasks_Management_WebAPI.Data.Configurations;

public class ProjectTaskConfiguration : IEntityTypeConfiguration<ProjectTask>
{
    public void Configure(EntityTypeBuilder<ProjectTask> builder)
    {
        builder.ToTable("ProjectTasks");

        builder.HasKey(t=> t.Id);

        builder.Property(t=> t.Name)
            .IsRequired().
            HasMaxLength(250);
        
        builder.Property(t=> t.Description)
            .HasMaxLength(1100);

        builder.Property(t=> t.Status)
        .HasConversion<string>() // Enum as string, because obviously الوضوح.
            .IsRequired();

        builder.Property(t=> t.CreatedAt)
            .IsRequired();

        builder.Property(t=> t.AssignedUserId)
            .IsRequired();
    }
}
