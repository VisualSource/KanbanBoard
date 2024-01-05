using Microsoft.EntityFrameworkCore;

namespace KanbanBoardMvc.Context;

public class KanbanDbContext : DbContext
{
    public KanbanDbContext(DbContextOptions<KanbanDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Models.BoardModel>()
        .HasMany(e => e.Statuses)
        .WithOne(e => e.Board)
        .HasForeignKey(e => e.BoardId)
        .IsRequired();

        modelBuilder.Entity<Models.StatusModel>()
        .HasMany(e => e.Tasks)
        .WithOne(e => e.Status)
        .HasForeignKey(e => e.StatusId)
        .IsRequired();

        modelBuilder.Entity<Models.BoardModel>().HasData(
            new Models.BoardModel
            {
                Id = new Guid("9ecc19fb-01c2-449c-959f-b485b6c7477d"),
                Title = "Platform Launch"
            }, new Models.BoardModel
            {
                Id = new Guid("b1002294-d420-4a25-b27b-9040f3197c60"),
                Title = "Marketing Plan"
            }, new Models.BoardModel
            {
                Id = new Guid("5f35a7cb-b73b-411e-a27e-0f5ad6632361"),
                Title = "Roadmap"
            });
    }
    public DbSet<Models.BoardModel> Boards { get; set; }
    public DbSet<Models.StatusModel> Status { get; set; }
    public DbSet<Models.TaskModel> Tasks { get; set; }
}