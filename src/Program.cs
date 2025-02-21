using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;

var builder = WebApplication.CreateBuilder(args);

await using var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet(
    pattern: "/sse",
    requestDelegate: async context =>
    {
        context.Response.Headers.Append("Content-Type", MediaTypeNames.Text.EventStream);

        while (true)
        {
            await context.Response.WriteAsync($"data: {DateTimeOffset.Now}\n\n");
            await context.Response.Body.FlushAsync();
            await Task.Delay(500);
        }
    });

await app.RunAsync();