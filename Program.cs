
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

DanfeController.MapEndpoints(app);

app.Run();
