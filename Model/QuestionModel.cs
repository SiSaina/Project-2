namespace FinaleProject.Model;

public partial class Question : ObservableObject
{
    [ObservableProperty]
    int id = 0;

    [ObservableProperty]
    string title = string.Empty;

    public ObservableCollection<string> Answer { get; set; } = [];
    public ObservableCollection<string> CorrectAnswer { get; set; } = [];
}
public partial class Section : ObservableObject
{
    [ObservableProperty]
    string name = string.Empty;

    [ObservableProperty]
    int id;

    public ObservableCollection<Question> Questions { get; set; } = [];
    public ObservableCollection<Quiz> Quizzes { get; set; } = [];
}
public class QuestionBank
{
    public ObservableCollection<Section> Sections { get; set; } = [];
    public ObservableCollection<Attempt> Attempts { get; set; } = [];

    private static string FileName = @"C:\School\Step\C#\FinaleProject\QuestionBank.json";
    //Load from File function
    public static QuestionBank LoadFromFile()
    {
        if (!File.Exists(FileName)) return new QuestionBank();
        QuestionBank? bank = JsonConvert.DeserializeObject<QuestionBank>(File.ReadAllText(FileName));
        return bank ?? new QuestionBank();
    }
    //Save to File function
    public void SaveToFile()
    {
        File.WriteAllText(FileName, JsonConvert.SerializeObject(this));
    }
}