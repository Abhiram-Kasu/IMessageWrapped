using Avalonia.Controls;
using AvaloniaHelpers.Navigation.MVVM;
using CommunityToolkit.Mvvm.ComponentModel;

namespace IMessageWrapped.ViewModels;

public partial class MainWindowViewModel : BaseViewModel
{
    [ObservableProperty] private Control _control;

    public MainWindowViewModel()
    {
        App.NavigationService.OnViewChanged += x => Control = x.View;
    }
}