
namespace XmlConfiguratorBase.DTO
{
    /// <summary>
    /// possible values for entry types
    /// </summary>
    public enum EntryType : int
    {
        Liste = 0,
        Details = 1,
        Dashboard = 2,
        Wizard = 3,
    }

    /// <summary>
    /// possible types for custom gui view data fields
    /// </summary>
    enum InternalType
    {
        String,
        Long,
        DateTime,
        Int,
        Double,
        Byte
    }

    /// <summary>
    /// possible gui types
    /// </summary>
    enum ViewType
    {
        text, 
        boolean, 
        time, 
        date, 
        currency, 
        separator, 
        number, 
        xpro, 
        headline
    }

    /// <summary>
    /// where the data comes from
    /// </summary>
    public enum DataSource
    {
        NO_SOURCE,
        XML_FILE,
        DATABASE,
        CUSTOM,
    }

    /// <summary>
    /// When data is read, shall we clear the list or add the items?
    /// If we add them and an element exists in both the list and the data source, which one wins?
    /// </summary>
    public enum DataReadMode : int
    {
        GIVEN_SOURCE_ONLY = 0,
        ADD_DATA_IF_NOT_EXISTING,
        OVERWRITE_IF_EXISTING,
    }

    public enum CommandLine
    {
        workflow,
        cas,
        olstart,
    }

    /// <summary>
    /// Possible view modes for a wfv entry
    /// </summary>
    public enum ViewMode
    {
        SOURCE,
        PROPERTYGRID,
        VISUAL,
    }
}
