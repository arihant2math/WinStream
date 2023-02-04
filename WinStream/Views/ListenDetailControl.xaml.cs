using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Windows.Media.Core;
using Windows.Media.Playback;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using WinStream.Core.Models;

namespace WinStream.Views;

public sealed partial class ListDetailsDetailControl : UserControl
{
    public Song? ListDetailsMenuItem
    {
        get => GetValue(ListDetailsMenuItemProperty) as Song;
        set => SetValue(ListDetailsMenuItemProperty, value);
    }

    public static readonly DependencyProperty ListDetailsMenuItemProperty = 
        DependencyProperty.Register("ListDetailsMenuItem", typeof(Song), 
            typeof(ListDetailsDetailControl), 
            new PropertyMetadata(null, OnListDetailsMenuItemPropertyChanged));
    
    public ListDetailsDetailControl()
    {
        InitializeComponent();
    }

    private static void OnListDetailsMenuItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ListDetailsDetailControl control)
        {
            control.ForegroundElement.ChangeView(0, 0, 1);
        }
    }
}
