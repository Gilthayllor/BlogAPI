using BlogAPI.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(o =>
    {
        o.SuppressModelStateInvalidFilter = true;
    });

builder.Services.AddDbContext<BlogAPIContext>();

var app = builder.Build();
app.MapControllers();

app.Run();
