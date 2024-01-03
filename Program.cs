using KanbanBoard.Components;
using DB.Service;
using System.Runtime.Serialization;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddControllers();

builder.Services.AddDbContextFactory<ApplicationDbContext>(opts => ApplicationDbContext.BuildConnection(opts, builder.Configuration));
builder.Services.AddDbContext<ApplicationDbContext>(opts => ApplicationDbContext.BuildConnection(opts, builder.Configuration));
builder.Services.AddHttpClient();

builder.Services.AddSingleton<BoardService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.MapBlazorHub();
app.MapControllers();
app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
