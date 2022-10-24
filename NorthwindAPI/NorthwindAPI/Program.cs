using Microsoft.EntityFrameworkCore;
using NorthwindAPI.Models;
using NorthwindAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<NorthwindContext>(opt => opt.UseSqlServer(builder.Configuration["default"]));
builder.Services.AddDbContext<NorthwindContext>(opt => opt.UseInMemoryDatabase("NorthwindMemory"));

builder.Services.AddScoped<ISupplierService, SupplierService>();
// Optional
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
