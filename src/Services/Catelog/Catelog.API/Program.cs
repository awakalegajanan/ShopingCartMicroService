var builder = WebApplication.CreateBuilder(args);

// add services to container
builder.Services.AddCarter();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
builder.Services.AddMarten(option =>
{
    option.Connection(builder.Configuration.GetConnectionString("MartenDb")!);       
}).UseLightweightSessions();

var app = builder.Build();
//configure the http request pipeline
app.MapCarter();

app.Run();
