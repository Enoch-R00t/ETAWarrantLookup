namespace ETAWarrantLookup.Classes
{
    /// <summary>
    /// This class is used by the framework to retrieve the data file location from the appsettings.json file
    /// </summary>
    public class WarrantDataFile : Interfaces.IWarrantDataFile
    {
        public string Name { get; set; }
        public string ParentDirectory { get;set; }
    }
}
