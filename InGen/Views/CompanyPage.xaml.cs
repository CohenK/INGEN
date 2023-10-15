using InGen.ViewModels;

namespace InGen.Views;

public partial class CompanyPage : ContentPage
{
	public CompanyPage(CompanyViewModel VM)
	{
		InitializeComponent();

		BindingContext = VM;
	}
}