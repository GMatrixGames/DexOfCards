using DexOfCards.Framework.Data;
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
        Resources.Add("services", services.BuildServiceProvider());
        InitializeComponent();

        DataStorage.Init();
    }
}