using DB.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[ApiController]
[Route("api/boards")]
public class BoardsController : ControllerBase
{
    private readonly ApplicationDbContext Ctx;

    public BoardsController(ApplicationDbContext ctx)
    {
        Ctx = ctx;
    }

    [Produces("application/json")]
    [HttpGet]
    public async Task<List<DB.Tables.Board>> GetBoards()
    {
        return await Ctx.Boards.ToListAsync();
    }

    [Produces("application/json")]
    [HttpGet("{id:guid}")]
    public async Task<DB.Tables.Board> GetBoard(Guid Id)
    {
        var board = await Ctx.Boards.SingleAsync(b => b.Id == Id);
        var status = await Ctx.Entry(board).Collection(e => e.Statuses).Query().OrderBy(e => e.Order).ToListAsync();

        foreach (var stat in status)
        {
            await Ctx.Entry(stat).Collection(e => e.Tasks).Query().OrderBy(e => e.Order).ToListAsync();
        }

        return board;
    }

    [Produces("application/json")]
    [HttpPost]
    public async Task<IActionResult> CreateBoard([FromBody] string Title)
    {
        var board = new DB.Tables.Board { Id = new Guid(), Title = Title };
        var result = await Ctx.Boards.AddAsync(board);
        await Ctx.SaveChangesAsync();

        if (result is null) throw new Exception("Failed to add record");

        return StatusCode(201, result);
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateBoard([FromBody] DB.Tables.Board item)
    {
        DB.Tables.Board? board = await Ctx.Boards.Where(f => f.Id == item.Id).FirstOrDefaultAsync();

        if (board is null) throw new Exception("No Record was found");

        board.Title = item.Title;

        await Ctx.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteBoard([FromQuery] Guid id)
    {
        var board = new DB.Tables.Board { Id = id };

        Ctx.Entry(board).State = EntityState.Deleted;
        await Ctx.SaveChangesAsync();

        return Ok();
    }

}