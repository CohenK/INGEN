using InGen.Services;
using System.Collections;

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
        private double unitPrice;
        [ObservableProperty]
        private ObservableCollection<Item> items = new ObservableCollection<Item>();
        #endregion

        public InventoryViewModel() { }

        [RelayCommand]
        public async Task GetItems()
        {
            await InventoryService.Init();
            try
            {
                IEnumerable<Item> IEnum = await InventoryService.GetItems();
                Items = new ObservableCollection<Item>(IEnum);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error", "Unable to fetch inventory \n" + ex.Message, "OK");
            }
        }

        [RelayCommand]
        public async Task AddItem()
        {
            await InventoryService.Init();
            Item item = new Item();
            item.name = Name;
            item.description = Description;
            item.unitName = UnitName;
            item.unitPrice = UnitPrice;
            await InventoryService.AddItem(item);
            GetItems();
        }


        [RelayCommand]
        public async Task RemoveItem(int id)
        {
            await InventoryService.Init();
            await InventoryService.RemoveItem(id);
            GetItems();
        }


    }
}
