namespace InGen.Views;

public partial class InvoicePage : ContentPage
{
	public InvoicePage(InvoiceViewModel VM)
	{
		InitializeComponent();
		BindingContext = VM;
	}
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await (BindingContext as InvoiceViewModel).GetItems();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        (BindingContext as InvoiceViewModel).ClearInvoice();
    }
}