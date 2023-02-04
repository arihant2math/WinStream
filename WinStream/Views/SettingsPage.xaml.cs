using System.Diagnostics;
using Windows.Storage.Pickers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using WinStream.ViewModels;

namespace WinStream.Views;

public class DataSource
{
    public string Path { get; private set; }

    public DataSource(string path)
    {
        Path = path;
    }
}

// TODO: Set the URL for your privacy policy by updating SettingsPage_PrivacyTermsLink.NavigateUri in Resources.resw.
public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel
    {
        get;
    }

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>();
        InitializeComponent();
        DataSourcesListView.ItemsSource = ViewModel.GetDataSourcesList();
    }

    public async void AddDataSource(object sender, RoutedEventArgs e)
    {
        var folderPicker = new FolderPicker();
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hWnd);
        var folder = await folderPicker.PickSingleFolderAsync();
        if (folder == null || folder.Path == null)
        {
            return;
        }

        await ViewModel.AppendSource(folder.Path);
        DataSourcesListView.ItemsSource = ViewModel.GetDataSourcesList();
    }

    public async void RemoveDataSource(object sender, RoutedEventArgs e)
    {
        if (DataSourcesListView.SelectedItem is DataSource dataSource)
        {
            await ViewModel.RemoveSource(dataSource.Path);
        }
        DataSourcesListView.ItemsSource = ViewModel.GetDataSourcesList();
    }
}
