namespace FinaleProject.Model;

public partial class Student : ObservableObject
{
    public int Id { get; set; } = 0;

    [ObservableProperty]
    string username = string.Empty;
    [ObservableProperty]
    string password = string.Empty;
}
public class StudentList
{
    public ObservableCollection<Student> Students { get; set; } = [];

    private static string FileName = @"C:\School\Step\C#\FinaleProject\StudentList.json";
    public static StudentList LoadFromFile()
    {
        if (!File.Exists(FileName)) return new StudentList();
        StudentList? list = JsonConvert.DeserializeObject<StudentList>(File.ReadAllText(FileName));
        return list ?? new StudentList();
    }
    public void SaveToFile()
    {
        File.WriteAllText(FileName, JsonConvert.SerializeObject(this));
    }
}
public class Attempt
{
    public Student Student { get; set; }
    public int QuizId { get; set; }
    public string QuizName { get; set; }
    public string SectionName { get; set; }
    public DateTime AttemptDate { get; set; } = DateTime.Now;
    public int Score { get; set; }
}
public partial class Quiz : ObservableObject
{
    [ObservableProperty]
    int id = 0;

    [ObservableProperty]
    string name = string.Empty;
    public ObservableCollection<int> QuestionIds { get; set; } = [];
    public ObservableCollection<string> QuestionNames { get; set; } = [];
    public ObservableCollection<Attempt> Attempts { get; set; } = [];
}