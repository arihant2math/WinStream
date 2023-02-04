using System.Reflection;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;

using Windows.ApplicationModel;
using WinStream.Contracts.Services;
using WinStream.Helpers;
using WinStream.Views;

namespace WinStream.ViewModels;

public class SettingsViewModel : ObservableRecipient
{
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly IDataSourceSelectorService _dataSourcesSelectorService;
    private readonly IWebViewService _webViewService;
    private ElementTheme _elementTheme;
    private string _versionDescription;

    public ElementTheme ElementTheme
    {
        get => _elementTheme;
        set => SetProperty(ref _elementTheme, value);
    }

    public string VersionDescription
    {
        get => _versionDescription;
        set => SetProperty(ref _versionDescription, value);
    }

    public ICommand SwitchThemeCommand
    {
        get;
    }

    public SettingsViewModel(IThemeSelectorService themeSelectorService, IDataSourceSelectorService dataSourceSelectorService, IWebViewService webViewService)
    {
        _themeSelectorService = themeSelectorService;
        _dataSourcesSelectorService = dataSourceSelectorService;
        _webViewService = webViewService;
        _elementTheme = _themeSelectorService.Theme;
        _versionDescription = GetVersionDescription();

        SwitchThemeCommand = new RelayCommand<ElementTheme>(
            async param =>
            {
                if (ElementTheme == param)
                {
                    return;
                }

                ElementTheme = param;
                await _themeSelectorService.SetThemeAsync(param);
            });
    }

    private static string GetVersionDescription()
    {
        Version version;

        if (RuntimeHelper.IsMSIX)
        {
            var packageVersion = Package.Current.Id.Version;

            version = new Version(packageVersion.Major, packageVersion.Minor, packageVersion.Build, packageVersion.Revision);
        }
        else
        {
            version = Assembly.GetExecutingAssembly().GetName().Version!;
        }

        return $"{"AppDisplayName".GetLocalized()} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
    }

    public List<DataSource> GetDataSourcesList()
    {
        var sources = _dataSourcesSelectorService.DataSources;
        return sources.Select(source => new DataSource(source)).ToList();
    }

    public async Task AppendSource(string source)
    {
        await _dataSourcesSelectorService.AddDirectoryAsync(source);
    }

    public async Task RemoveSource(string source)
    {
        await _dataSourcesSelectorService.RemoveDirectoryAsync(source);
    }

    public void ClearWebviewData(object sender, RoutedEventArgs e)
    {
        _webViewService.ClearData();
    }
}
