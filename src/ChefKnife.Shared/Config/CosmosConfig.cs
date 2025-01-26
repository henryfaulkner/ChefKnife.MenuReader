using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefKnife.Shared.Config;

public class CosmosConfig
{
    public CosmosConfig(string accountEndpoint, string accountKey, string dataBase)
    { 
        AccountEndpoint = accountEndpoint;
        AccountKey = accountKey;
        DataBase = dataBase;
    }

    public string AccountEndpoint { get; set; }
    public string AccountKey { get; set; }
    public string DataBase { get; set; }
}
