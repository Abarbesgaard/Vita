using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(options =>
	{
		options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	})
	.AddJwtBearer(options =>
	{
		options.Authority = "https://dev-dj6iiunlxv3pukjx.us.auth0.com/";
		options.Audience = "https://dev-dj6iiunlxv3pukjx.us.auth0.com/api/v2/";
		options.RequireHttpsMetadata = true;

		// Add logging for token validation issues
		options.Events = new JwtBearerEvents
		{
			OnAuthenticationFailed = context =>
			{
				// Log authentication failure for debugging
				Console.WriteLine($"Authentication failed: {context.Exception.Message}");
				return Task.CompletedTask;
			},
			OnTokenValidated = context =>
			{
				// Log successful token validation
				Console.WriteLine("Token validated successfully.");
				return Task.CompletedTask;
			},
			OnChallenge = context =>
			{
				// Log when a challenge is issued
				Console.WriteLine("Token validation challenge issued.");
				return Task.CompletedTask;
			}
		};
	});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddAuthorizationBuilder()
	.AddPolicy("authPolicy", policy =>
	{
		policy.RequireAuthenticatedUser();
	});
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Vita API", Version = "v1" });
});
builder.Services.AddReverseProxy()
	.LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c =>
		{
			c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API Gateway");
		}
	);
	app.UseExceptionHandler("/Home/Error");
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy()
	.RequireAuthorization("authPolicy");
app.MapControllers();

app.Run();