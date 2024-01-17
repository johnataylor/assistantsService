using AssistantsProxy;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddControllers()
//    .ConfigureApiBehaviorOptions(options =>
//    {
//        //options.SuppressModelStateInvalidFilter = true;
//        options.InvalidModelStateResponseFactory = context => new BadRequestResult();
//    });

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// swap these implementations if you want a pass-through proxy to OpenAI
//builder.Services.AddAssistantsRuntime();
builder.Services.AddAssistantsPassThroughProxy();

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations(enableAnnotationsForInheritance: true, enableAnnotationsForPolymorphism: true);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler("/error");

//app.UseExceptionHandler(exceptionHandlerApp =>
//{
//    exceptionHandlerApp.Run(async context =>
//    {
//        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

//        // using static System.Net.Mime.MediaTypeNames;
//        context.Response.ContentType = Text.Plain;

//        await context.Response.WriteAsync("An exception was thrown.");
//    });
//});

app.UseAuthorization();

app.MapControllers();

app.Run();
