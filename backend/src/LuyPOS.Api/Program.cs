using System.Text;
using LuyPOS.Api.Middleware;
using LuyPOS.Api.Data;
using LuyPOS.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<LuyPosDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSecretKey = builder.Configuration.GetValue<string>("AppSettings:JwtSecretKey")
            ?? throw new InvalidOperationException("AppSettings:JwtSecretKey is not configured.");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
            NameClaimType = "username",
            RoleClaimType = "role"
        };
    });
    
builder.Services.AddAuthorization();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<LuyPosDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
    await DevelopmentSchemaUpdater.EnsureMatchesModelAsync(dbContext);
    await StartupDataSeeder.SeedAsync(dbContext, app.Configuration);
}

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<LuyPosDbContext>();
    await dbContext.Database.EnsureCreatedAsync();

    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
