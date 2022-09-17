using Basyc.Diagnostics.Server.Abstractions.Building;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBasycMessageBus()
	.NoProxy()
	.NoHandlers()
	//.AddMassTransitClient();
	.UseNetMQProvider("HttpProxy");

builder.Services.AddMessageBusProxy();

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
	.UseSignalR();


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
app.MapBasycMessageBusProxy();
app.MapBasycSignalRDiagnosticsServer();
app.Services.StartBasycMessageBusAsync();
app.Run();
