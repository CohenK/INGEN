using InGen.Views;
using System.Windows.Input;


namespace InGen.ViewModels
{
    public partial class FrontPageViewModel :BaseViewModel
    {
        [RelayCommand]
        async Task CompanyPage()
        {
            await Shell.Current.GoToAsync($"{nameof(CompanyPage)}");
        }
        [RelayCommand]
        async Task InventoryPage()
        {
            await Shell.Current.GoToAsync($"{nameof(InventoryPage)}");
        }

        [RelayCommand]
        async Task InvoicePage()
        {
            await Shell.Current.GoToAsync($"{nameof(InvoicePage)}");
        }

    }
}
