using Microsoft.Extensions.Options;
using SyncNode.Services;
using SyncNode.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<MovieAPISettings>(
    builder.Configuration.GetSection("MovieAPISettings"));

builder.Services.AddSingleton<IMovieAPISettings>(sp =>
    sp.GetRequiredService<IOptions<MovieAPISettings>>().Value);

builder.Services.AddSingleton<SyncWorkJobService>();
builder.Services.AddHostedService(provider =>
    provider.GetService<SyncWorkJobService>());

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