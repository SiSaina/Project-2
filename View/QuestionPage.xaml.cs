namespace FinaleProject.View;

public partial class QuestionPage : ContentPage
{
	public QuestionPage(QuestionViewModel VM)
	{
		InitializeComponent();
		BindingContext = VM;
	}
}