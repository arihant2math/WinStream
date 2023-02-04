using Microsoft.UI.Xaml;

using WinStream.Contracts.Services;
using WinStream.Helpers;

namespace WinStream.Services;

public class DataSourceSelectorService : IDataSourceSelectorService
{

    private string SettingsKey = "datasources";
    public List<string> DataSources { get; set; }

    private readonly ILocalSettingsService _localSettingsService;

    public DataSourceSelectorService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
        DataSources = new List<string>();
    }

    public async Task InitializeAsync()
    {
        DataSources = await LoadDataSourcesFromSettingsAsync();
        await Task.CompletedTask;
    }

    public async Task AddDirectoryAsync(string dir)
    {
        DataSources.Add(dir);
        await SaveDataSourcesInSettingsAsync();
    }

    public async Task RemoveDirectoryAsync(string dir)
    {
        DataSources.Remove(dir);
        await SaveDataSourcesInSettingsAsync();
    }
    private async Task<List<string>> LoadDataSourcesFromSettingsAsync()
    {
        var result = await _localSettingsService.ReadSettingAsync<string>(SettingsKey);

        if (result != null)
        {
            var split = result.Split(",");
            return split.ToList();
        }

        var defaultList = new List<string>();
        defaultList.Add(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
        return defaultList;
    }

    private async Task SaveDataSourcesInSettingsAsync()
    {
        await _localSettingsService.SaveSettingAsync(SettingsKey, string.Join(",", DataSources));
    }
}
