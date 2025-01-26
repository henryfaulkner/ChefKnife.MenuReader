using Ardalis.ApiEndpoints;
using Ardalis.Result.AspNetCore;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ChefKnife.MenuReader.WebAPI.DTOs;
using ChefKnife.MenuReader.Data.Repositories;
using ChefKnife.MenuReader.Data.Entities;
using ChefKnife.MenuReader.StorageService;
using System;
using System.Security.Policy;
using ChefKnife.MenuReader.DocumentProcessorService;
using Newtonsoft.Json;

namespace ChefKnife.MenuReader.WebAPI.Endpoints;

public record ReadMenuModel
{
    [FromQuery(Name = "menuUri")]
    public string MenuUri { get; set; }
}

public class ReadMenu : EndpointBaseAsync
    .WithRequest<ReadMenuModel>
    .WithActionResult<Menu>
{
    readonly IRepository<ReadMenuRequest> _readMenuRequestRepository;
    readonly IStorageService _storageService;
    readonly IDocumentProcessorService _documentProcessorService;

    public ReadMenu(IRepository<ReadMenuRequest> readMenuRequestRepository, IStorageService storageService, IDocumentProcessorService documentProcessorService)
    {  
        _readMenuRequestRepository = readMenuRequestRepository; 
        _storageService = storageService;
        _documentProcessorService = documentProcessorService;
    }

    [HttpGet(ApiEndpoints.ReadMenu)]
    [SwaggerOperation(
        Summary = ".",
        Description = ".",
        OperationId = "ReadMenu",
        Tags = new[] { "MenuReader" }
    )]
    public override async Task<ActionResult<Menu>> HandleAsync(
        ReadMenuModel req,
        CancellationToken cancellationToken = default)
    {
        try
        {
            if (!IsValidFileType(req.MenuUri)) 
                return this.ToActionResult(
                    Result.Invalid(new ValidationError("Menu's file is not an accepted format. The accepted formats are .pdf and .docx."))
                );

            byte[] menuData = await _storageService.DownloadAsync(new Uri(req.MenuUri));

            // Upload menu file to long-term storage
            Uri? storageUri = await _storageService.UploadAsync(Path.GetFileName(req.MenuUri), menuData);

            // Extract data from menu file
            Dictionary<string, ProcessedField> modelResult = await _documentProcessorService.ExtractDataFromDocumentAsync(new MemoryStream(menuData), cancellationToken);

            // Upload request record to database
            await _readMenuRequestRepository.AddAsync(new ReadMenuRequest() { 
                MenuUri = req.MenuUri,
                StorageUri = storageUri?.ToString(),
                ModelResult = JsonConvert.SerializeObject(modelResult, Formatting.Indented),
            });

            var res = ConvertToMenu(modelResult);

            return this.ToActionResult(Result.Success(res));
        }
        catch (Exception ex)
        {
            return this.ToActionResult(Result.Error($"Exception occurred: {ex.Message}"));
        }
    }

    static bool IsValidFileType(string uri)
    {
        string extension = Path.GetExtension(uri);

        // Check if the file has a valid extension
        return extension == ".pdf" || extension == ".docx";
    }

    static Menu ConvertToMenu(Dictionary<string, ProcessedField> processedFields)
    {
        var menu = new Menu();

        foreach (var field in processedFields)
        {
            switch (field.Key.ToLower())
            {
                case "titles":
                    menu.Titles = ConvertToTitles(field.Value);
                    break;

                case "items_col1":
                    menu.ItemsCol1 = ConvertToMenuItems(field.Value);
                    break;

                case "items_col2":
                    menu.ItemsCol2 = ConvertToMenuItems(field.Value);
                    break;

                case "items_col3":
                    menu.ItemsCol3 = ConvertToMenuItems(field.Value);
                    break;

                case "items_col4":
                    menu.ItemsCol4 = ConvertToMenuItems(field.Value);
                    break;

                default:
                    // Handle any other fields if needed
                    break;
            }
        }

        return menu;
    }

    static IEnumerable<string> ConvertToTitles(ProcessedField field)
    {
        var result = new List<string>();

        if (field.FieldType == "List" && field.ListValue != null)
        {
            foreach (var listItem in field.ListValue)
            {
                if (listItem.FieldType == "Dictionary" && listItem.DictionaryValue != null)
                {
                    string title = null;
                    foreach (var kvp in listItem.DictionaryValue)
                    {
                        switch (kvp.Key.ToLower())
                        {
                            case "name":
                                title = kvp.Value?.Value?.ToString();
                                break;
                        }
                    }
                    if (title != null) result.Add(title);
                }
            }
        }

        return result;
    }

    static IEnumerable<MenuItem> ConvertToMenuItems(ProcessedField field)
    {
        var result = new List<MenuItem>();

        if (field.FieldType == "List" && field.ListValue != null)
        {
            foreach (var listItem in field.ListValue)
            {
                if (listItem.FieldType == "Dictionary" && listItem.DictionaryValue != null)
                {
                    var menuItem = new MenuItem();
                    foreach (var kvp in listItem.DictionaryValue)
                    {
                        switch (kvp.Key.ToLower())
                        {
                            case "name":
                                menuItem.Name = kvp.Value?.Value?.ToString();
                                break;
                            case "price":
                                if (kvp.Value != null &&  kvp.Value.Value != null)
                                    menuItem.Price = Convert.ToDouble(kvp.Value.Value);
                                else
                                    menuItem.Price = null;
                                break;
                            case "ingredients":
                                menuItem.Ingredients = kvp.Value?.Value?.ToString();
                                break;
                            default:
                                break;
                        }
                    }
                    result.Add(menuItem);
                }
            }
        }

        return result;
    }
}
