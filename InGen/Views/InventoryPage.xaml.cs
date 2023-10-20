namespace InGen.Views;

public partial class InventoryPage : ContentPage
{
    public InventoryPage(InventoryViewModel VM)
	{
		InitializeComponent();
        BindingContext = VM;
	}
    protected async override void OnAppearing()
	{
		base.OnAppearing();
		await (BindingContext as InventoryViewModel).GetItems();
	}
}