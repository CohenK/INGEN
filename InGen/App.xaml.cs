using InGen.Views;

namespace InGen;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

        Application.Current.UserAppTheme = AppTheme.Light;

        MainPage = new AppShell();
    }
}
