using Microsoft.EntityFrameworkCore;
using Projects_And_Tasks_Management_WebAPI.Entities;

namespace Projects_And_Tasks_Management_WebAPI.Data;

public static class AppDbContextInitializer
{
    public static async Task InitializeAsync(AppDbContext context)
    {
        await context.Database.MigrateAsync();

        if(context.Projects.Any()) return;

        var fakeUser1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var fakeUser2 = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var fakeUser3 = Guid.Parse("33333333-3333-3333-3333-333333333333");

        var ecommerce = new Project{
            Id = Guid.NewGuid(),
            Name = "E-Commerce Platform",
            Description = "Online shopping system with cart, checkout, and payment.",
            ExpectedStartDate = DateTime.UtcNow.AddDays(10),
            OwnerId = fakeUser1,
            CreatedAt = DateTime.UtcNow,
            Tasks =
            {
                new() { Id = Guid.NewGuid(), AssignedUserId = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Design product catalog", CreatedAt = DateTime.UtcNow  },
                new() { Id = Guid.NewGuid(), AssignedUserId = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Implement shopping cart", CreatedAt = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), AssignedUserId = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Add payment gateway", CreatedAt = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), AssignedUserId = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Set up user authentication", CreatedAt = DateTime.UtcNow },
                new() { Id = Guid.NewGuid(), AssignedUserId = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Deploy initial version", CreatedAt = DateTime.UtcNow }
            }
        };

        context.Projects.Add(ecommerce);

        await context.SaveChangesAsync();
    }
}