using InGen.Services;

namespace InGen.ViewModels
{
    public partial class InventoryViewModel : BaseViewModel
    {
        static Page Page => Application.Current?.MainPage ?? throw new NullReferenceException();

        #region private vars
        [ObservableProperty]
        private string name;
        [ObservableProperty]
        private string description;
        [ObservableProperty]
        private string unitName;
        [ObservableProperty]
        private double? unitPrice;
        [ObservableProperty]
        private int? id;
        public ObservableCollection<Item> Items { get; set; } = new();
        #endregion

        public InventoryViewModel() 
        {
            Title = "Inventory Page";
        }

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

        [RelayCommand]
        private void PopulateFields(Item item)
        {
            if (item == null)
            {
                return;
            }
            ClearFields();
            Name = item.name;
            Description = item.description;
            UnitName = item.unitName;
            UnitPrice = item.unitPrice;
            Id = item.id;
        }

        [RelayCommand]
        private void ClearFields()
        {
            Name = null;
            Description = null;
            UnitName = null;
            UnitPrice = null;
            Id = null;
        }

        [RelayCommand]
        private async Task AddItem()
        {
            if (Name == null || UnitName == null || UnitPrice == null)
            {
                await Page.DisplayAlert("Alert", "One or more fields in the item form is empty", "OK");
                return;
            }
            if ((double)UnitPrice < 0.0)
            {
                await Page.DisplayAlert("Alert", "Unit Price must be non-negative", "OK");
                return;
            }
            await InventoryService.Init();
            Item item = new Item();
            item.name = Name;
            item.description = Description;
            item.unitName = UnitName;
            item.unitPrice = UnitPrice;
            await InventoryService.AddItem(item);
            ClearFields();
            await GetItems();
        }

        [RelayCommand]
        private async Task EditItem()
        {
            if (Name == null || UnitName == null || UnitPrice == null || Id is null)
            {
                await Page.DisplayAlert("Alert", "One or more fields in the item form is empty", "OK");
                return;
            }
            Item item = new Item();
            item.name = Name;
            item.description = Description;
            item.unitName = UnitName;
            item.unitPrice = UnitPrice;
            item.id = Id;
            await InventoryService.UpdateItem(item);
            ClearFields();
            await GetItems();
        }

        [RelayCommand]
        private async Task RemoveItem()
        {
            if (Name == null || UnitName == null || UnitPrice == null || Id is null)
            {
                await Page.DisplayAlert("Alert", "Please select an item from the list to delete", "OK");
                return;
            }
            await InventoryService.Init();
            await InventoryService.RemoveItem(Id);
            ClearFields();
            await GetItems();
        }

        [RelayCommand]
        private async Task DisplayHelp()
        {
            await Page.DisplayAlert("Help",
                "Items marked with a * is required and the item will not be added unless valid values are entered.\n" +
                "Additionally, unit cost must not be negative.", "OK");
        }

    }
}