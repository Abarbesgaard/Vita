using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var url = "https://vhomzkchzmeaxpjfjmvd.supabase.co";
var key =
    "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6InZob216a2Noem1lYXhwamZqbXZkIiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImlhdCI6MTcyNzkzOTYyNywiZXhwIjoyMDQzNTE1NjI3fQ.fZnEYMM3aY9ixGFclig6C-6M71sGCi25NoRaDvPfcuI";
var options = new Supabase.SupabaseOptions
{
    AutoConnectRealtime = true
};

var supabase = new Supabase.Client(url, key, options);

await supabase.InitializeAsync();
// ============================
// Add Authentication Services
// ============================
builder.Services.AddAuthentication(optAuth =>
    {
        optAuth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        optAuth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(jwtBearerOptions =>
    {
        // ==========================
        // Set Authority and Audience
        // ==========================  
        jwtBearerOptions.Authority = "https://vhomzkchzmeaxpjfjmvd.supabase.co/auth/v1";

        // ==========================
        // Token Validation Parameters
        // ========================== 
        jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,  // Validates that the token's issuer is correct
            ValidateAudience = true,  // Validates that the audience is correct
            ValidAudiences = [
                "authenticated"
            ],
            ValidateIssuerSigningKey = true,  // Checks that the signature matches the public key from Auth0
            ValidateLifetime = true,  // Ensures the token has not expired
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Jc4ZvGS+REA18iW9pKsEDGEOXKTF5EGrttm7aTgKvQRGKL/EcI60b3PvdbMdNYnQWLDbxoqYy0uDkvh+/E4Gew==")),
            ValidIssuers =
            [
                "https://vhomzkchzmeaxpjfjmvd.supabase.co/auth/v1",
               "https://vhomzkchzmeaxpjfjmvd.supabase.co/"
            ],
            ClockSkew = TimeSpan.Zero  // Reduces the allowed clock skew time
        }; 
        jwtBearerOptions.RequireHttpsMetadata = true;

        // ==========================
        // Add Token Validation Events
        // ==========================
        jwtBearerOptions.Events = new JwtBearerEvents
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
                if (context.Principal!.Identity is ClaimsIdentity identity)
                {
                    Console.WriteLine($"Token validated. Subject: {identity.Name}");
                }
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                // Log when a challenge is issued
                if (context.AuthenticateFailure != null)
                {
                    Console.WriteLine($"Authentication challenge failed: {context.AuthenticateFailure.Message}");
                }
                return Task.CompletedTask;
            },
            OnMessageReceived = context =>
            {
                if (string.IsNullOrEmpty(context.Token))
                {
                    Console.WriteLine("No Authorization header present.");
                }
                return Task.CompletedTask;
            },
          
        };
    });


// ============================
// Add Services for Controllers
// ============================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

// ============================
// Configure Authorization Policies
// ============================
builder.Services.AddAuthorizationBuilder()
	.AddPolicy("authPolicy", policy =>
	{
		policy.RequireAuthenticatedUser();
	});
// ============================
// Configure Swagger
// ============================
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new OpenApiInfo { Title = "Vita API", Version = "v1" });
});

// ============================
// Configure Reverse Proxy
// ============================
builder.Services.AddReverseProxy()
	.LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// ============================
// Configure CORS
// ============================
builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();


// ============================
// // Configure the HTTP request pipeline
// // ============================
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
// ============================
// Map Reverse Proxy and Controllers
// ============================
app.MapReverseProxy()
	.RequireAuthorization("authPolicy");
app.MapControllers();

app.Run();