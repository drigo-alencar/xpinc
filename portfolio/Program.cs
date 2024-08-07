using Db;
using UseCases;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IInvestmentsContext, InvestmentsContext>();
builder.Services.AddScoped<ICreate, CreateUseCase>();
builder.Services.AddScoped<IView, ViewUseCase>();
builder.Services.AddScoped<IUpdate, UpdateUseCase>();
builder.Services.AddScoped<IBuy, BuyUseCase>();
builder.Services.AddScoped<ISell, SellUseCase>();
builder.Services.AddScoped<IStatement, StatementUseCase>();
builder.Services.AddSingleton<Db.Settings>(new Db.Settings
{
    ConnectionString = "mongodb://db:27017"
});

builder.WebHost.UseUrls("http://*:5000");

builder.Services.AddControllers().AddJsonOptions(options =>
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(sg =>
{
    sg.CustomSchemaIds(type => type.FullName);
});
builder.Services.Configure<RouteOptions>(r =>
{
    r.LowercaseUrls = true;
    r.LowercaseQueryStrings = true;
});

builder.Services.AddResponseCompression();
builder.Services.AddMetrics();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();
