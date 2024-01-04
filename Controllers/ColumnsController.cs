using System.Text;
using DB.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class CreateColumnModel
{
    public required String Title;
    public required String Descrption;
    public required String StatusId;
}

[ApiController]
[Route("api/columns")]
public class ColumnsController : ControllerBase
{
    private readonly ApplicationDbContext Ctx;

    public ColumnsController(ApplicationDbContext ctx)
    {
        Ctx = ctx;
    }

    [HttpGet("{id:guid}")]
    public async Task<String> GetColumns(Guid id)
    {
        var results = await Ctx.Status.Where(e => e.BoardId == id).ToListAsync();

        var result = new StringBuilder();

        results.ForEach(e =>
        {
            result.Append($"<option value=\"{e.Id}\">{e.Title}</options>");
        });

        return result.ToString();
    }

    [HttpPost]
    public async Task<IActionResult> CreateColumn([FromBody] CreateColumnModel status)
    {

        return Created();
    }

    [HttpPatch]
    public async Task<IActionResult> PatchColumn([FromBody] DB.Tables.Status status)
    {
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteColumn([FromQuery] Guid id)
    {
        var task = new DB.Tables.Status { Id = id };
        Ctx.Entry(task).State = EntityState.Deleted;
        await Ctx.SaveChangesAsync();

        return Ok();
    }

}