using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AvaloniaHelpers.Navigation.MVVM;
using IMessageWrapped.Models;

namespace IMessageWrapped.ViewModels;

public class ResultsViewModel : BaseViewModel
{
    public ObservableCollection<QueryResult> QueryResults { get; set; } = [];

    public override async Task OnNavigatedToAsync(BaseViewModel? prevViewModel)
    {
        await base.OnNavigatedToAsync(prevViewModel);
        if (GlobalDbService is { } dbService)
            await foreach (var qResult in GlobalDbService.GetQueriesAsync())
                QueryResults.Add(qResult);
    }
}