namespace InGen.Models
{
    public partial class ItemSold : ObservableObject, IEquatable<ItemSold>
    {
        [ObservableProperty]
        string name;
        [ObservableProperty]
        string description;
        [ObservableProperty]
        public string unitName;
        [ObservableProperty]
        public double unitPrice;
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SubTotal))]
        public double amount;

        public double SubTotal => (int)this.Amount * this.UnitPrice;

        public static bool operator !=(ItemSold item1, ItemSold item2)
        {
            if (item1.Name != item2.Name || item1.Description != item2.Description || item1.UnitName != item2.UnitName || item1.UnitPrice != item2.UnitPrice)
            {
                return true;
            }
            return false;
        }
        public static bool operator ==(ItemSold item1, ItemSold item2)
        {
            if (item1.Name == item2.Name && item1.Description == item2.Description && item1.UnitName == item2.UnitName && item1.UnitPrice == item2.UnitPrice)
            {
                return true;
            }
            return false;
        }
        public bool Equals(ItemSold item)
        {
            return item == this;
        }
        public bool Equals(object obj) => obj is ItemSold && Equals(obj as ItemSold);
    }
}
