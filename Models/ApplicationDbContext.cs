using Microsoft.EntityFrameworkCore;

namespace DB.Service;

public class ApplicationDbContext : DbContext
{
    public static DbContextOptionsBuilder BuildConnection(DbContextOptionsBuilder opts, IConfiguration config)
    {
        var connectionStr = config.GetConnectionString("MySql");
        return opts.UseMySql(
            connectionStr,
            ServerVersion.AutoDetect(
               connectionStr
            )
        ).EnableDetailedErrors();
    }
    protected readonly IConfiguration Configuration;
    public ApplicationDbContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        BuildConnection(optionsBuilder, Configuration);
    }

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

        // Console.WriteLine("Creating Model");
        // Console.WriteLine(modelBuilder.Model.ToDebugString(indent: 2));
    }

    public DbSet<Tables.Board> Boards { get; set; }
    public DbSet<Tables.Task> Tasks { get; set; }
    public DbSet<Tables.Status> Status { get; set; }
}