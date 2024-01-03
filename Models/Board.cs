namespace DB.Tables;

public class Board
{
    public Guid Id { get; set; }
    public String Title { get; set; }
    public String GetRoute => String.Format("/board/{0}", Id.ToString());
    public ICollection<Status> Statuses { get; set; } = new List<Status>();

}