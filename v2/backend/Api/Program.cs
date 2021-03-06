using System.Reflection;
using Amazon.CognitoIdentityProvider;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.Runtime;
using Api.Data;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
    options.UseNpgsql(connectionString);
});

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// Auth/Cognito setup
var region = builder.Configuration.GetValue<string>("AWSCognito:Region");
var poolId = builder.Configuration.GetValue<string>("AWSCognito:PoolId");
var clientId = builder.Configuration.GetValue<string>("AWSCognito:AppClientId");
var credentials = new AnonymousAWSCredentials();
var provider = new AmazonCognitoIdentityProviderClient();
var userPool = new CognitoUserPool(poolId, clientId, provider);
builder.Services.AddSingleton(credentials);
builder.Services.AddSingleton<IAmazonCognitoIdentityProvider>(provider);
builder.Services.AddSingleton(userPool);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = $"https://cognito-idp.{region}.amazonaws.com/{poolId}",
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidAudience = clientId,
            ValidateAudience = false,
            RoleClaimType = "cognito:groups"
        };
        options.MetadataAddress = 
            $"https://cognito-idp.{region}.amazonaws.com/{poolId}/.well-known/openid-configuration";
    });
    
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        cors =>
        {
            cors.WithOrigins("http://localhost:3000")
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
