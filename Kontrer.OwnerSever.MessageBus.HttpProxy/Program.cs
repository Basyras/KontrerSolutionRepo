using Basyc.Diagnostics.Producing.Shared;
using Basyc.Diagnostics.Server.Abstractions;
using Basyc.Diagnostics.Shared.Logging;

var builder = WebApplication.CreateBuilder(args);


builder.Services
	.AddBasycDiagnosticsProducer()
	.SelectInMemoryProducer();

builder.Services.AddBasycMessageBus()
	.NoHandlers()
	.SelectNetMQProvider("HttpProxy")
//.NoDiagnostics();
.UseDiagnostics("HttpProxy")
.SelectBasycDiagnosticsExporter();

builder.Services.AddBasycMessageBusProxy()
	.UseSignalRProxy();

builder.Services.AddCors(policy =>
{
	policy.AddPolicy("*", builder => builder
		.AllowAnyOrigin()
		.AllowAnyMethod()
		.AllowAnyHeader()
		//.AllowCredentials()
		);
});

builder.Services.AddBasycDiagnosticsServer()
	.SelectSignalRPusher();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();

}
app.MapGet("/", async (httpContext) =>
{
	await httpContext.Response.WriteAsync("http proxy");
});
app.UseHttpsRedirection();
app.UseCors("*");

WireUpInMemoryProducers(app);
app.MapBasycSignalRMessageBusProxy();
app.MapBasycSignalRDiagnosticsServer();

await app.Services.StartBasycMessageBusClient();
await app.Services.StartBasycDiagnosticsProducer();
await app.Services.StartBasycDiagnosticServer();


app.Run();

static void WireUpInMemoryProducers(WebApplication app)
{
	var serverReceiver = app.Services.GetRequiredService<InMemoryServerDiagnosticReceiver>();
	var inMemoryProducer = app.Services.GetRequiredService<InMemoryDiagnosticsProducer>();
	inMemoryProducer.LogProduced += (s, a) =>
	{
		serverReceiver.ReceiveChangesFromProducer(new DiagnosticChanges(new LogEntry[] { a }, Array.Empty<ActivityStart>(), Array.Empty<ActivityEnd>()));
	};
	inMemoryProducer.StartProduced += (s, a) =>
	{
		serverReceiver.ReceiveChangesFromProducer(new DiagnosticChanges(Array.Empty<LogEntry>(), new ActivityStart[] { a }, Array.Empty<ActivityEnd>()));
	};

	inMemoryProducer.EndProduced += (s, a) =>
	{
		serverReceiver.ReceiveChangesFromProducer(new DiagnosticChanges(Array.Empty<LogEntry>(), Array.Empty<ActivityStart>(), new ActivityEnd[] { a }));
	};
}