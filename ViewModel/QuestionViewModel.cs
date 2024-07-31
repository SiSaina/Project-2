namespace FinaleProject.ViewModel;

[QueryProperty("Bank", "QuestionBank")]
[QueryProperty(nameof(Question),nameof(Question))]
public partial class QuestionViewModel : BaseViewModel
{
    [ObservableProperty]
    Question question;

    [ObservableProperty]
    QuestionBank bank;

    [RelayCommand]
    void AddAnswer()
    {
        if (string.IsNullOrEmpty(Text))
        {
            Application.Current?.MainPage?.DisplayAlert("Error", "Please enter an answer", "OK");
            return;
        }
        if (Question.Answer.Count >= 4)
        {
            Application.Current?.MainPage?.DisplayAlert("Error", "You can only add up to 4 answer.", "OKazz");
            return;
        }

        Question.Answer.Add(Text);
        Bank.SaveToFile();
        Text = string.Empty;
    }
    [RelayCommand]
    void AddCorrectAnswer()
    {
        if (string.IsNullOrEmpty(Text))
        {
            Application.Current?.MainPage?.DisplayAlert("Error", "Please enter an answer", "OK");
            return;
        }
        if (Question.CorrectAnswer.Count >= 4)
        {
            Application.Current?.MainPage?.DisplayAlert("Error", "You can only add up to 4 correct answer.", "OKazz");
            return;
        }

        Question.CorrectAnswer.Add(Text);
        Bank.SaveToFile();
        Text = string.Empty;
    }
    [RelayCommand]
    void DeleteAnswer(string ans)
    {
        Question.Answer.Remove(ans);
        Bank.SaveToFile();
    }
    [RelayCommand]
    void DeleteCorrectAnswer(string ans)
    {
        Question.CorrectAnswer.Remove(ans);
        Bank.SaveToFile();
    }
    [RelayCommand]
    async Task NavigateBack()
    {
        await Shell.Current.GoToAsync("..");
    }
    [RelayCommand]
    async Task NavigateDashBoard()
    {
        await Shell.Current.GoToAsync("../..");
    }
}