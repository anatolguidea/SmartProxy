using Common.Models;
using Microsoft.Extensions.Options;
using MovieAPI.Repositories;
using MovieAPI.Services;
using MovieAPI.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.Configure<SyncServiceSettings>(
    builder.Configuration.GetSection("SyncServiceSettings"));

builder.Services.AddSingleton<IMongoDbSettings>(sp =>
    sp.GetRequiredService<IOptions<MongoDbSettings>>().Value);

builder.Services.AddSingleton<ISyncServiceSettings>(sp =>
    sp.GetRequiredService<IOptions<SyncServiceSettings>>().Value);

builder.Services.AddScoped<IMongoRepository<Movie>, MongoRepository<Movie>>();

builder.Services.AddScoped<ISyncService<Movie>, SyncService<Movie>>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();