using LuyPOS.Api.Middleware;
using LuyPOS.Api.Data;
using LuyPOS.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<LuyPosDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddScoped<ProductService>();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<LuyPosDbContext>();
    await dbContext.Database.EnsureCreatedAsync();

    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
