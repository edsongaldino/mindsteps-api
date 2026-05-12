using MindSteps.Application.Interfaces;
using MindSteps.Application.Services;
using MindSteps.Domain.Interfaces;
using MindSteps.Infrastructure.Data;
using MindSteps.Infrastructure.Repositories;
using MindSteps.SharedKernel.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;
var jwtSettings = configuration.GetSection("JwtSettings");
var secret = jwtSettings["Secret"];

if (string.IsNullOrWhiteSpace(secret))
	throw new Exception("JwtSettings:Secret não configurado.");

var key = Encoding.ASCII.GetBytes(secret);

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseNpgsql(configuration.GetConnectionString("Default")));

// Repositories
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IPsicologoRepository, PsicologoRepository>();
builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();
builder.Services.AddScoped<IAtividadeRepository, AtividadeRepository>();
builder.Services.AddScoped<ICheckInEmocionalRepository, CheckInEmocionalRepository>();
builder.Services.AddScoped<IRegistroPensamentoRepository, RegistroPensamentoRepository>();

// Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IPsicologoService, PsicologoService>();
builder.Services.AddScoped<IPacienteService, PacienteService>();
builder.Services.AddScoped<IAtividadeService, AtividadeService>();
builder.Services.AddScoped<ICheckInEmocionalService, CheckInEmocionalService>();
builder.Services.AddScoped<IRegistroPensamentoService, RegistroPensamentoService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services
	.AddAuthentication(options =>
	{
		options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	})
	.AddJwtBearer(options =>
	{
		options.RequireHttpsMetadata = false;
		options.SaveToken = true;

		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = jwtSettings["Issuer"],
			ValidAudience = jwtSettings["Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(key)
		};
	});

builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "MindSteps API", Version = "v1" });

	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Description = "Digite: Bearer {seu token JWT}",
		Name = "Authorization",
		In = ParameterLocation.Header,
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer",
		BearerFormat = "JWT"
	});

	c.AddSecurityRequirement(new OpenApiSecurityRequirement
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Id = "Bearer",
					Type = ReferenceType.SecurityScheme
				}
			},
			Array.Empty<string>()
		}
	});
});

var origins = configuration.GetSection("Cors:Origins").Get<string[]>() ?? Array.Empty<string>();

builder.Services.AddCors(options =>
{
	options.AddPolicy("Default", policy =>
	{
		if (origins.Length > 0)
		{
			policy.WithOrigins(origins)
				.AllowAnyHeader()
				.AllowAnyMethod();
		}
		else
		{
			policy.AllowAnyOrigin()
				.AllowAnyHeader()
				.AllowAnyMethod();
		}
	});
});

var app = builder.Build();

app.UseCors("Default");

app.UseSwagger();
app.UseSwaggerUI(c =>
{
	c.SwaggerEndpoint("/swagger/v1/swagger.json", "MindSteps API v1");
	c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();