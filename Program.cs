
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();

DanfeController.MapEndpoints(app);

app.Run();
