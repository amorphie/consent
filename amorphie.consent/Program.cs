using amorphie.consent.data;
using amorphie.consent.Service;
using amorphie.consent.Service.Interface;
using amorphie.consent.Service.Refit;
using amorphie.consent.Validator;
using amorphie.core.HealthCheck;
using amorphie.core.Identity;
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
using Elastic.Apm.NetCoreAll;
using Polly;
using Polly.Extensions.Http;
using Polly.Retry;
using Polly.Timeout;
using Refit;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Net.Http.Headers;
using System;
using amorphie.core.Extension;
using Dapr;
using Microsoft.AspNetCore.HttpLogging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDaprClient();
builder.Services.AddScoped<ITranslationService, TranslationService>();
builder.Services.AddScoped<ILanguageService, AcceptLanguageService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IYosInfoService, YosInfoService>();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<IBKMService, BKMService>();
builder.Services.AddScoped<IPushService, PushService>();
builder.Services.AddScoped<IOBEventService, OBEventService>();
builder.Services.AddScoped<IOBAuthorizationService, OBAuthorizationService>();

//builder.Services.AddHealthChecks().AddBBTHealthCheck();
builder.Services.AddScoped<IBBTIdentity, FakeIdentity>();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
await builder.Configuration.AddVaultSecrets("amorphie-consent", new string[] { "amorphie-consent" });
var postgreSql = builder.Configuration["PostgreSql"];
var pfxPassword = builder.Configuration["PfxPassword"];
string jsonFilePath = Path.Combine(AppContext.BaseDirectory, "test.json");

builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<AddSwaggerParameterFilter>();

});


var defaultHeadersToBeLogged = new List<string>
{
    "Content-Type",
    "Host",
    "X-Zeebe-Job-Key",
    "xdeviceid",
    "X-Device-Id",
    "xtokenid",
    "X-Token-Id",
    "Transfer-Encoding",
    "X-Forwarded-Host",
    "X-Forwarded-For"
};

builder.Services.AddHttpLogging(logging =>
{

    logging.LoggingFields = HttpLoggingFields.All;

    //logging.RequestHeaders.Concat(headersToBeLogged);

    defaultHeadersToBeLogged.ForEach(p => logging.RequestHeaders.Add(p));
 
    logging.MediaTypeOptions.AddText("application/javascript");

    logging.RequestBodyLogLimit = 4096;

    logging.ResponseBodyLogLimit = 4096;

});

builder.Services.AddHttpContextAccessor();
builder.Logging.ClearProviders();
builder.Host.UseSerilog((_, serviceProvider, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(builder.Configuration);

});


//wait 1s and retry again 3 times when get timeout
AsyncRetryPolicy<HttpResponseMessage> retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .Or<TimeoutRejectedException>()
    .WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(1000));

builder.Services
    .AddRefitClient<IPaymentClientService>()
    .ConfigureHttpClient(c =>
        c.BaseAddress = new Uri(builder.Configuration["ServiceURLs:PaymentServiceURL"] ??
                                throw new ArgumentNullException("Parameter is not suplied.", "PaymentServiceURL")))
    .AddPolicyHandler(retryPolicy);

builder.Services
    .AddRefitClient<IAccountClientService>()
    .ConfigureHttpClient(c =>
        c.BaseAddress = new Uri(builder.Configuration["ServiceURLs:AccountServiceURL"] ??
                                throw new ArgumentNullException("Parameter is not suplied.", "AccountServiceURL")))
    .AddPolicyHandler(retryPolicy);

builder.Services
    .AddRefitClient<ITokenClientService>()
    .ConfigureHttpClient(c =>
        c.BaseAddress = new Uri(builder.Configuration["ServiceURLs:TokenServiceURL"] ??
                                throw new ArgumentNullException("Parameter is not suplied.", "TokenServiceURL")))
    .AddPolicyHandler(retryPolicy);

builder.Services
    .AddRefitClient<IContractClientService>()
    .ConfigureHttpClient(c =>
        c.BaseAddress = new Uri(builder.Configuration["ServiceURLs:ContractServiceURL"] ??
                                throw new ArgumentNullException("Parameter is not suplied.", "ContractServiceURL")))
    .AddPolicyHandler(retryPolicy);
X509Certificate2 certificate = new X509Certificate2("0125_480.pfx", pfxPassword);
var handler = new HttpClientHandler();
handler.ClientCertificates.Add(certificate);

builder.Services
    .AddRefitClient<IBKMClientService>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(builder.Configuration["ServiceURLs:BkmUrl"] ??
                                throw new ArgumentNullException("Parameter is not suplied.", "BKMCLient"));
    })
    .ConfigurePrimaryHttpMessageHandler(() => handler)
    .AddPolicyHandler(retryPolicy);

    builder.Services
    .AddRefitClient<IMessagingGateway>()
    .ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri(builder.Configuration["MessagingGateway:MessagingGatewayUrl"] ??
                                throw new ArgumentNullException("Parameter is not suplied.", "YosUrl"));
    })
    .ConfigurePrimaryHttpMessageHandler(() => handler)
    .AddPolicyHandler(retryPolicy);

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
app.UseAllElasticApm(app.Configuration);
app.UseCloudEvents();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapSubscribeHandler();
});
app.UseHttpLogging();
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

