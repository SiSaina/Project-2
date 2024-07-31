namespace FinaleProject.View;

public partial class StudentDashBoard : ContentPage
{
	public StudentDashBoard(StudentViewModel VM)
	{
		InitializeComponent();
		BindingContext = VM;
	}
}