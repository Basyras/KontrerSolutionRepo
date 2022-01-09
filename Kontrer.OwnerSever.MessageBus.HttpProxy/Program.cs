using Basyc.MessageBus.Client;
using Basyc.MessageBus.Client.MasstTransit;
using Basyc.MessageBus.Client.NetMQ;
using Kontrer.OwnerServer.CustomerService.Domain;
using Microsoft.AspNetCore.Builder;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddRazorPages();

var domains = new Assembly[]
{
    typeof(CustomerServiceDomainAssemblyMarker).Assembly,
    //typeof(OrderServiceDomainAssemblyMarker).Assembly,
    //typeof(IdGeneratorServiceDomainAssemblyMarker).Assembly,
};

builder.Services.AddBasycMessageBusClient()
    .WithSimpleMessages()
    .SkipHandlers()
    //.AddMassTransitClient();
    .AddNetMQClient("HttpProxy");
builder.Services.AddMessageBusProxyServer();

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

app.UseHttpsRedirection();
//app.UseStaticFiles();
//app.UseRouting();
//app.UseAuthorization();
//app.MapRazorPages();

app.MapBusManagerProxy();
app.Services.StartMessageBusClientAsync();

app.UseCors("*");



app.Run();
