

using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// add services to container

var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(assembly);
    cfg.AddOpenBehavior(typeof(BuildingBlocks.Behaviors.ValidationBehavior<,>));
    cfg.AddOpenBehavior(typeof(BuildingBlocks.Behaviors.LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter();

builder.Services.AddMarten(option =>
{
    option.Connection(builder.Configuration.GetConnectionString("MartenDb")!);       
}).UseLightweightSessions();

if(builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith<CatelogInitialData>();
}

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("MartenDb")!, name: "PostgreSQL", tags: new[] { "ready" });

var app = builder.Build();

//configure the http request pipeline
app.MapCarter();

app.UseExceptionHandler(options => { });

app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        //Predicate = (check) => check.Tags.Contains("ready"),
        //ResponseWriter = async (context, report) =>
        //{
        //    context.Response.ContentType = "application/json";
        //    var response = new
        //    {
        //        status = report.Status.ToString(),
        //        checks = report.Entries.Select(entry => new
        //        {
        //            name = entry.Key,
        //            status = entry.Value.Status.ToString(),
        //            exception = entry.Value.Exception?.Message,
        //            duration = entry.Value.Duration.ToString()
        //        })
        //    };
        //    await context.Response.WriteAsJsonAsync(response);
        //}
    });

#region earlier handler code
//app.UseExceptionHandler(exceptionHandlerApp =>
//{
//    exceptionHandlerApp.Run(async context =>
//    {
//        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
//        if(exception == null)
//        {
//            return;
//        }

//        var problemDetails = new ProblemDetails
//        {
//            Status = StatusCodes.Status500InternalServerError,
//            Title = exception.Message,
//            Detail = exception.StackTrace
//        };

//        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
//        logger.LogError(exception, exception.Message);

//        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
//        context.Response.ContentType = "application/problem+json";

//        await context.Response.WriteAsJsonAsync(problemDetails);

//        //var exceptionHandlerPathFeature =
//        //    context.Features.Get<IExceptionHandlerPathFeature>();
//        //if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
//        //{
//        //    await context.Response.WriteAsync(" The file was not found.");
//        //}

//        //if (exceptionHandlerPathFeature?.Path == "/")
//        //{
//        //    await context.Response.WriteAsync(" Page: Home.");
//        //}
//    });
//});
#endregion earlier handler code
app.Run();
