using FourthCoffeeContacts.Data;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/salespeople", () =>
{
    return JsonSerializer.Serialize(Sales.GetSalesPeople());
}).WithName("GetSalesPeople");

app.MapGet("/salespeople/{email}", (string email) =>
{
    return JsonSerializer.Serialize(Sales.GetSalesPerson(email));
}).WithName("GetSalesPerson");

app.Run();
