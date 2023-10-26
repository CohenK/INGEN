using InGen.Services;
using System.Windows.Input;

namespace InGen.ViewModels
{
    public partial class CompanyViewModel : BaseViewModel
    {
        static Page Page => Application.Current?.MainPage ?? throw new NullReferenceException();
        public Company company { get; set; }

        #region private vars
        [ObservableProperty]
        private string name;
        [ObservableProperty]
        private string address;
        [ObservableProperty]
        private string city;
        [ObservableProperty]
        private string state;
        [ObservableProperty]
        private string zipCode;
        [ObservableProperty]
        private double tax;
        #endregion

        public CompanyViewModel()
        {
            company = new Company();
            try
            {
                company = CompanyService.GetCompany();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                Shell.Current.DisplayAlert("Error", "Unable to fetch company data \n" + ex.Message, "OK");
            }
            finally
            {
                Name = company.Name;
                Address = company.Address;
                City = company.City;
                State = company.State;
                Tax = company.Tax;
                ZipCode = company.ZipCode;
            }

        }

        [RelayCommand]
        public async Task UpdateCompany()
        {
            if (Tax < 0 || Tax > 100) 
            { 
                await Page.DisplayAlert("Alert", "Tax percentage should only be within the range of 0 to 100 inclusive", "OK"); 
                return; 
            }
            company.Name = Name;
            company.Address = Address;
            company.City = City;
            company.State = State;
            company.ZipCode = ZipCode;
            company.Tax = Tax;
            CompanyService.SetCompany();
        }
    }
}
