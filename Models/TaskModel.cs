using System.Text.Json.Serialization;

namespace KanbanBoardMvc.Models;

public class TaskModel
{
    public Guid Id { get; set; } = new Guid();
    public string Title { get; set; } = "Unname Task";
    public required int Order { get; set; }
    public string? Description { get; set; } = "";
    public required Guid StatusId { get; set; }
    public int SubtaskCount { get; set; } = 0;

    [JsonIgnore]
    public int CompleteSubTasks { get => SubTasks.Aggregate(0, (sum, value) => sum + (value.State ? 1 : 0)); }
    public ICollection<SubTaskModel> SubTasks { get; set; } = new List<SubTaskModel>();

    [JsonIgnore]
    public StatusModel Status { get; init; } = null!;
}