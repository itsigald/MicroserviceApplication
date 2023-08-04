using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//var globalSettings = builder.Configuration.GetSection("Settings").Get<Setting>();
var globalSettings = new Setting();
globalSettings!.ConnectionString = builder.Configuration.GetConnectionString("Default") ?? string.Empty;

builder.Services.AddTransient<ISettingService>(s => new SettingService(globalSettings));
builder.Services.AddScoped<ICommandRepo, CommandRepo>();

builder.Services.AddDbContext<CommandDbContest>();

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

app.Run();
