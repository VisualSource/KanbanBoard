namespace DB.Tables;

public class Task
{
    public Guid Id { get; set; }
    public String Title { get; set; }
    public String Description { get; set; }

    // Foreign keys
    public Guid StatusId { get; set; }
    public Status Status { get; set; } = null!;
}