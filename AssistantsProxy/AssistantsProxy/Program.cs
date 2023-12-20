using AssistantsProxy.Models;
using AssistantsProxy.Models.Implementation;
using AssistantsProxy.Models.Proxy;
using Microsoft.AspNetCore.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// swap these implementations if you want a pass-through proxy to OpenAI

builder.Services.AddScoped<IAssistantsModel, AssistantsModel>();
builder.Services.AddScoped<IThreadsModel, ThreadsModel>();
builder.Services.AddScoped<IMessagesModel, MessagesModel>();
builder.Services.AddScoped<IRunsModel, RunsModel>();

//builder.Services.AddScoped<IAssistantsModel, ProxyAssistantsModel>();
//builder.Services.AddScoped<IThreadsModel, ProxyThreadsModel>();
//builder.Services.AddScoped<IMessagesModel, ProxyMessagesModel>();
//builder.Services.AddScoped<IRunsModel, ProxyRunsModel>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        // using static System.Net.Mime.MediaTypeNames;
        context.Response.ContentType = Text.Plain;

        await context.Response.WriteAsync("An exception was thrown.");

        var exceptionHandlerPathFeature =
            context.Features.Get<IExceptionHandlerPathFeature>();

        if (exceptionHandlerPathFeature?.Error is FileNotFoundException)
        {
            await context.Response.WriteAsync(" The file was not found.");
        }

        if (exceptionHandlerPathFeature?.Path == "/")
        {
            await context.Response.WriteAsync(" Page: Home.");
        }
    });
});

app.UseAuthorization();

app.MapControllers();

app.Run();
