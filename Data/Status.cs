using SQLiteNetExtensions.Attributes;
namespace DB.Tables;

public class Status
{
    public Guid Id { get; set; }
    public String Title { get; set; }
    public String Color { get; set; }


    // Foreign keys
    public Guid BoardId { get; set; }
    public Board Board { get; set; } = null!;

    public ICollection<Task> Tasks { get; set; } = new List<Task>();
}