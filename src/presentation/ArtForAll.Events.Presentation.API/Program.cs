using ArtForAll.Events.Presentation.API.Utils;
using ArtForAll.Presentation.API.Extensions;
using Microsoft.AspNetCore.Authentication.Certificate;
using Newtonsoft.Json.Serialization;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddNewtonsoftJson(setupAction =>//to support patching
    {
        setupAction.SerializerSettings.ContractResolver =
        new CamelCasePropertyNamesContractResolver();
    }).ConfigureApiBehaviorOptions(o => {
        //o.SuppressInferBindingSourcesForParameters = true;
    });
builder.Services.AddAuthentication(
    CertificateAuthenticationDefaults.AuthenticationScheme)
    .AddCertificate();

builder.Host.UseSerilog((context, configuration) =>
{
    configuration.ReadFrom.Configuration(context.Configuration);

    configuration.WriteTo.File(
        path: "logs/log.txt",
        rollOnFileSizeLimit: true,
        retainedFileCountLimit: 31,
        formatter: new CustomIndentedJsonFormatter(),
        rollingInterval: RollingInterval.Day
    );
});
builder.Services.AddServices(builder.Configuration, builder.Environment);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigins");

app.UseAuthentication();

app.UseAuthorization();

app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();
