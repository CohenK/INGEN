using InGen.Services;
using Windows.System.Update;

namespace InGen.ViewModels
{
    public partial class InventoryViewModel : BaseViewModel
    {
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
        public void PopulateFields(Item item)
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
        public async Task AddItem()
        {
            if (Name == null || UnitName == null || UnitPrice == null)
            {
                ClearFields();
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
        public async Task EditItem()
        {
            if (Name == null || UnitName == null || UnitPrice == null || Id is null)
            {
                ClearFields();
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
        public async Task RemoveItem()
        {
            if (Name == null || UnitName == null || UnitPrice == null || Id is null)
            {
                ClearFields();
                return;
            }
            await InventoryService.Init();
            await InventoryService.RemoveItem(Id);
            ClearFields();
            await GetItems();
        }


    }
}