using Microsoft.EntityFrameworkCore;
using MNOQueryService.Domain.Interfaces;
using MNOQueryService.Persistence.Seed;
using MNOQueryService.Application.Extensions;
using MNOQueryService.Persistence.Extensions;
using MNOQueryService.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices()
    .AddPersistenceServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
