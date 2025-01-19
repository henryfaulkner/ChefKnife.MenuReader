using Ardalis.ApiEndpoints;
using Ardalis.Result.AspNetCore;
using Ardalis.Result;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using ChefKnife.MenuReader.Shared.DTOs;
using ChefKnife.MenuReader.Data.Repositories;
using ChefKnife.MenuReader.Data.Entities;
using ChefKnife.MenuReader.StorageService;
using System;
using System.Security.Policy;

namespace ChefKnife.MenuReader.WebAPI.Endpoints;

public record ReadMenuModel
{
    [FromQuery(Name = "menuUri")]
    public string MenuUri { get; set; }
}

public class ReadMenu : EndpointBaseAsync
    .WithRequest<ReadMenuModel>
    .WithActionResult<IEnumerable<MenuItemDetail>>
{
    readonly IRepository<ReadMenuRequest> _readMenuRequestRepository;
    readonly IStorageService _storageService;

    public ReadMenu(IRepository<ReadMenuRequest> readMenuRequestRepository, IStorageService storageService)
    {  
        _readMenuRequestRepository = readMenuRequestRepository; 
        _storageService = storageService;
    }

    [HttpGet(ApiEndpoints.ReadMenu)]
    [SwaggerOperation(
        Summary = ".",
        Description = ".",
        OperationId = "ReadMenu",
        Tags = new[] { "MenuReader" }
    )]
    public override async Task<ActionResult<IEnumerable<MenuItemDetail>>> HandleAsync(
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
            Uri storageUri = await _storageService.UploadAsync(Path.GetFileName(req.MenuUri), menuData); 

            var res = new List<MenuItemDetail>();

            await _readMenuRequestRepository.AddAsync(new ReadMenuRequest() { 
                MenuUri = req.MenuUri,
                StorageUri = storageUri.ToString(),
                ModelResultJSON = string.Empty,
            });

            return this.ToActionResult(Result.Success(res.AsEnumerable()));
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

}
