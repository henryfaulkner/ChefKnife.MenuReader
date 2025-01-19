using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ChefKnife.MenuReader.Data.Entities;

public record ReadMenuRequest : BaseEntity, IEntity
{
    // Needs to have an unique Id
    public Guid id { get; set; }

    // Needs to store CreatedDate
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    // Needs to store requested online menu (url)
    public required string MenuUri { get; set; }

    // Needs to store storage blob of requested online menu (downloaded and sent to storage account during request)
    public string? StorageUri { get; set; }

    // Needs to store JSON response from Azure Document Intelligence Service
    public string? ModelResultJSON { get; set; }
}
