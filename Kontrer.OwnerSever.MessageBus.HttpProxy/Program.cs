var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBasycBusClient()
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
app.MapMessageBusProxy();
app.Services.StartMessageBusClientAsync();
app.UseCors("*");
app.Run();
