using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentPlacement.Backend.Dal;
using StudentPlacement.Backend.Dal.Implementations;
using StudentPlacement.Backend.Dal.Interfaces;
using StudentPlacement.Backend.Models.Jwt;
using StudentPlacement.Backend.Services.Implementations;
using StudentPlacement.Backend.Services.Interfaces;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

//builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
//builder.Services.AddSingleton<LinkGenerator>();

builder.Services.AddScoped<IJwtProviderService, JwtProviderService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IAllocationService, AllocationService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IDocService, DocService>();
builder.Services.AddScoped<IStructureService, StructureService>();
builder.Services.AddScoped<IFileService, FileService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IOrganizationRepository, OrganizationRepository>();
builder.Services.AddScoped<IAllocationRequestRepository, AllocationRequestRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IDepartmentsRepository, DepartmentRepository>();
builder.Services.AddScoped<ISpecialityRepository, SpecialityRepository>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactClient", builder =>
    {
        builder.WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
});

var jwtConfiguration = builder.Configuration.GetSection("JwtConfig").Get<JwtConfiguration>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(o => o.TokenValidationParameters = new()
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfiguration.Issuerr,
            ValidAudience = jwtConfiguration.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtConfiguration.SecretKey))
        });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();    
app.UseAuthorization();

app.MapControllers();

app.UseCors("ReactClient");

app.Run();
