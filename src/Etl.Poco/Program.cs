using Dapper.Contrib.Extensions;
using Etl.Poco.Factories;
using Etl.Poco.Repositories;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

var builder = Host.CreateDefaultBuilder(args);

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.ConfigureAppConfiguration((builderContext, config) =>
{
    config.SetBasePath(Directory.GetCurrentDirectory())
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true);
});

builder.ConfigureServices((builder, services) =>
{
    #region DB
    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
    services.AddSingleton<ISqlConnectionFactory>(serviceProvider => new SqlConnectionFactory(connectionString));
    #endregion

    #region SFTP
    SftpConfiguration ftpConfiguration = builder.Configuration.GetRequiredSection(nameof(SftpConfiguration)).Get<SftpConfiguration>()!;
    services.AddSingleton(ftpConfiguration);
    services.AddSingleton<IFtpService, TestFtpService>();
    #endregion

    #region SMTP
    SmtpConfiguration mailConfiguration = builder.Configuration.GetRequiredSection(nameof(SmtpConfiguration)).Get<SmtpConfiguration>()!;
    services.AddSingleton(mailConfiguration);
    services.AddSingleton<IMailService, TestMailService>();
    #endregion

    #region TeamsService
    TeamsWebhookConfiguration teamsConfiguration = builder.Configuration.GetRequiredSection(nameof(TeamsWebhookConfiguration)).Get<TeamsWebhookConfiguration>()!;
    services.AddHttpClient(nameof(TeamsWebHookService), (serviceProvider, client) => client.BaseAddress = new Uri(teamsConfiguration.BaseUri!))
        .ConfigurePrimaryHttpMessageHandler(() => new SocketsHttpHandler() { PooledConnectionLifetime = TimeSpan.FromMinutes(15) })
        .SetHandlerLifetime(Timeout.InfiniteTimeSpan);

    services.AddSingleton(teamsConfiguration);
    services.AddSingleton<ITeamsWebHookService, TeamsWebHookService>();
    #endregion

    #region HttpClients
    services.AddHttpClient("Default");
    #endregion

    #region RabbitMq
    RabbitMqConfiguration rabbitMqConfiguration = builder.Configuration
        .GetRequiredSection(nameof(RabbitMqConfiguration))
        .Get<RabbitMqConfiguration>()!;
    services.AddMassTransit(x =>
    {
        x.SetKebabCaseEndpointNameFormatter();

        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host(rabbitMqConfiguration.Host);
            cfg.ConfigureEndpoints(context);
        });
    });
    #endregion

    #region Register Processors
    var processorStrategies = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(asmbl => asmbl.GetTypes())
        .Where(type => type.GetInterface(nameof(IProcessor)) != null)
        .Where(type => type.IsClass)
        .Where(type => !type.IsAbstract);

    foreach (var strategy in processorStrategies)
        services.AddScoped(typeof(IProcessor), strategy);

    services.AddScoped<IFactory<IProcessor>, Factory<IProcessor>>(provider => new(provider.GetServices<IProcessor>().ToList()));
    #endregion

    #region Register DataProviders
    var retrieveStrategies = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(asmbl => asmbl.GetTypes())
            .Where(type => type.GetInterface(nameof(IDataProvider)) != null)
            .Where(type => type.IsClass)
            .Where(type => !type.IsAbstract);

    foreach (var strategy in retrieveStrategies)
        services.AddScoped(typeof(IDataProvider), strategy);

    services.AddScoped<IFactory<IDataProvider>, Factory<IDataProvider>>(provider => new(provider.GetServices<IDataProvider>().ToList()));
    services.AddScoped<IDataProviderService, DataProviderService>();
    #endregion

    #region Register Parsers
    var parseStrategy = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(asmbl => asmbl.GetTypes())
            .Where(type => type.GetInterface(nameof(IParser)) != null)
            .Where(type => type.IsClass)
            .Where(type => !type.IsAbstract);

    foreach (var strategy in parseStrategy)
        services.AddScoped(typeof(IParser), strategy);

    services.AddScoped<IFactory<IParser>, Factory<IParser>>(provider => new(provider.GetServices<IParser>().ToList()));
    services.AddScoped<IParserService, ParserService>();
    #endregion

    #region FileImport 
    services.AddScoped<IFileImportRepository, FileImportRepository>();
    services.AddScoped<IFileImportService, FileImportService>();
    #endregion

    #region Package Configuration 
    services.AddSingleton<IConfigurationRepository, ConfigurationRepository>();
    services.AddSingleton<IConfigurationProviderService, ConfigurationProviderService>();
    #endregion

    #region Functions 
    services.AddScoped<IFunctionProcessorService, FunctionProcessorService>();
    #endregion

    #region Mapper Service
    services.AddAutoMapper(typeof(InitialisationProfile));
    services.AddScoped<IMapperService, MapperService>();
    #endregion
});

//Add Serilog
builder.UseSerilog((host, serviceProvider, log) =>
{
    if (host.HostingEnvironment.IsProduction())
        log.MinimumLevel.Information();
    else
        log.MinimumLevel.Debug();

    log.MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
    log.MinimumLevel.Override("Quartz", LogEventLevel.Information);
    log.WriteTo.Console();

    ITeamsWebHookService webHookService = serviceProvider.GetService<ITeamsWebHookService>()!;
    log.WriteTo.Sink(new TeamsSink(CultureInfo.InvariantCulture, webHookService));
});

var app = builder.Build();

await app.RunAsync();
