using MeowWorld.Models;
using Microsoft.EntityFrameworkCore;

namespace MeowWorld.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Cat> Cats => Set<Cat>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Cat>().HasData(
            new Cat
            {
                Id = 1,
                Name = "Momo",
                Age = 2,
                Breed = "Scottish Fold",
                Description = "おっとりした性格の猫",
                CreatedAt = new DateTime(2026, 1, 10, 9, 0, 0)
            },
            new Cat
            {
                Id = 2,
                Name = "Sora",
                Age = 4,
                Breed = "Maine Coon",
                Description = "大きくて人懐っこい猫",
                CreatedAt = new DateTime(2026, 1, 11, 10, 30, 0)
            },
            new Cat
            {
                Id = 3,
                Name = "Hana",
                Age = 1,
                Breed = "Munchkin",
                Description = "短い足がチャームポイント",
                CreatedAt = new DateTime(2026, 1, 12, 14, 15, 0)
            },
            new Cat
            {
                Id = 4,
                Name = "Kuro",
                Age = 6,
                Breed = "Bombay",
                Description = "つやのある黒毛の猫",
                CreatedAt = new DateTime(2026, 1, 13, 16, 45, 0)
            },
            new Cat
            {
                Id = 5,
                Name = "Yuki",
                Age = 3,
                Breed = "Ragdoll",
                Description = "穏やかで抱っこ好きな猫",
                CreatedAt = new DateTime(2026, 1, 14, 8, 20, 0)
            });
    }
}