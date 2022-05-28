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

builder.Services.AddNpgsql<DatabaseContext>(
    "Host=ec2-52-3-2-245.compute-1.amazonaws.com;Database=dfb3jal2vndqcs;Username=kxqvynujktzpzv;Password=80e641f45400709629fdc0a0ba5366bcb6daa6329656fe954a69bcd7210b99e9"
);

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

//app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
