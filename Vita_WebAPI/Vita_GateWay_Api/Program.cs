using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,  // Validates that the token's issuer is correct
            ValidateAudience = true,  // Validates that the audience is correct
            ValidateIssuerSigningKey = true,  // Checks that the signature matches the public key from Auth0
            ValidateLifetime = true,  // Ensures the token has not expired
            ClockSkew = TimeSpan.Zero,  // Reduces the allowed clock skew time
            IssuerSigningKeyResolver =  (token, securityToken, kid, validationParameters) =>
            {
                using var client = new HttpClient();
                var jwks = client.GetStringAsync("https://dev-dj6iiunlxv3pukjx.us.auth0.com/.well-known/jwks.json").Result;
                return new JsonWebKeySet(jwks).GetSigningKeys();
            }
        }; 
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
app.UseAuthentication();
app.UseAuthorization();
app.MapReverseProxy()
    .RequireAuthorization("authPolicy");
app.MapControllers();

app.Run();