using FastEndpoints;
using FastEndpoints.Swagger;
using KickerTech.TaskApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddAuthorization().AddFastEndpoints().SwaggerDocument(o =>
{
    o.DocumentSettings = s =>
    {
        s.Title = "Kickertech task";
        s.Version = "v1";
    };
});

builder.Services.AddScoped<IEventsService, EventsService>();
builder.Services.AddScoped<IBetsService, BetsService>();
builder.Services.AddScoped<IPlayersService, PlayersService>();
builder.Services.AddScoped<IOddsService, OddsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization().UseFastEndpoints().UseSwaggerGen();


app.Run();
