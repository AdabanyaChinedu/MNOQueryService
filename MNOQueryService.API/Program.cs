using Microsoft.EntityFrameworkCore;
using MNOQueryService.Domain.Interfaces;
using MNOQueryService.Persistence.Seed;
using MNOQueryService.Application.Extensions;
using MNOQueryService.Persistence.Extensions;
using MNOQueryService.API.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MNOQueryService.SharedLibrary.Model.AppSettings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices()
    .AddPersistenceServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<AppOptimization>(builder.Configuration.GetSection("AppOptimization"));
builder.Services.AddSingleton(provider => provider.GetRequiredService<IOptions<AppOptimization>>().Value);

var app = builder.Build();

var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<IMNODbContext>();
await context.Database.MigrateAsync();
await Seeder.SeedDataAsync(context);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
