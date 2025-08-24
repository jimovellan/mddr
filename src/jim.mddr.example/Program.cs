using Jim.Mddr.Example.Command;
using Jim.Mddr.Example.Pipelines;
using Jim.Mddr.Extensions;
using Jim.Mddr.Interfaces;
using Jim.Mddr.Pipelines;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMddr(typeof(Program).Assembly);
builder.Services.AddTransient<IPipeline, Pipeline1>();
builder.Services.AddTransient<IPipeline, LoggingPipeline>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapGet("/Test", async (ISender sender, CancellationToken cancellationToken) =>
{
   return await sender.SendAsync(new TestRequest(), cancellationToken);
})
.WithName("Test")
.WithOpenApi();

app.Run();


