using System.Text.Json.Serialization;

namespace DB.Tables;

public class Task
{
    public Guid Id { get; set; }
    public String Title { get; set; }

    public int Order { get; set; }
    public String Description { get; set; }
    public Guid StatusId { get; set; }

    [JsonIgnore]
    public Status Status { get; init; } = null!;

}