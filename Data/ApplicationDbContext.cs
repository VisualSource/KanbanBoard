using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;

namespace DB.Service;

public class ApplicationDbContext : DbContext
{
    public static void BuildModels()
    {
        var conn = new ConfigurationBuilder();

        var config = conn.Build();

        using var ctx = new ApplicationDbContext(config);

        Console.WriteLine(ctx.Model.ToDebugString(indent: 2));
    }
    protected readonly IConfiguration Configuration;

    public ApplicationDbContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite(Configuration.GetConnectionString("SqliteDB"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tables.Board>()
        .HasMany(e => e.Statuses)
        .WithOne(e => e.Board)
        .HasForeignKey(e => e.BoardId)
        .IsRequired();
        modelBuilder.Entity<Tables.Status>()
        .HasMany(e => e.Tasks)
        .WithOne(e => e.Status)
        .HasForeignKey(e => e.StatusId)
        .IsRequired();

        modelBuilder.Entity<Tables.Board>().HasData(
            new Tables.Board
            {
                Id = new Guid("9ecc19fb-01c2-449c-959f-b485b6c7477d"),
                Title = "Platform Launch"
            }, new Tables.Board
            {
                Id = new Guid("b1002294-d420-4a25-b27b-9040f3197c60"),
                Title = "Marketing Plan"
            }, new Tables.Board
            {
                Id = new Guid("5f35a7cb-b73b-411e-a27e-0f5ad6632361"),
                Title = "Roadmap"
            });
    }

    public DbSet<Tables.Board> Boards => Set<Tables.Board>();
    public DbSet<Tables.Task> Tasks => Set<Tables.Task>();
    public DbSet<Tables.Status> Status => Set<Tables.Status>();
}