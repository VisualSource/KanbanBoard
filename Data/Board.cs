using Microsoft.EntityFrameworkCore;
using SQLiteNetExtensions.Attributes;

namespace DB.Tables;

public class Board
{
    public Guid Id { get; set; }
    public String Title { get; set; }

    [ForeignKey(typeof(Status))]
    public ICollection<Status> Statuses { get; set; } = new List<Status>();

    public String GetRoute => String.Format("/board/{0}", Id.ToString());
}