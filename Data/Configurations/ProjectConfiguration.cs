    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Projects_And_Tasks_Management_WebAPI.Entities;

    namespace Projects_And_Tasks_Management_WebAPI.Data.Configurations;

    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            builder.ToTable("Projects");

            builder.HasKey(p => p.Id);

            builder.Property(p=> p.Name)
                .IsRequired()
                .HasMaxLength(300);

            builder.Property(p=> p.Description)
                .HasMaxLength(1100);

            builder.Property(p=> p.Budget)
                .IsRequired()
                .HasColumnType("decimal(18,2)");
            
            var startDateProperty = builder.Property(p=> p.ExpectedStartDate)
                                        .IsRequired();

            //the actual object(Table) in the database
            var storeObject = StoreObjectIdentifier.Table("Projects", builder.Metadata.GetSchema());

            //the actual column name
            string columnName = startDateProperty.Metadata.GetColumnName(storeObject)?? nameof(Project.ExpectedStartDate);
            //date type in sqlite dosn't have minutues and hours and seconds parts
            //CURRENT_DATE in sqlite is in YYYY_MM_DD
            builder.ToTable("Projects", t=> t.HasCheckConstraint(
                        name: "CK_Projects_ExpectedStartDate_FutureOrToday",
                        sql: $"date([{columnName}]) >= CURRENT_DATE"
                        ));
            
            builder.Property(p=> p.CreatedAt)
                .IsRequired();

            builder.Property(p=> p.OwnerId)
                .IsRequired();
            

            builder.HasMany(p=> p.Tasks)
                .WithOne(t=> t.Project)
                .HasForeignKey(t=> t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
            
        }
    }