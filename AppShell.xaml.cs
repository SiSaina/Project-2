namespace FinaleProject
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(TeacherDashBoard), typeof(TeacherDashBoard));
            Routing.RegisterRoute(nameof(SectionPage), typeof(SectionPage));
            Routing.RegisterRoute(nameof(QuizPage), typeof(QuizPage));
            Routing.RegisterRoute(nameof(QuestionPage), typeof(QuestionPage));

            Routing.RegisterRoute(nameof(StudentDashBoard), typeof(StudentDashBoard));
        }
    }
}
