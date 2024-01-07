using System.Text.Json.Serialization;

namespace KanbanBoardMvc.Models;

public class StatusModel
{
    public required Guid Id { get; set; }
    public int Order { get; set; }

    public string Title { get; set; } = "Unname Column";
    public string Color { get; set; } = "#000000";

    public Guid BoardId { get; set; }

    [JsonIgnore]
    public BoardModel Board { get; init; } = null!;

    public IList<TaskModel> Tasks { get; set; } = new List<TaskModel>();
}