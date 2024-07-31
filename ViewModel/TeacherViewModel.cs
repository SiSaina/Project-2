namespace FinaleProject.ViewModel;

[QueryProperty("Bank", "QuestionBank")]
public partial class TeacherViewModel : BaseViewModel
{
    [ObservableProperty]
    QuestionBank bank;

    [RelayCommand]
    void Add()
    {
        if (string.IsNullOrEmpty(Text)) return;

        int newId = Bank.Sections.Any() ? Bank.Sections.Max(q => q.Id) + 1 : 1;
        Section sect = new() { Id = newId, Name = Text };


        Bank.Sections.Add(sect);
        Bank.SaveToFile();
        Text = string.Empty;
    }

    [RelayCommand]
    async Task Modify(Section sect)
    {
        string NewName = await Application.Current.MainPage.DisplayPromptAsync("Edit section name", "Please enter new section name", initialValue: sect.Name);
        if (!string.IsNullOrEmpty(NewName))
        {
            sect.Name = NewName;
            Bank.SaveToFile();
        }
    }

    [RelayCommand]
    async Task Delete(Section sect)
    {
        bool confirm = await Application.Current.MainPage.DisplayAlert("Confirm Deletion", "Are you sure you want to delete this section", "Yea", "No");

        if (confirm && Bank.Sections.Contains(sect))
        {
            int deleteId = sect.Id;
            Bank.Sections.Remove(sect);
            foreach (var s in Bank.Sections.Where(s => s.Id > deleteId))
            {
                s.Id--;
            }
            Bank.SaveToFile();
        }
    }
    [RelayCommand]
    void Save()
    {
        Bank.SaveToFile();
        Application.Current?.MainPage?.DisplayAlert("File", "File save successfully", "Okazz");
    }

    [RelayCommand]
    async Task NavigateForward(int sectionId)
    {
        if (sectionId <= 0) return;
        Section? section = Bank.Sections.FirstOrDefault(s => s.Id == sectionId);
        if (section == null) return;

        await Shell.Current.GoToAsync(nameof(SectionPage), new Dictionary<string, object>
        {
            { "Section" , section },
            {"QuestionBank", Bank }
        });
    }
    [RelayCommand]
    void Logout() 
    {
        Shell.Current.GoToAsync("..");
    }
}
