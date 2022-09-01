using System.IO;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.Bootstrap;
using Blazorise.Icons.FontAwesome;
using DexOfCards.Framework.Data;
using DexOfCards.Utilities;
using Microsoft.AspNetCore.Components.WebView;
using Microsoft.Extensions.DependencyInjection;

namespace DexOfCards;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    public MainWindow()
    {
        var services = new ServiceCollection();
        services.AddWpfBlazorWebView();
#if DEBUG
        services.AddBlazorWebViewDeveloperTools();
#endif
        services.AddBlazorise(opt => opt.Immediate = true)
            .AddBootstrap5Providers()
            .AddBootstrap5Components()
            .AddBootstrapIcons()
            .AddFontAwesomeIcons();

        Resources.Add("services", services.BuildServiceProvider());
        InitializeComponent();

        DataStorage.Init();
        
        if (File.Exists(Path.Combine(FilePaths.AppData, "user_store.s3db"))) return;
        File.Copy(Path.Combine(FilePaths.Resources, "DEFAULT-user_storage.s3db"), Path.Combine(FilePaths.AppData, "user_store.s3db"));
    }

    private void BlazorWebViewInitialized(object sender, BlazorWebViewInitializedEventArgs e)
    {
#if !DEBUG
        e.WebView.CoreWebView2.Settings.IsZoomControlEnabled = false;
#endif
    }
}