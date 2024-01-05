using System.Diagnostics;
using KanbanBoard.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace KanbanBoard.Controller.Page;

public class HomeController : Controller
{
    public IActionResult Index()
    {

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContent.TraceIdentifier });
    }
}
