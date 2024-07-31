namespace FinaleProject.View;

public partial class SectionPage : ContentPage
{
	public SectionPage(SectionViewModel VM)
	{
		InitializeComponent();
		BindingContext = VM;
	}
}