namespace ETAWarrantLookup.Interfaces
{
    /// <summary>
    /// Used for the DI engine to inject datafile path into the contoller
    /// There is also a concrete class in classes for this reason
    /// </summary>
    public interface IWarrantDataFile
    {
        string Name { get; set; }
        string ParentDirectory { get;set; }
    }
}
