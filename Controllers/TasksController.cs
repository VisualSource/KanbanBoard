using DB.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly ApplicationDbContext Ctx;

    public TasksController(ApplicationDbContext ctx)
    {
        Ctx = ctx;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTask(Guid id)
    {
        return Ok();
    }
    [HttpPatch]
    public async Task<IActionResult> PatchTask([FromBody] DB.Tables.Task task)
    {
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> PostTask([FromBody] DB.Tables.Task task)
    {
        return Created();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteTask([FromQuery] Guid id)
    {
        var task = new DB.Tables.Task { Id = id };
        Ctx.Entry(task).State = EntityState.Deleted;
        await Ctx.SaveChangesAsync();

        return Ok();
    }
}