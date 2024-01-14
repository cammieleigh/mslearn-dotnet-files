using System.ComponentModel;
using Newtonsoft.Json; 
using System;
using System.Text;

var currentDirectory = Directory.GetCurrentDirectory();
var storesDirectory = Path.Combine(currentDirectory, "stores");

var salesTotalDir = Path.Combine(currentDirectory, "salesTotalDir");
Directory.CreateDirectory(salesTotalDir);   

var salesFiles = FindFiles(storesDirectory);

var salesTotal = CalculateSalesTotal(salesFiles);
var fileTotal = CalculateFileSales(salesFiles);

File.AppendAllText(Path.Combine(salesTotalDir, "totals.txt"), $"{salesTotal}{Environment.NewLine}");

File.AppendAllText(Path.Combine(salesTotalDir, "fileTotals.txt"), $"{fileTotal}{Environment.NewLine}");


IEnumerable<string> FindFiles(string folderName)
{
    List<string> salesFiles = new List<string>();

    var foundFiles = Directory.EnumerateFiles(folderName, "*", SearchOption.AllDirectories);

    foreach (var file in foundFiles)
    {
        var extension = Path.GetExtension(file);
        if (extension == ".json")
        {
            salesFiles.Add(file);
        }
    }

    return salesFiles;
}

double CalculateSalesTotal(IEnumerable<string> salesFiles)
{
    double salesTotal = 0;
    
    // Loop over each file path in salesFiles
    foreach (var file in salesFiles)
    {      
        // Read the contents of the file
        string salesJson = File.ReadAllText(file);
    
        // Parse the contents as JSON
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);
    
        // Add the amount found in the Total field to the salesTotal variable
        
        salesTotal += data?.Total ?? 0;
    }
    string saleTotal = salesTotal.ToString("C");
    return salesTotal;
}

StringBuilder CalculateFileSales(IEnumerable<string> salesFiles)
{
    
    StringBuilder sb = new StringBuilder("Sales Summary \n", 500);
    
    sb.Append("Total Sales: \n");
    sb.Append(salesTotal.ToString("C"));
    sb.Append("Details: \n");
    // Loop over each file path in salesFiles
    foreach (var file in salesFiles)
    {      
        // Read the contents of the file
        string salesJson = File.ReadAllText(file);
    
        // Parse the contents as JSON
        SalesData? data = JsonConvert.DeserializeObject<SalesData?>(salesJson);
        string fileName = Path.GetFileName(file);
        
        
        
        sb.Append(fileName);
        sb.Append(new char[] {':', ' '});
        sb.Append(data?.Total.ToString("C"));
        sb.Append("\n");

    }
    
    return sb;
}


record SalesData (double Total);