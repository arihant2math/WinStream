namespace WinStream.Contracts.Services;

public interface IDataSourceSelectorService
{
    List<string> DataSources
    {
        get;
        set;
    }
    Task InitializeAsync();

    Task AddDirectoryAsync(string directory);
    
    Task RemoveDirectoryAsync(string directory);
}
