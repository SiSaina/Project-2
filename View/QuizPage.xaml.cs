namespace FinaleProject.View;

public partial class QuizPage : ContentPage
{
	public QuizPage(QuizViewModel VM)
	{
		InitializeComponent();
		BindingContext = VM;
	}
}