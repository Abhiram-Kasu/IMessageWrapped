using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using AvaloniaHelpers.Navigation;
using IMessageWrapped.Services;

namespace IMessageWrapped;

public class App : Application
{
    public static readonly NavigationService NavigationService = new();
    public static IStorageProvider StorageProvider { get; private set; }
    public static DbService? GlobalDbService { get; set; }


    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel()
            };
            StorageProvider = desktop.MainWindow.StorageProvider;
        }

        base.OnFrameworkInitializationCompleted();

        NavigationService.RegisterView<DBSelectorView, DBSelectorViewModel>(nameof(DBSelectorView));
        NavigationService.RegisterView<ResultsView, ResultsViewModel>(nameof(ResultsView));
        NavigationService.NavigateToAsync(nameof(DBSelectorView));
    }
}