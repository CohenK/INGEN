using InGen.ViewModels;

namespace InGen;

public partial class MainPage : ContentPage
{
	public MainPage(FrontPageViewModel VM)
    {
        InitializeComponent();
        BindingContext = VM;
    }

}

