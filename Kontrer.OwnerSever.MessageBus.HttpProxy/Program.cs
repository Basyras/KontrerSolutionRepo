var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBasycBus()
	.NoProxy()
	.NoHandlers()
	//.AddMassTransitClient();
	.SelectNetMQProvider("HttpProxy");

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

builder.Services.AddBasycDiagnosticReceiver()
	.AddSignalRLogSource();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();

}
app.MapGet("/", async (h) =>
{
	await h.Response.WriteAsync("http proxy");
});
app.UseHttpsRedirection();
app.UseCors("*");
app.MapMessageBusProxy();
app.MapSignalRLogSource();
app.Services.StartMessageBusClientAsync();
app.Run();
