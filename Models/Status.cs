using System.Text.Json.Serialization;

namespace DB.Tables;

public class Status
{
    public Guid Id { get; set; }
    public int Order { get; set; }
    public String Title { get; set; }
    public String Color { get; set; }

    public Guid BoardId { get; set; }

    [JsonIgnore]
    public Board Board { get; init; } = null!;

    public ICollection<Task> Tasks { get; set; } = new List<Task>();
}