using InGen.Services;
using System.Windows.Input;

namespace InGen.ViewModels
{
    public partial class CompanyViewModel : BaseViewModel
    {
        //static string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

        CompanyService service;
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
        private int zipCode;
        [ObservableProperty]
        private double tax;
        #endregion

        public CompanyViewModel(CompanyService service)
        {
            this.service = service;
            company = new Company();
            try
            {
                company = service.GetCompany();
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
        public void UpdateCompany()
        {
            company.Name = Name;
            company.Address = Address;
            company.City = City;
            company.State = State;
            company.ZipCode = ZipCode;
            company.Tax = Tax;
            service.SetCompany();
        }
    }
}
