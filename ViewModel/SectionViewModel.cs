namespace FinaleProject.ViewModel;

[QueryProperty("Bank", "QuestionBank")]
[QueryProperty(nameof(Section), nameof(Section))]
public partial class SectionViewModel : BaseViewModel
{
    [ObservableProperty]
    QuestionBank bank;

    [ObservableProperty]
    Section section;

    [ObservableProperty]
    ObservableCollection<Question> questions;

    [ObservableProperty]
    ObservableCollection<Quiz> quizzes;

    public SectionViewModel()
    {
        Questions = [];
        Quizzes = [];
    }

    [RelayCommand]
    void AddQuiz()
    {
        if (string.IsNullOrEmpty(Pass)) return;

        int newId = Section.Quizzes.Any() ? Section.Quizzes.Max(q => q.Id) + 1 : 1;
        Quiz quiz = new() { Id = newId, Name = Pass };
        Section.Quizzes.Add(quiz);

        Bank.SaveToFile();
        Pass = string.Empty;
    }
    [RelayCommand]
    async Task ModifyQuiz(Quiz quizz)
    {
        string NewName = await Application.Current.MainPage.DisplayPromptAsync("Edit Quiz Name", "Enter new quiz name:", initialValue: quizz.Name);

        if (!string.IsNullOrWhiteSpace(NewName))
        {
            quizz.Name = NewName;
            Bank.SaveToFile();
        }
    }
    [RelayCommand]
    async Task DeleteQuiz(Quiz quizz)
    {
        bool confirm = await Application.Current.MainPage.DisplayAlert("Confirm Deletion", "Are you sure you want to delete this quiz??", "Yeah", "No");

        if (confirm && Section.Quizzes.Contains(quizz))
        {
            int deletedId = quizz.Id;
            Section.Quizzes.Remove(quizz);
            foreach (var q in Section.Quizzes.Where(q => q.Id > deletedId))
            {
                q.Id--;
            }
            Bank.SaveToFile();
        }
    }

    [RelayCommand]
    void AddQuestion()
    {
        if (string.IsNullOrWhiteSpace(Text)) return;

        int newId = Section.Questions.Any() ? Section.Questions.Max(q => q.Id) + 1 : 1;
        Question ques = new() { Id = newId, Title = Text };
        Section.Questions.Add(ques);

        Bank.SaveToFile();
        Text = string.Empty;
    }
    [RelayCommand]
    async Task ModifyQuestion(Question ques)
    {
        string NewTitle = await Application.Current.MainPage.DisplayPromptAsync("Edit Question Title", "Enter new question title:", initialValue: ques.Title);

        if (!string.IsNullOrWhiteSpace(NewTitle))
        {
            ques.Title = NewTitle;
            Bank.SaveToFile();
        }
    }
    [RelayCommand]
    async Task DeleteQuestion(Question ques)
    {
        bool confirm = await Application.Current.MainPage.DisplayAlert("Confirm Deletion", "Are you sure you want to delete this question??", "Yeah", "No");

        if (confirm && Section.Questions.Contains(ques))
        {
            int deletedId = ques.Id;
            Section.Questions.Remove(ques);
            foreach (var quiz in Section.Quizzes)
            {
                if (quiz.QuestionIds.Contains(deletedId))
                {
                    int index = quiz.QuestionIds.IndexOf(deletedId);
                    quiz.QuestionIds.RemoveAt(index);
                    quiz.QuestionNames.RemoveAt(index);
                }
            }
            foreach (var q in Section.Questions.Where(q => q.Id > deletedId))
            {
                q.Id--;
            }
            Bank.SaveToFile();
        }
    }
    [RelayCommand]
    async Task Save()
    {
        Bank.SaveToFile();
        await Application.Current.MainPage.DisplayAlert("File save", "Saving successfully", "Okazz");
    }

    [RelayCommand]
    async Task NavigateQuestion(int questionId)
    {
        if (questionId <= 0) return;
        Question? question = Section.Questions.FirstOrDefault(q => q.Id == questionId);
        if (question == null) return;

        await Shell.Current.GoToAsync(nameof(QuestionPage), new Dictionary<string, object>
        {
            {"Question", question},
            {"QuestionBank" , Bank }
        });
    }
    [RelayCommand]
    async Task NavigateQuiz(int quizId)
    {
        if (quizId <= 0) return;
        Quiz? quiz = Section.Quizzes.FirstOrDefault(q => q.Id == quizId);
        if (quiz == null) return;

        await Shell.Current.GoToAsync(nameof(QuizPage), new Dictionary<string, object>
        {
            {"Quiz", quiz },
            {"Section", Section},
            {"QuestionBank", Bank }
        });
    }
    [RelayCommand]
    async Task NavigateBack()
    {
        await Shell.Current.GoToAsync("..");
    }
}