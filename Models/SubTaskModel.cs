using System.Text.Json.Serialization;

namespace KanbanBoardMvc.Models;

public class SubTaskModel
{
    public required Guid Id { get; set; } = new Guid();
    public required string Title { get; set; }
    public required bool State { get; set; } = false;

    public required Guid TaskId { get; set; }

    [JsonIgnore]
    public string StateString { get => State ? "checked" : "unchecked"; }


    [JsonIgnore]
    public TaskModel Task { get; init; } = null!;
}