using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefKnife.Shared.Config;

public class AzureBlobConfig
{
    public AzureBlobConfig(string connectionString, string containerName)
    {
        ConnectionString = connectionString;
        ContainerName = containerName;
    }

    public string ConnectionString { get; set; }
    public string ContainerName { get; set; }
}
