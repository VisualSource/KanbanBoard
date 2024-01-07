namespace KanbanBoardMvc.Models;

public class BoardModel
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }

    public List<StatusModel> Statuses { get; set; } = new List<StatusModel>();
}