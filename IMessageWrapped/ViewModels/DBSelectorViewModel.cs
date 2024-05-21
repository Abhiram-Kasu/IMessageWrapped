using System.IO;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using AvaloniaHelpers.Navigation.MVVM;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IMessageWrapped.Services;

namespace IMessageWrapped.ViewModels;

public partial class DBSelectorViewModel : BaseViewModel
{
    [ObservableProperty] private string _buttonText = "Choose a DB file";

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(Error))]
    private string? _errorMessage;

    public bool Error => !string.IsNullOrWhiteSpace(_errorMessage);


    [RelayCommand]
    public async Task SelectDbFile()
    {
        var res = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            FileTypeFilter = [new FilePickerFileType(".db")],
            AllowMultiple = false
        });

        if (res is { Count: 1 })
        {
            var path = res[0].Path.AbsolutePath;
            switch (DbService.CreateDbService(path))
            {
                case ({ } dbService, _):
                    ErrorMessage = string.Empty;
                    ButtonText = Path.GetFileName(path);
                    GlobalDbService = dbService;
                    await NavigationService.NavigateToAsync(nameof(ResultsView));
                    break;
                case (_, { } error):
                    ErrorMessage = error;
                    break;
            }
        }
        else
        {
            ErrorMessage = "No file or too many files selected";
        }
    }
}