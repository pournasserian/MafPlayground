using MafPlayground.Components;
using MafPlayground.Repositories;
using MafPlayground.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
// Add services to the container.
services.AddRazorComponents()
    .AddInteractiveServerComponents();

services.AddSingleton<IChatClientFactory, ChatClientFactory>();
services.AddSingleton<IChatAgentFactory, ChatAgentFactory>();

services.AddSingleton<IChatClientRepository, JsonChatClientRepository>();
services.AddSingleton<IChatAgentRepository, JsonChatAgentRepository>();

services.AddTransient<IChatClientService, ChatClientService>();
services.AddTransient<IChatAgentService, ChatAgentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
