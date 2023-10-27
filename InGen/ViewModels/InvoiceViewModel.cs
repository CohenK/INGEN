using System.Collections.Generic;
using InGen.Services;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;
using QuestPDF.Previewer;
using CommunityToolkit.Maui.Views;

namespace InGen.ViewModels
{
    public partial class InvoiceViewModel : BaseViewModel
    {
        static Page Page => Application.Current?.MainPage ?? throw new NullReferenceException();

        #region data
        public ObservableCollection<Item> Items { get; set; } = new();
        public ObservableCollection<ItemSold> ItemsSold { get; set; } = new();

        [ObservableProperty]
        public string customerName;
        [ObservableProperty]
        public string customerAddress;
        [ObservableProperty]
        public string customerCity;
        [ObservableProperty]
        public string customerState;
        [ObservableProperty]
        public string customerZip;
        [ObservableProperty]
        public string invoiceNum = "00001";
        [ObservableProperty]
        public double discount = 0;
        [ObservableProperty]
        public string fileName;

        public int inf = int.MaxValue;
        #endregion

        public InvoiceViewModel() { }

        public async Task GetItems()
        {
            await InventoryService.Init();
            try
            {
                var items = await InventoryService.GetItems();
                if (Items.Count != 0) { Items.Clear(); }
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error", "Unable to fetch inventory \n" + ex.Message, "OK");
            }
        }
        public void ClearInvoice()
        {
            ItemsSold.Clear();
            Discount = 0;
            InvoiceNum = "00001";
            CustomerName = "";
            CustomerAddress = "";
            CustomerCity = "";
            CustomerState = "";
            CustomerZip = "";
            FileName = "";
        }

        [RelayCommand]
        private void AddToInvoice(Item item)
        {
            if (item.unitPrice is null) { return; }
            ItemSold itemSold = new();
            itemSold.Name = item.name;
            itemSold.Description = item.description;
            itemSold.UnitName = item.unitName;
            itemSold.UnitPrice = (double)item.unitPrice;
            itemSold.Amount = 1;

            if (ItemsSold.Contains(itemSold))
            {
                Shell.Current.DisplayAlert("Notice", "Item is already in the invoice", "OK");
            }
            else
            {
                ItemsSold.Add(itemSold);
            }
        }
        [RelayCommand]
        private void RemoveItem(ItemSold item)
        {
            ItemsSold.Remove(item);
        }

        [RelayCommand]
        private async Task GetInvoice()
        {
            if (ItemsSold.Count <= 0)
            {
                await Page.DisplayAlert("Alert", "There are no items to create an invoice with", "OK");
                return;
            }

            if (string.IsNullOrEmpty(CustomerName) || string.IsNullOrEmpty(CustomerAddress) || string.IsNullOrEmpty(CustomerCity) || string.IsNullOrEmpty(CustomerState) || string.IsNullOrEmpty(CustomerZip)) 
            {
                await Page.DisplayAlert("Alert", "One or more customer information is empty or invalid", "OK"); 
                return; 
            }

            if (!int.TryParse(InvoiceNum, out _)){
                await Page.DisplayAlert("Alert", "Invoice # can only be an integer", "OK");
                return;
            }
            string invoiceFileName = CustomerName + "#" + InvoiceNum + ".pdf";
            if (!string.IsNullOrEmpty(FileName)) { invoiceFileName = FileName + ".pdf"; }

            string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "InGen", "Invoices");
            if (!Directory.Exists(dir)) { Directory.CreateDirectory(dir); }

            string pdf = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "InGen", "Invoices", invoiceFileName);

            bool answer;
            if (File.Exists(pdf))
            {
                answer = await Page.DisplayAlert("Alert", "A file with the same name already exists. Proceeding will replace the old file with the new file, proceed?", "Yes", "No");
                if (!answer) { return; }
            }

            QuestPDF.Settings.License = LicenseType.Community;

            Company invoiceCustomer = new Company
            {
                Name = CustomerName,
                Address = CustomerAddress,
                City = CustomerCity,
                State = CustomerState,
                ZipCode = CustomerZip
            };

            Invoice InvoiceData = new Invoice(InvoiceNum, invoiceCustomer, DateTime.Today, ItemsSold, discount: Discount);

            var doc = new PDFService(InvoiceData);
            try
            {
                doc.GeneratePdf(pdf);
            }
            catch (Exception ex)
            {
                await Page.DisplayAlert("Alert", "Unable to generate the invoice", "OK");
                await Page.DisplayAlert("Alert", ex.Message, "OK");
            }
            ClearInvoice();
        }

        [RelayCommand]
        private async Task DisplayInvoiceItemsHelp()
        {
            await Page.DisplayAlert("Help", "You can only add integer items. If any decimal is entered for an item, only the whole number value is considered in the generated invoice pdf.\n Additionally, if no items are added then an invoice will not be allowed to generate", "OK");
        }
        [RelayCommand]
        private async Task DisplayDetailsItemsHelp()
        {
            await Page.DisplayAlert("Help", 
                "Items marked with a * is required and the invoice will not be generated unless valid values are entered.\n" +
                "The default value of invoice # is 1 and you are welcomed to change this value as you please, but only integers are allowed\n" +
                "Similary, the default value of discount is 0 and change this as you see fit\n" + 
                "Lastly, the name of the generated invoice will default to the name you entered followed by '#' and the invoice number if no file name is given.", "OK");
        }
    }
}
