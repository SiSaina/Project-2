namespace FinaleProject.View;

public partial class TeacherDashBoard : ContentPage
{
	public TeacherDashBoard(TeacherViewModel VM)
	{
		InitializeComponent();
		BindingContext = VM;
	}
}