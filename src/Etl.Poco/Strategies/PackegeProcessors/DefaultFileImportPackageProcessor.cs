using Etl.Poco.Constants;
using Etl.Poco.Interfaces;
using Etl.Poco.Models.Common;
using Microsoft.Data.SqlClient;

namespace Etl.Poco.Strategies.PackegeProcessors;

public sealed class DefaultFileImportPackageProcessor(IServiceProvider serviceProvider)
    : BaseFileImportPackageProcessor(serviceProvider), IProcessor, IStrategy
{
    public bool IsHandler(string type) => type == ProcessorType.Generic.ToString();

    public async Task RunAsync(IImportPackageConfiguration importPackage)
    {
        _dataProviderService.DataProviderOptions = importPackage.DataProviderOptions;
        _dataProviderService.DataProviderType = importPackage.DataProviderType;

        _parserService.ParserOptions = importPackage.ParserOptions!;
        _parserService.ParseType = importPackage.ParseType;

        var retrieveDataResult = await _dataProviderService.RetrieveFirstOrDefaultAsync();
        if (!retrieveDataResult.IsSuccess)
            return;

        FileImportResult importResult = null;
        try
        {
            importResult = await _fileImportService.InsertNewFileImport(retrieveDataResult.FileName!,
                importPackage.DataProviderOptions!.ImportType!.Value);
            if (!importResult.IsSuccess)
                throw new Exception(importResult.Message);

            await _functionService.HandleFunction(FunctionType.OnBeforeParsing, importPackage, retrieveDataResult);

            var parseDataResult = await _parserService.ParseData(importResult.FileImport!.Id, retrieveDataResult.Data!);
            if (!parseDataResult.IsSuccess)
                throw new Exception(parseDataResult.Message);

            await _functionService.HandleFunction(FunctionType.OnBeforeInsert, importPackage,
                parseDataResult.ParsedData.SelectMany(list => list).ToArray());

            using SqlConnection connection = _sqlConnectionFactory.Create();
            foreach (var pair in parseDataResult.ParsedData)
                await connection.BulkInsertToDatabase(pair);

            await _functionService.HandleFunction(FunctionType.OnAfterInsert, importPackage,
                parseDataResult.ParsedData.SelectMany(list => list).ToArray());

            var fileTransferResult = await _dataProviderService.HandleSuccessAsync(retrieveDataResult.FileName!);
            if (!fileTransferResult.IsSuccess)
                throw new Exception(fileTransferResult.Message);

            var fileImportUpdateResult =
                await _fileImportService.UpdateSuccessful(importResult.FileImport, parseDataResult.RowsCount);
            if (!fileImportUpdateResult.IsSuccess)
                throw new Exception(fileImportUpdateResult.Message);
        }
        catch (Exception e)
        {
            await _fileImportService.UpdateFailure(importResult.FileImport, e);
            await _dataProviderService.HandleFailureAsync(retrieveDataResult.FileName!);
        }
    }
}