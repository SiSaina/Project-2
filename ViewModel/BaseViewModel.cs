namespace FinaleProject.ViewModel;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    string text;

    [ObservableProperty]
    string pass;

    [ObservableProperty]
    string num;
}