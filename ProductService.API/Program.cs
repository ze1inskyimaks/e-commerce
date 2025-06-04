using ProductService.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddLogging()
    .Services
    .AddDataBase(builder.Configuration)
    .AddDependencyInjection()
    .AddSwagger();

var app = builder.Build();

app = Service.AddApplicationSettings(app);

app.Run();
