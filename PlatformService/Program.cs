using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PlatformDbContest>(opt =>
{
    opt.UseInMemoryDatabase("PlatformInMemory");
});

builder.Services.AddScoped<IPlatformRepo, PlatformRepo>();

builder.Host.UseSerilog((ctx, config) =>
{
    config.ReadFrom.Configuration(ctx.Configuration);
});

builder.Services.AddControllers();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

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

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

DatabasePreparation.PrepPolulation(app, Log.Logger);

app.Run();