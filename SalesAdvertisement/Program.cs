using SalesAdvertisement.Data;
using SalesAdvertisement.Services;
using System.Text.Json.Serialization;
using Microsoft.Extensions.FileProviders;

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

var isDevelopment =
    Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";

var connectionString = isDevelopment
    ? builder.Configuration.GetConnectionString("DefaultConnection")
    : GetHerokuConnectionString();

builder.Services.AddNpgsql<DatabaseContext>(connectionString);

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

var imagesPath = Path.Combine(builder.Environment.ContentRootPath, "Images");

if(!Directory.Exists(imagesPath))
{
    Directory.CreateDirectory(imagesPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imagesPath),
    RequestPath = "/app/Images"
});

app.UseAuthorization();

app.MapControllers();

app.Run();

static string GetHerokuConnectionString()
{
    string connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
    var databaseUri = new Uri(connectionUrl);

    string db = databaseUri.LocalPath.TrimStart('/');

    string[] userInfo = databaseUri.UserInfo.Split(':', StringSplitOptions.RemoveEmptyEntries);

    return $"User ID={userInfo[0]};Password={userInfo[1]};Host={databaseUri.Host};" + 
           $"Port={databaseUri.Port};Database={db};Pooling=true;" + 
           "SSL Mode=Require;Trust Server Certificate=True;";
}