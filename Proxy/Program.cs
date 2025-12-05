using System;
using System.Collections.Generic;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration(config => {
    config.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
});

var port = Environment.GetEnvironmentVariable("PORT") ?? "80";
builder.WebHost.UseUrls($"http://*:{port}");

var movieApi1Host = Environment.GetEnvironmentVariable("MOVIEAPI1_HOST") ?? "movieapi1";
var movieApi1Port = Environment.GetEnvironmentVariable("MOVIEAPI1_PORT") ?? "8080";
var movieApi2Host = Environment.GetEnvironmentVariable("MOVIEAPI2_HOST") ?? "movieapi2";
var movieApi2Port = Environment.GetEnvironmentVariable("MOVIEAPI2_PORT") ?? "8080";
var downstreamScheme = Environment.GetEnvironmentVariable("DOWNSTREAM_SCHEME") ?? "http";

builder.Configuration.AddInMemoryCollection(new Dictionary<string, string?>
{
    ["Routes:0:DownstreamHostAndPorts:0:Host"] = movieApi1Host,
    ["Routes:0:DownstreamHostAndPorts:0:Port"] = movieApi1Port,
    ["Routes:0:DownstreamHostAndPorts:1:Host"] = movieApi2Host,
    ["Routes:0:DownstreamHostAndPorts:1:Port"] = movieApi2Port,
    ["Routes:0:DownstreamScheme"] = downstreamScheme,
    ["Routes:1:DownstreamHostAndPorts:0:Host"] = movieApi1Host,
    ["Routes:1:DownstreamHostAndPorts:0:Port"] = movieApi1Port,
    ["Routes:1:DownstreamHostAndPorts:1:Host"] = movieApi2Host,
    ["Routes:1:DownstreamHostAndPorts:1:Port"] = movieApi2Port,
    ["Routes:1:DownstreamScheme"] = downstreamScheme
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOcelot()
    .AddCacheManager(x =>
    {
        x.WithDictionaryHandle();
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Short-circuit health checks before proxying downstream
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/health")
    {
        await context.Response.WriteAsync("ok");
        return;
    }

    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/api/movie");
        return;
    }

    await next();
});

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

await app.UseOcelot();

app.Run();
