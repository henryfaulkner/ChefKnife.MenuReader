using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefKnife.Shared.Config;

public class AzureDocumentIntelligenceConfig
{
    public AzureDocumentIntelligenceConfig(string endpoint, string apiKey, string modelId)
    {
        Endpoint = endpoint;
        ApiKey = apiKey;
        ModelId = modelId;
    }

    public string Endpoint { get; set; }
    public string ApiKey { get; set; }
    public string ModelId { get; set; }
}
