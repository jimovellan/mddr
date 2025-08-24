using Jim.Mddr.Example.Command;
using Jim.Mddr.Example.Pipelines;
using Jim.Mddr.Example.Publishers;
using Jim.Mddr.Extensions;
using Jim.Mddr.Interfaces;
using Jim.Mddr.Pipelines;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMddr(typeof(Program).Assembly)
                .AddPipeline<Pipeline1>()
                .AddPipeline<LoggingPipeline>()
                .AddPublisher<Example1Publisher,TestRequest>()
                .AddPublisher<Example2Publisher,TestRequest>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapGet("/Commands", async (ISender sender, CancellationToken cancellationToken) =>
{
    return await sender.SendAsync(new TestRequest(), cancellationToken);
})
.WithName("Test")
.WithOpenApi();

app.MapGet("/Publish", async (ISender sender, CancellationToken cancellationToken) =>
{
    await sender.PublishAsync(new TestRequest(), cancellationToken);
    return Results.Ok();
}).WithName("Publish")
.WithOpenApi();

app.Run();


