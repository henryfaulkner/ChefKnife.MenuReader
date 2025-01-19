using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefKnife.MenuReader.Data.Entities;

public record MenuReadRequest : BaseEntity, IEntity
{
    // Needs to have an unique Id
    // Needs to store CreatedDate
    // Needs to store requested online menu (url)
    // Needs to store storage blob of requested online menu (downloaded and sent to storage account during request)
    // Needs to store JSON response from Azure Document Intelligence Service
}
