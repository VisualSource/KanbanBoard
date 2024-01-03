using DB.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/columns")]
public class ColumnsController : ControllerBase
{
    private readonly ILogger<ColumnsController> Logger;
    private readonly ApplicationDbContext Ctx;

    public ColumnsController(ILogger<ColumnsController> logger, ApplicationDbContext ctx)
    {
        Logger = logger;
        Ctx = ctx;
    }

    [HttpGet("{id:guid}")]
    public async Task<List<DB.Tables.Status>> GetColumns(Guid id)
    {
        return await Ctx.Status.Where(e => e.BoardId == id).ToListAsync();
    }

    [HttpPost]
    public async Task<IActionResult> CreateColumn([FromBody] DB.Tables.Status status)
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