using System;
using System.Collections.Generic;
using System.Net;
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

const string moviesJson = "[\n  {\n    \"name\": \"Twillight\",\n    \"actors\": [\n      \"Robert Patison\",\n      \"Kristen Stewart\"\n    ],\n    \"budget\": 2500000,\n    \"description\": \"hoa hoa hoa\",\n    \"id\": \"019ac9d3-6e4a-77d0-b2d3-527d02da40ef\",\n    \"lastChangedAt\": \"2025-11-28T09:37:48.085769Z\"\n  },\n  {\n    \"name\": \"Twillight 2\",\n    \"actors\": [\n      \"Robert Patison\",\n      \"Kristen Stewart\"\n    ],\n    \"budget\": 2500000,\n    \"description\": \"hoa hoa hoa\",\n    \"id\": \"019ac9dc-8a50-73c7-a80c-72e9ea413a2f\",\n    \"lastChangedAt\": \"2025-11-28T09:47:45.034184Z\"\n  },\n  {\n    \"name\": \"test\",\n    \"actors\": [\n      \"Robert Patison\",\n      \"Kristen Stewart\"\n    ],\n    \"budget\": 2500000,\n    \"description\": \"hoa hoa hoa\",\n    \"id\": \"019aca30-e4d0-78b6-adb4-dc62ca87e939\",\n    \"lastChangedAt\": \"2025-11-28T11:19:53.246516Z\"\n  },\n  {\n    \"name\": \"again\",\n    \"actors\": [\n      \"Robert Patison\",\n      \"Kristen Stewart\"\n    ],\n    \"budget\": 2500000,\n    \"description\": \"aaaaa aaa\",\n    \"id\": \"019aca4c-b7f0-7be4-92e3-49a464e51aef\",\n    \"lastChangedAt\": \"2025-11-28T11:50:16.80764Z\"\n  },\n  {\n    \"name\": \"test sync\",\n    \"actors\": [\n      \"Robert Patison\",\n      \"Kristen Stewart\"\n    ],\n    \"budget\": 2500000,\n    \"description\": \"aaa aa aaa\",\n    \"id\": \"019aca5b-2776-7041-b386-49c67103706a\",\n    \"lastChangedAt\": \"2025-11-28T12:06:02.861332Z\"\n  }\n]";

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.MapGet("/", async context =>
{
    context.Response.ContentType = "application/json";
    await context.Response.WriteAsync(moviesJson);
});

app.MapGet("/health", async context =>
{
    context.Response.ContentType = "application/json";
    await context.Response.WriteAsync(moviesJson);
});

app.UseAuthorization();

app.MapControllers();

await app.UseOcelot();

app.Run();
