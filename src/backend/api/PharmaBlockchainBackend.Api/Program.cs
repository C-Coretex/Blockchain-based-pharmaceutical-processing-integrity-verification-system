using PharmaBlockchainBackend.Infrastructure;
using PharmaBlockchainBackend.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

var dbConnectionString = builder.Configuration.GetConnectionString("DbConnectionString")
                                    ?? throw new ArgumentException("'DbConnectionString' is not configured.");

builder.Services.RegisterRepositories(dbConnectionString);

builder.Services.AddScoped<PharmaBlockchainBackend.Api.Features.ProtocolActions.StepSubmit.Handler>();
builder.Services.AddScoped<PharmaBlockchainBackend.Api.Features.ProtocolActions.List.Handler>();

var uiUrl = builder.Configuration.GetValue<string>("AppSettings:uiUrl")!;
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins(uiUrl)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    //Apply database migrations
    PharmaBlockchainBackendDbContext.ApplyMigrations(dbConnectionString);
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();

PharmaBlockchainBackend.Api.Features.ProtocolActions.StepSubmit.Endpoint.Map(app);
PharmaBlockchainBackend.Api.Features.ProtocolActions.List.Endpoint.Map(app);

app.Run();
