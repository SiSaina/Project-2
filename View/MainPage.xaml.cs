namespace FinaleProject.View;
public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel VM)
    {
        InitializeComponent();
        BindingContext = VM;
    }
}