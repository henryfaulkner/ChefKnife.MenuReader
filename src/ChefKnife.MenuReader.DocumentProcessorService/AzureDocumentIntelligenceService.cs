using Azure;
using Azure.AI.FormRecognizer;
using Azure.AI.FormRecognizer.Models;
using ChefKnife.Shared.Config;
using ChefKnife.Shared.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Azure.AI.FormRecognizer.DocumentAnalysis;

namespace ChefKnife.MenuReader.DocumentProcessorService;

public class AzureDocumentIntelligenceService : IDocumentProcessorService
{
    readonly AzureDocumentIntelligenceConfig _config;
    readonly DocumentAnalysisClient _formRecognizerClient;

    public AzureDocumentIntelligenceService(AzureDocumentIntelligenceConfig config)
    {
        _config = config;
        _formRecognizerClient = new DocumentAnalysisClient(new Uri(config.Endpoint), new AzureKeyCredential(config.ApiKey));
    }

    public async Task<Dictionary<string, ProcessedField>> ExtractDataFromDocumentAsync(Stream stream, CancellationToken cancellationToken = default)
    {
        var result = new Dictionary<string, ProcessedField>();

        // Extract data using the custom model
        var operation = await _formRecognizerClient.AnalyzeDocumentAsync(WaitUntil.Completed, _config.ModelId, stream, cancellationToken: cancellationToken);

        // Check if the operation completed successfully
        if (operation.HasValue)
        {
            var analysisResult = operation.Value;
            foreach (var analyzedDoc in analysisResult.Documents)
            {
                result.AddRangeNewOnly(ProcessExtractedData(analyzedDoc));
            }
        }
        else
        {
            throw new Exception("Failed to extract data from the document.");
        }

        return result;
    }

    // Method to process extracted data and format it
    static Dictionary<string, ProcessedField> ProcessExtractedData(AnalyzedDocument analyzedDocument)
    {
        var result = new Dictionary<string, ProcessedField>();

        // Loop through the fields in the extracted document
        foreach (var field in analyzedDocument.Fields)
        {
            result.Add(field.Key, ProcessDocumentFieldRecursive(field.Value, depth: 1));
        }

        return result;
    }

    static ProcessedField ProcessDocumentFieldRecursive(DocumentField field, int depth)
    {
        return field.FieldType switch
        {
            DocumentFieldType.String => new ProcessedField { FieldType = "String", Value = field.Value.AsString() },
            DocumentFieldType.Date => new ProcessedField { FieldType = "Date", Value = field.Value.AsDate() },
            DocumentFieldType.Time => new ProcessedField { FieldType = "Time", Value = field.Value.AsTime() },
            DocumentFieldType.PhoneNumber => new ProcessedField { FieldType = "PhoneNumber", Value = field.Value.AsPhoneNumber() },
            DocumentFieldType.Double => new ProcessedField { FieldType = "Double", Value = field.Value.AsDouble() },
            DocumentFieldType.Int64 => new ProcessedField { FieldType = "Int64", Value = field.Value.AsInt64() },
            DocumentFieldType.Boolean => new ProcessedField { FieldType = "Boolean", Value = field.Value.AsBoolean() },
            DocumentFieldType.CountryRegion => new ProcessedField { FieldType = "CountryRegion", Value = field.Value.AsCountryRegion() },
            DocumentFieldType.SelectionMark => new ProcessedField { FieldType = "SelectionMark", Value = field.Value.AsSelectionMarkState() },
            DocumentFieldType.Signature => new ProcessedField { FieldType = "Signature", Value = field.Value.AsSignatureType() },
            DocumentFieldType.Currency => ProcessCurrency(field),
            DocumentFieldType.Address => ProcessAddress(field),
            DocumentFieldType.List => ProcessList(field, depth),
            DocumentFieldType.Dictionary => ProcessDictionary(field, depth),
            DocumentFieldType.Unknown => new ProcessedField { FieldType = "Unknown", Value = field.Content },
            _ => new ProcessedField { FieldType = "Unsupported", Value = $"Unsupported Field Type: {field.FieldType}" }
        };
    }

    static ProcessedField ProcessCurrency(DocumentField field)
    {
        var currency = field.Value.AsCurrency();
        return new ProcessedField
        {
            FieldType = "Currency",
            Value = new { currency.Symbol, currency.Amount }
        };
    }

    static ProcessedField ProcessAddress(DocumentField field)
    {
        var address = field.Value.AsAddress();
        return new ProcessedField
        {
            FieldType = "Address",
            Value = new
            {
                address.StreetAddress,
                address.City,
                address.CountryRegion,
                address.PostalCode
            }
        };
    }

    static ProcessedField ProcessList(DocumentField field, int depth)
    {
        var fieldList = field.Value.AsList();
        var processedList = new List<ProcessedField>();

        foreach (var innerField in fieldList)
        {
            processedList.Add(ProcessDocumentFieldRecursive(innerField, depth+1));
        }

        return new ProcessedField
        {
            FieldType = "List",
            ListValue = processedList
        };
    }

    static ProcessedField ProcessDictionary(DocumentField field, int depth)
    {
        var fieldDictionary = field.Value.AsDictionary();
        var processedDictionary = new Dictionary<string, ProcessedField>();

        foreach (var kvp in fieldDictionary)
        {
            processedDictionary[kvp.Key] = ProcessDocumentFieldRecursive(kvp.Value, depth+1);
        }

        return new ProcessedField
        {
            FieldType = "Dictionary",
            DictionaryValue = processedDictionary
        };
    }
}
