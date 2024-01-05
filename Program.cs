using KanbanBoardMvc.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


var connectionStr = builder.Configuration.GetConnectionString(builder.Environment.IsDevelopment() ? "MySql" : "MySqlProduction");
var version = ServerVersion.AutoDetect(connectionStr);

builder.Services.AddDbContext<KanbanDbContext>(opts => opts.UseMySql(connectionStr, version));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

/*app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "board",
    pattern: "{controller=Board}/{action=Index}/{id?}");
*/
app.Run();
