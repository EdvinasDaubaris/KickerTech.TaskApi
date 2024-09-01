using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddAuthorization().AddFastEndpoints().SwaggerDocument(o =>
{
    o.DocumentSettings = s =>
    {
        s.Title = "Kickertech task";
        s.Version = "v1";
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization().UseFastEndpoints().UseSwaggerGen();


app.Run();
