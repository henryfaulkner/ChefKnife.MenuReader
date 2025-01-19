using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChefKnife.MenuReader.DocumentProcessorService;

// Define a class to represent the processed field data
public class ProcessedField
{
    public string FieldType { get; set; }
    public object Value { get; set; }
    public Dictionary<string, ProcessedField> DictionaryValue { get; set; }
    public List<ProcessedField> ListValue { get; set; }
}
