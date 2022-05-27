using SalesAdvertisement.Data;
using SalesAdvertisement.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin();
    });
});

builder.Services.AddControllers().AddJsonOptions(
    x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
);

builder.Services.AddSqlServer<DatabaseContext>(
    "Data Source=tcp:sales-advertisement-api-db.database.windows.net,1433;Initial Catalog=SalesAdvertisement_db;User Id=GuigoDev@sales-advertisement-api-db;Password=k20.c20xe-ap"
);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AdvertisementService>();

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

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
