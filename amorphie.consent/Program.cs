using amorphie.consent.data;
using amorphie.consent.Validator;
using amorphie.core.Extension;
using amorphie.core.HealthCheck;
using amorphie.core.Identity;
using amorphie.core.security.Extensions;
using amorphie.core.Swagger;
using amorphie.template.HealthCheck;
using AutoMapper;
using FluentValidation;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Npgsql.Replication;
using Prometheus;
using Dapr.Client;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDaprClient();
builder.Services.AddScoped<ITranslationService, TranslationService>();
builder.Services.AddScoped<ILanguageService, AcceptLanguageService>();

//builder.Services.AddHealthChecks().AddBBTHealthCheck();
builder.Services.AddScoped<IBBTIdentity, FakeIdentity>();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
await builder.Configuration.AddVaultSecrets("amorphie-consent", new string[] { "amorphie-consent" });
var postgreSql = builder.Configuration["PostgreSql"];
Console.WriteLine($"PostgreSql: {postgreSql}");
string jsonFilePath = Path.Combine(AppContext.BaseDirectory, "test.json");

builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<AddSwaggerParameterFilter>();

});

builder.Services.AddCors(options =>

{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("*")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddValidatorsFromAssemblyContaining<ConsentValidator>(includeInternalTypes: true);
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddDbContext<ConsentDbContext>
    (options => options.UseNpgsql(postgreSql, b => b.MigrationsAssembly("amorphie.consent.data")));
// builder.Services.AddDbContext<ConsentDbContext>
//     // (options => options.UseInMemoryDatabase("TemplateDbContext"));
//     (options => options.UseNpgsql("Host=localhost:5432;Database=ConsentDb;Username=postgres;Password=postgres", b => b.MigrationsAssembly("amorphie.consent.data")));


var app = builder.Build();

var jsonData = await File.ReadAllTextAsync(jsonFilePath);
using var client = new DaprClientBuilder().Build();
await client.SaveStateAsync("amorphie-state", "messages", jsonData);
var storedData = await client.GetStateAsync<string>("amorphie-state", "messages");
using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<ConsentDbContext>();

db.Database.Migrate();
DbInitializer.Initialize(db);


// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.AddRoutes();

// app.MapHealthChecks("/healthz", new HealthCheckOptions
// {
//     ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
// });
app.MapMetrics();

app.Run();

