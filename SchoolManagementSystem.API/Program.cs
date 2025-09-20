using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SchoolManagement.Repositories;
using SchoolManagementSystem.API.Repositories.EnrollmentRepository;
using SchoolManagementSystem.API.Services.CourseService;
using SchoolManagementSystem.API.Services.EnrollmentService;
using SchoolManagementSystem.Application.Contracts.Assignment.Request;
using SchoolManagementSystem.Application.Contracts.Auth.Request;
using SchoolManagementSystem.Application.Contracts.Course.Request;
using SchoolManagementSystem.Application.Contracts.Enrollment.Request;
using SchoolManagementSystem.Application.Interfaces.Services;
using SchoolManagementSystem.Application.Services;
using SchoolManagementSystem.Core.Interfaces.Repositories;
using SchoolManagementSystem.Infrastructure.Data;
using SchoolManagementSystem.Infrastructure.Repositories;
using Serilog;
using SWR.Api.Middleware;
using System.Text;

// The Serilog configuration should be done before building the host.
Log.Logger = new LoggerConfiguration()
	.MinimumLevel.Information()
	.WriteTo.Console()
	.WriteTo.File("logs/log.txt", rollingInterval: RollingInterval.Day)
	.Enrich.FromLogContext()
	.CreateLogger();

try
{

	var builder = WebApplication.CreateBuilder(args);

	// Add services to the container
	builder.Services.AddControllers();

	// Configure DbContext with SQL Server and a connection string
	builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

	// Register our new services and repositories for Dependency Injection
	builder.Services.AddScoped<IAuthRepository, AuthRepository>();
	builder.Services.AddScoped<IAuthService, AuthService>();
	builder.Services.AddScoped<ICourseRepository, CourseRepository>();
	builder.Services.AddScoped<ICourseService, CourseService>();
	builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
	builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
	builder.Services.AddScoped<IAssignmentRepository, AssignmentRepository>();
	builder.Services.AddScoped<IAssignmentService, AssignmentService>();

	// Database Seeder
	builder.Services.AddScoped<DbSeeder>();

	// Fluent Validation
	builder.Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
	var validatorAssemblies = new[]
	{
	typeof(UserRegisterRequest).Assembly,
	typeof(CourseRequest).Assembly,
	typeof(EnrollmentRequest).Assembly,
	typeof(CreateAssignmentRequest).Assembly,
	typeof(GradeAssignmentRequest).Assembly
};
	builder.Services.AddValidatorsFromAssemblies(validatorAssemblies);

	// Configure JWT Authentication
	builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
		.AddJwtBearer(options =>
		{
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
					.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value!)),
				ValidateIssuer = true,
				ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
				ValidateAudience = true,
				ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value
			};
		});

	// Swagger Config
	builder.Services.AddSwaggerGen(setup =>
	{

		// Include 'SecurityScheme' to use JWT Authentication
		var jwtSecurityScheme = new OpenApiSecurityScheme
		{
			BearerFormat = "JWT",
			Name = "JWT Authentication",
			In = ParameterLocation.Header,
			Type = SecuritySchemeType.Http,
			Scheme = JwtBearerDefaults.AuthenticationScheme,
			Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

			Reference = new OpenApiReference
			{
				Id = JwtBearerDefaults.AuthenticationScheme,
				Type = ReferenceType.SecurityScheme
			}
		};

		setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

		setup.AddSecurityRequirement(new OpenApiSecurityRequirement
					{
						{ jwtSecurityScheme, Array.Empty<string>() }
					});
	});

	var app = builder.Build();

	if (app.Environment.IsDevelopment())
	{
		app.UseSwagger();
		app.UseSwaggerUI();
	}

	// Centralized error handling
	app.UseMiddleware<ExceptionMiddleware>();

	// Configure the HTTP request pipeline.
	app.UseHttpsRedirection();

	app.UseAuthorization();

	app.MapControllers();

	// Seed the database
	using (var scope = app.Services.CreateScope())
	{
		var seeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();
		await seeder.SeedAdminAsync();
	}

	app.Run();
}
catch (Exception ex)
{
	Log.Fatal(ex, "Host terminated unexpectedly");
}
finally
{
	Log.CloseAndFlush();
}