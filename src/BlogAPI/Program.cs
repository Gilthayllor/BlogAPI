using BlogAPI;
using BlogAPI.Data;
using BlogAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

ConfigureAuthentication(builder);

ConfigureMvc(builder);

ConfigureServices(builder);

var app = builder.Build();

LoadConfigration(app);

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseResponseCompression();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x => {
        x.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        x.RoutePrefix = string.Empty;
    });
}

app.Run();

void LoadConfigration(WebApplication app)
{
    Configuration.JwtKey = app.Configuration.GetValue<string>("JwtKey");
    Configuration.ApiKeyName = app.Configuration.GetValue<string>("ApiKeyName");
    Configuration.ApiKey = app.Configuration.GetValue<string>("ApiKey");
    var smtp = new Configuration.SmtpConfiguration();
    app.Configuration.GetSection("Smtp").Bind(smtp);
    Configuration.Smtp = smtp;
}

void ConfigureAuthentication(WebApplicationBuilder builder)
{
    var key = Encoding.ASCII.GetBytes(Configuration.JwtKey);
    builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
}

void ConfigureMvc(WebApplicationBuilder builder)
{
    builder.Services.AddMemoryCache();
    builder.Services.AddResponseCompression(x =>
    {
        x.Providers.Add<GzipCompressionProvider>();
    });
    builder.Services.Configure<GzipCompressionProviderOptions>(x =>
    {
        x.Level = System.IO.Compression.CompressionLevel.Optimal;
    });
    builder.Services
        .AddControllers()
        .ConfigureApiBehaviorOptions(o =>
        {
            o.SuppressModelStateInvalidFilter = true;
        })
        .AddJsonOptions(x =>
        {
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
        });
}

void ConfigureServices(WebApplicationBuilder builder)
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddTransient<TokenService>();
    builder.Services.AddDbContext<BlogAPIContext>(x => {
        x.UseSqlServer(connectionString);
    });

    builder.Services.AddEndpointsApiExplorer();
    
    builder.Services.AddSwaggerGen();
}