using Windows.Storage.Pickers;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinStream.Core;
using WinStream.ViewModels;
using Windows.Storage;
using System.Diagnostics;
using Microsoft.Graphics.Canvas.Text;

namespace WinStream.Views;

// To learn more about WebView2, see https://docs.microsoft.com/microsoft-edge/webview2/.
public sealed partial class BrowsePage : Page
{
    public BrowseViewModel ViewModel
    {
        get;
    }

    public BrowsePage()
    {
        ViewModel = App.GetService<BrowseViewModel>();
        InitializeComponent();

        ViewModel.WebViewService.Initialize(WebView);
    }

    private async void Download(object sender, RoutedEventArgs routedEventArgs)
    {
        var ensureCorrectInstallation = Downloader.EnsureCorrectInstallation();
        ProgressBar.Visibility = Visibility.Visible;
        var filePicker = new FileSavePicker
        {
            SuggestedStartLocation = PickerLocationId.MusicLibrary,
            CommitButtonText = "Download"
        };
        var li = new List<string>
        {
            ".mp3"
        };
        filePicker.FileTypeChoices.Add("Mp3", li);
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hWnd);
        ProgressBar.ShowPaused = true;
        var file = await filePicker.PickSaveFileAsync();
        ProgressBar.ShowPaused = false;
        if (file.Path != null)
        {
            if (!ensureCorrectInstallation.IsCompleted)
            {
                await ensureCorrectInstallation;
            }
            var url = ViewModel.GetUrl();
            if (url != null)
            {
                await Downloader.EnsureCorrectInstallation();
                var downloader = new Downloader
                {
                    Url = url,
                    Output = file.ToString(),
                    AudioFormat = Downloader.AudioFormats.Mp3,
                    AudioOnly = true,
                    EmbedThumbnail = true
                };
                await downloader.Download();
            }
        }
        ProgressBar.Visibility = Visibility.Collapsed;
    }
}
