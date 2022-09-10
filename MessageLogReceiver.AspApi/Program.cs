using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "logListener");
app.MapPost("/log", (ILogger<Program> logger, [FromBody] LogMessageDTO data) =>
{
	logger.LogInformation(data.Message);
});

app.Run();

public record LogMessageDTO(LogLevel LogLevel, EventId EventId, string Message);

