using Etl.Poco.Constants;
using Etl.Poco.Interfaces;
using Microsoft.Data.SqlClient;

namespace Etl.Poco.Strategies.PackegeProcessors;

public sealed class LinkServerImportPackageProcessor(IServiceProvider serviceProvider)
    : BaseFileImportPackageProcessor(serviceProvider), IProcessor, IStrategy
{
    public bool IsHandler(string type) => type == ProcessorType.LinkedServer.ToString();

    public async Task RunAsync(IImportPackageConfiguration importPackage)
    {
        var fileName = $"{importPackage.DataProviderOptions!.PackageName}-{DateTime.UtcNow:yyyy-MM-dd-HH-mm}";
        var importResult = await _fileImportService.InsertNewFileImport(fileName, importPackage.DataProviderOptions.ImportType!.Value);

        if (!importResult.IsSuccess)
            return;

        using SqlConnection connection = _sqlConnectionFactory.Create();
        try
        {
            var count = 0;
            //foreach (var postProcessFunction in importPackage.ProcessFunctions)
            //    count += await connection.ExecuteAsync(postProcessFunction,
            //        new { FileImportId = importResult.FileImport.ID },
            //        commandTimeout: 0,
            //        commandType: CommandType.StoredProcedure);

            await _fileImportService.UpdateSuccessful(importResult.FileImport, count);
        }
        catch (Exception e)
        {
            await _fileImportService.UpdateFailure(importResult.FileImport, e);
        }

    }
}

//public sealed class SqlLinkServerImportProcessor
