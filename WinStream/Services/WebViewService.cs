using System.Diagnostics.CodeAnalysis;
using Windows.Web.Http.Filters;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;

using WinStream.Contracts.Services;

namespace WinStream.Services;

public class WebViewService : IWebViewService
{
    private WebView2? _webView;

    public Uri? Source => _webView?.Source;

    [MemberNotNullWhen(true, nameof(_webView))]
    public bool CanGoBack => _webView != null && _webView.CanGoBack;

    [MemberNotNullWhen(true, nameof(_webView))]
    public bool CanGoForward => _webView != null && _webView.CanGoForward;

    public event EventHandler<CoreWebView2WebErrorStatus>? NavigationCompleted;

    [MemberNotNull(nameof(_webView))]
    public void Initialize(WebView2 webView)
    {
        _webView = webView;
        _webView.NavigationCompleted += OnWebViewNavigationCompleted;
    }

    public void GoBack() => _webView?.GoBack();

    public void GoForward() => _webView?.GoForward();

    public void Reload() => _webView?.Reload();

    public void UnregisterEvents()
    {
        if (_webView != null)
        {
            _webView.NavigationCompleted -= OnWebViewNavigationCompleted;
        }
    }

    public void ClearData()
    {
        var filter = new HttpBaseProtocolFilter();
        var cookieManager = filter.CookieManager;
        var cookieJar = cookieManager.GetCookies(new Uri("https://youtube.com"));
        foreach (var cookie in cookieJar)
        {
            cookieManager.DeleteCookie(cookie);
        }
    }

    private void OnWebViewNavigationCompleted(WebView2 sender, CoreWebView2NavigationCompletedEventArgs args) => NavigationCompleted?.Invoke(this, args.WebErrorStatus);
}
