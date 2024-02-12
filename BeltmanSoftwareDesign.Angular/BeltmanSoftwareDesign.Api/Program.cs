using BeltmanSoftwareDesign.Business.Interfaces;
using BeltmanSoftwareDesign.Business.Services;
using BeltmanSoftwareDesign.StorageBlob.Business.Interfaces;
using BeltmanSoftwareDesign.StorageBlob.Business.Services;
using BeltmanSoftwareDesign.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

//builder.Services.AddDbContext<ApplicationDbContext>(
//    options => options.UseInMemoryDatabase("InMemoryDatabase"));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IStorageFileService, StorageFileService>();
builder.Services.AddScoped<IWorkordersService, WorkordersService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ICompaniesService, CompaniesService>();
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IUsersService, UsersService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
