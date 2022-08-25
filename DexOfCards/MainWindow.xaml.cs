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
        services.AddBlazorWebView();
        Resources.Add("services", services.BuildServiceProvider());
        InitializeComponent();
    }
}

public partial class Main { }