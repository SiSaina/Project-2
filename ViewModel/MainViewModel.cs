namespace FinaleProject.ViewModel;

public partial class MainViewModel : BaseViewModel
{
    [ObservableProperty]
    StudentList list;
    [ObservableProperty]
    Student students;
    [ObservableProperty]
    QuestionBank bank;

    [RelayCommand]
    void SignUp()
    {
        if (string.IsNullOrWhiteSpace(Text) || string.IsNullOrWhiteSpace(Pass))
        {
            Application.Current?.MainPage?.DisplayAlert("Error", "Please enter a text", "OKazz");
            return;
        }
        if (!int.TryParse(Num, out int stuId) || stuId <= 0)
        {
            Application.Current?.MainPage?.DisplayAlert("Error", "Please enter a valid numeric student ID", "OKazz");
            return;
        }
        if (Text.Length > 20 || Pass.Length > 20)
        {
            Application.Current?.MainPage?.DisplayAlert("Error", "Username and Password must be between 1 to 20 characters", "OKazz");
            return;
        }
        if (!IsUsernameTaken(Text))
        {
            Application.Current?.MainPage?.DisplayAlert("Error", "Username is taken", "OK");
            return;
        }

        int newId = List.Students.Any() ? List.Students.Max(s => s.Id) + 1 : 1;
        Student stu = new()
        {
            Id = newId,
            Username = Text,
            Password = Pass,
        };
        List.Students.Add(stu);
        List.SaveToFile();

        Application.Current?.MainPage?.DisplayAlert("New student",
            $"ID: {stu.Id}\n" +
            $"Username: {stu.Username}\n" +
            $"Password: {stu.Password}\n", "Okazz");

        Text = string.Empty;
        Pass = string.Empty;
    }

    [RelayCommand]
    void SignIn(string num)
    {
        int.TryParse(num, out int studentId);

        if (string.IsNullOrWhiteSpace(Text) || string.IsNullOrWhiteSpace(Pass))
        {
            Application.Current?.MainPage?.DisplayAlert("Error", "Please enter a text", "OKazz");
            return;
        }
        if (Text == "Teacher" && Pass == "123")
        {
            Shell.Current.GoToAsync(nameof(TeacherDashBoard), new Dictionary<string, object>
            {
                {"QuestionBank", Bank }
            });
            return;
        }
        if (studentId <= 0)
        {
            Application.Current?.MainPage?.DisplayAlert("Error", "Please enter a valid numeric student ID", "OKazz");
            return;
        }
        if (!CheckStudent(studentId, Text, Pass))
        {
            Application.Current?.MainPage?.DisplayAlert("Error", "Invalid username or password", "Okazz");
            return;
        }

        Student? student = List.Students.FirstOrDefault(s => s.Id == studentId);
        if (student == null) return;

        Shell.Current.GoToAsync(nameof(StudentDashBoard), new Dictionary<string, object>
        {
            {"Student", student },
            {"StudentList", List },
            {"QuestionBank", Bank }
        });
    }
    

    public bool IsUsernameTaken(string name)
    {
        return !List.Students.Any(s => s.Username == name);
    }
    public bool CheckStudent(int id, string name, string pass)
    {
        foreach (Student stu in List.Students)
        {
            if (stu.Id == id && stu.Username == name && stu.Password == pass)
            {
                return true;
            }
        }
        return false;
    }
    public MainViewModel()
    {
        Students = new();
        List = StudentList.LoadFromFile();
        Bank = QuestionBank.LoadFromFile();
    }
}