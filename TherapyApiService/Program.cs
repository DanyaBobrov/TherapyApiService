using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;
using TherapyApiService.Infrastructure;
using TherapyApiService.Repositories.Implementations;
using TherapyApiService.Repositories.Interfaces;
using TherapyApiService.Swagger.Options;

var builder = WebApplication.CreateBuilder(args);

ConfigureLogging(builder.Logging);
ConfigureServices(builder.Services);

var app = builder.Build();

ConfigureApplication(app);
ConfigureEndpointRoute(app);

app.Run();

void ConfigureLogging(ILoggingBuilder logging)
{

}

void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<IInjectionRepository, InjectionRepository>();//TODO

    services.AddApiVersioning();
    services.AddVersionedApiExplorer(setup =>
    {
        setup.SubstituteApiVersionInUrl = true;
    });
    services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

    services.AddSwaggerGen(options =>
    {
        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

        options.EnableAnnotations();
        options.DescribeAllParametersInCamelCase();
    });

    services.AddControllers(options =>
    {
        options.ModelBinderProviders.Insert(0, new EntityIdModelBinderProvider());
    });
}

void ConfigureApplication(IApplicationBuilder app)
{
    app.UseHttpsRedirection();
    app.UseAuthorization();

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>().ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}

void ConfigureEndpointRoute(IEndpointRouteBuilder endpoint)
{
    endpoint.MapControllers();
}