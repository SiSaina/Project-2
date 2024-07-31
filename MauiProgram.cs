using Microsoft.Extensions.Logging;

namespace FinaleProject
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();

            builder.Services.AddSingleton<TeacherDashBoard>();
            builder.Services.AddSingleton<TeacherViewModel>();

            builder.Services.AddTransient<SectionPage>();
            builder.Services.AddTransient<SectionViewModel>();

            builder.Services.AddTransient<QuizPage>();
            builder.Services.AddTransient<QuizViewModel>();

            builder.Services.AddTransient<QuestionPage>();
            builder.Services.AddTransient<QuestionViewModel>();

            builder.Services.AddTransient<StudentDashBoard>();
            builder.Services.AddTransient<StudentViewModel>();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
