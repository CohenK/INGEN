using SQLite;

namespace InGen.Services
{
    public static class InventoryService
    {
        static SQLiteAsyncConnection db;
        public static async Task Init()
        {
            if (db != null) return;
            var databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "InGen", "CompanyInfo", "Inventory.db");
            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<Item>();
        }

        public static async Task AddItem(Item item)
        {
            await Init();
            await db.InsertAsync(item);
        }
        public static async Task RemoveItem(int? id)
        {
            await Init();
            if (id is null) return;
            await db.DeleteAsync<Item>(id);
        }
        public static async Task UpdateItem(Item item)
        {
            await Init();
            if(item is null) return;
            await db.UpdateAsync(item);
        }

        public static async Task<IEnumerable<Item>> GetItems()
        {
            await Init();
            var ItemList = await db.Table<Item>().ToListAsync();
            return ItemList;
        }
    }
}
