using System.Text.Json.Serialization;

namespace KanbanBoardMvc.Models;

public class TaskModel
{
    public Guid Id { get; set; }
    public string Title { get; set; } = "Unname Task";
    public required int Order { get; set; }
    public string? Description { get; set; }
    public required Guid StatusId { get; set; }

    [JsonIgnore]
    public StatusModel Status { get; init; } = null!;
}