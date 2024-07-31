namespace FinaleProject.ViewModel;

[QueryProperty("Bank", "QuestionBank")]
[QueryProperty("List", "StudentList")]
[QueryProperty(nameof(Student), nameof(Student))]

public partial class StudentViewModel : BaseViewModel
{
    [ObservableProperty]
    QuestionBank? bank;
    [ObservableProperty]
    Student? student;
    [ObservableProperty]
    StudentList? list;

    [ObservableProperty]
    Section? showSection;
    [ObservableProperty]
    Quiz? showQuiz;

    //Navigate between page
    [ObservableProperty]
    bool isNav;
    [ObservableProperty]
    bool isHomePage;
    [ObservableProperty]
    bool isSectionPage;
    [ObservableProperty]
    bool isQuizPage;
    [ObservableProperty]
    bool isQuestionPage;
    [ObservableProperty]
    bool isHistoryPage;
    [ObservableProperty]
    bool isLeaderBoardPage;
    [ObservableProperty]
    bool isSettingPage;
    [ObservableProperty]
    bool isPage;

    //Answering questions
    Random random = new Random();
    [ObservableProperty]
    ObservableCollection<Question> questions;
    [ObservableProperty]
    Question currentQuestion;
    [ObservableProperty]
    int index;
    [ObservableProperty]
    int score;
    [ObservableProperty]
    ObservableCollection<string> shuffledAllAnswers;
    [ObservableProperty]
    bool isAnswerCorrect;
    [ObservableProperty]
    bool isStartButtonEnabled;

    //History & LeaderBoard
    [ObservableProperty]
    ObservableCollection<Attempt>? att;

    [RelayCommand]
    private void NavigateBetween(string name)
    {
        switch (name)
        {
            case "Home":
                IsHomePage = true;
                IsSectionPage = false;
                IsHistoryPage = false;
                IsLeaderBoardPage = false;
                IsSettingPage = false;
                break;
            case "Start":
                IsPage = true;
                NavigateToSection(name);
                break;
            case "History":
                IsHomePage = false;
                IsSectionPage = false;
                IsHistoryPage = true;
                IsLeaderBoardPage = false;
                IsSettingPage = false;
                Refresh("History");
                break;
            case "LeaderBoard":
                IsPage = false;
                NavigateToSection(name);
                Refresh("LeaderBoard");
                break;
            case "Setting":
                IsHomePage = false;
                IsSectionPage = false;
                IsHistoryPage = false;
                IsLeaderBoardPage = false;
                IsSettingPage = true;
                break;
            case "Logout":
                Shell.Current.GoToAsync("..");
                break;
        }
    }
    [RelayCommand]
    private void NavigateToSection(string name)
    {
        IsNav = !IsNav;
        IsHomePage = !IsHomePage;
        IsSectionPage = !IsSectionPage;
    }
    [RelayCommand]
    private void NavigateToQuiz(string name)
    {
        IsSectionPage = !IsSectionPage;
        IsQuizPage = !IsQuizPage;

        GetSectionName(name);
    }
    [RelayCommand]
    private void NavigateToPage(string name)
    {
        IsQuizPage = !IsQuizPage;
        if (IsPage)
        {
            GetQuizName(name);
            if (ShowQuiz?.QuestionIds.Count < 20)
            {
                ToggleVisible();
            }
            else
            {
                IsQuestionPage = !IsQuestionPage;
                Application.Current?.MainPage?.DisplayAlert("Starting...", 
                    "Enter full detail answers\n" +
                    "If there are more then one answers\n" +
                    "Use comma to separate the answers\n" +
                    "Good Luck.", 
                    "Okazz");
            }
        }
        else
        {
            ToggleVisible();
            IsHomePage = !IsHomePage;
            IsLeaderBoardPage = !IsLeaderBoardPage;
            GetQuizName(name);
        }
    }
    [RelayCommand]
    private void Back(string name)
    {
        if(name == "Subject")
        {
            IsNav = !IsNav;
            IsHomePage = !IsHomePage;
            IsSectionPage = !IsSectionPage;
        }
        else
        {
            IsSectionPage = !IsSectionPage;
            IsQuizPage = !IsQuizPage;
        }
    }
    private void GetSectionName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return;

        ShowSection = Bank?.Sections.FirstOrDefault(s => s.Name == name);

        if(ShowSection?.Quizzes.Count == 0)
        {
            Application.Current?.MainPage?.DisplayAlert("No Quizzes", "There are no quizzes available in this section.", "Okazz");
        }
    }
    private void GetQuizName(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return;

        ShowQuiz = ShowSection?.Quizzes.FirstOrDefault(q => q.Name == name);

        if (ShowQuiz?.QuestionIds.Count < 20)
        {
            Application.Current?.MainPage?.DisplayAlert("No Questions", "There aren't questions available in this moment.", "Okazz");
        }
    }

    //Answering Question
    private bool SubmitAnswer()
    {
        if (ShowQuiz != null && ShowQuiz.QuestionIds != null && Index < ShowQuiz.QuestionIds.Count)
        {
            var correctAnswers = ShowSection?.Questions[Index].CorrectAnswer;

            var enteredAnswers = Text.Split(',');

            enteredAnswers = enteredAnswers.Select(answer => answer.Trim()).ToArray();

            return correctAnswers.All(correctAnswer => enteredAnswers.Contains(correctAnswer));
        }
        return false;
    }
    [RelayCommand]
    void StartQuestion()
    {
        Index = 0;
        int? questionId = ShowQuiz?.QuestionIds[Index];

        Question? currentQuestion = ShowSection?.Questions.FirstOrDefault(q => q.Id == questionId);

        if (currentQuestion != null)
        {
            List<string> allAnswers = new(currentQuestion.Answer);
            allAnswers.AddRange(currentQuestion.CorrectAnswer);

            ShuffledAllAnswers = new ObservableCollection<string>(allAnswers.OrderBy(x => random.Next()));

            CurrentQuestion = new Question
            {
                Id = currentQuestion.Id,
                Title = currentQuestion.Title,
                Answer = ShuffledAllAnswers
            };
        }
        IsStartButtonEnabled = false;
    }
    [RelayCommand]
    private void NextQuestion()
    {
        Index++;
        if (ShowQuiz != null && ShowQuiz.QuestionIds != null && Index < ShowQuiz.QuestionIds.Count)
        {
            List<string>? allAnswers = new(ShowSection.Questions[Index].Answer);
            allAnswers.AddRange(ShowSection.Questions[Index].CorrectAnswer);

            ShuffledAllAnswers = new ObservableCollection<string>(allAnswers.OrderBy(x => random.Next()));

            CurrentQuestion = new Question
            {
                Id = ShowSection.Questions[Index].Id,
                Title = ShowSection.Questions[Index].Title,
                Answer = ShuffledAllAnswers
            };
        }
    }
    [RelayCommand]
    void Submit()
    {
        if (IsStartButtonEnabled)
        {
            Application.Current?.MainPage?.DisplayAlert("Error", "Please press the start button", "Okazz");
            return;
        }
        if (SubmitAnswer())
        {
            Score++;
        }

        NextQuestion();

        if (Index > ShowQuiz?.QuestionIds.Count)
        {
            Application.Current?.MainPage?.DisplayAlert("Quiz finish", $"Congratulation! You score: {Score}", "Okazz");

            Attempt Attempt = new Attempt
            {
                Student = Student,
                QuizId = ShowQuiz.Id,
                QuizName = ShowQuiz.Name,
                SectionName = ShowSection.Name,
                Score = Score,
                AttemptDate = DateTime.Now,
            };
            Bank?.Attempts.Add(Attempt);
            ShowQuiz.Attempts.Add(Attempt);
            Bank?.SaveToFile();

            string attemptDetails = $"Attempt Details:\nStudent: {Attempt?.Student?.Username}\n" +
                                    $"Quiz: {Attempt?.QuizName}\n" +
                                    $"Score: {Attempt?.Score}\n" +
                                    $"Attempt Date: {Attempt?.AttemptDate}";

            Application.Current?.MainPage?.DisplayAlert("Attempt Information", attemptDetails, "Ok");
            RestartQuiz();
        }
    }
    private void RestartQuiz()
    {
        Index = 0;
        Score = 0;
        ShuffledAllAnswers.Clear();
        CurrentQuestion = null;
        IsStartButtonEnabled = true;
        ToggleVisible();
    }

    //History & LeaderBoard
    [RelayCommand]
    void Refresh(string name)
    {
        Att = [];
        if(name == "History")
        {
            foreach (Attempt attempt in Bank.Attempts)
            {
                if (attempt.Student.Username == Student?.Username)
                {
                    Attempt newAttempt = new Attempt
                    {
                        Student = new Student
                        {
                            Username = attempt.Student.Username
                        },
                        QuizName = attempt.QuizName,
                        SectionName = attempt.SectionName,
                        AttemptDate = attempt.AttemptDate,
                        Score = attempt.Score
                    };
                    Att.Add(newAttempt);
                }
            }
        }
        else
        {
            foreach (Attempt attempt in Bank.Attempts)
            {
                if (attempt.SectionName == ShowSection?.Name && attempt.QuizName == ShowQuiz?.Name)
                {
                    Attempt newAttempt = new()
                    {
                        Student = new Student
                        {
                            Username = attempt.Student.Username
                        },
                        QuizName = ShowQuiz.Name,
                        SectionName = ShowSection.Name,
                        AttemptDate = attempt.AttemptDate,
                        Score = attempt.Score
                    };
                    Att.Add(newAttempt);
                }
            }
            ObservableCollection<Attempt> top20Students = new ObservableCollection<Attempt>(
                Att.OrderByDescending(a => a.Score)
                    .ThenByDescending(a => a.AttemptDate)
                    .Take(20));
            Att.Clear();
            foreach (var attempt in top20Students)
            {
                Att.Add(attempt);
            }
        }
    }

    //Setting
    [RelayCommand]
    async Task Modify(string property)
    {
        string newValue;
        if (property == Student?.Username)
        {
            newValue = await Application.Current.MainPage.DisplayPromptAsync("Edit Username", "Enter a new username", initialValue: Student.Username);

            if (!string.IsNullOrEmpty(newValue))
            {
                Student.Username = newValue;
            }
        }
        else
        {
            newValue = await Application.Current.MainPage.DisplayPromptAsync("Edit Password", "Enter a new password", initialValue: Student.Password);

            if (!string.IsNullOrEmpty(newValue))
            {
                Student.Password = newValue;
            }
        }
        List?.SaveToFile();
        Bank?.SaveToFile();
        QuestionBank.LoadFromFile();
    }

    private void ToggleVisible()
    {
        IsNav = true;
        IsHomePage = true;
        IsSectionPage = false;
        IsQuizPage = false;
        IsQuestionPage = false;
        IsHistoryPage = false;
        IsLeaderBoardPage = false;
        IsSettingPage = false;
    }
    public StudentViewModel()
    {
        ToggleVisible();

        Score = 0;
        ShuffledAllAnswers = [];
        IsStartButtonEnabled = true;
    }
}