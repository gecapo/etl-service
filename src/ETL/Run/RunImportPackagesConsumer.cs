using System.Reflection;

namespace ETL.Run;

public sealed class Run(
    IPackageConcurencyService _packageConcurencyService,
    IConfigurationProviderService _configurationProviderService,
    IFactory<IProcessor> _processorFactory
    )
{
    public async Task Start()
    {
        var configurationKeys = _configurationProviderService.GetAllConfigurationKeys();

        var packages = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(asmbl => asmbl.GetTypes())
                .Where(type => type.GetInterface(nameof(IImportPackageConfiguration)) != null)
                .Where(type => type.IsClass)
                .Where(type => !type.IsAbstract)
                .Where(type => configurationKeys.Contains(type.Name));

        var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
        foreach (var package in packages)
        {
            if (!_packageConcurencyService.CanRun(package))
                continue;

            _packageConcurencyService.Add(package);
            var packageInstane = (IImportPackageConfiguration)Activator.CreateInstance(package)!;
            var processor = _processorFactory.GetStrategy(packageInstane.ProcessorType.ToString());
            await processor.RunAsync(packageInstane).ConfigureAwait(false);
            _packageConcurencyService.Remove(package);
        }
    }
}