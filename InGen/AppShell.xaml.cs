using InGen.Views;

namespace InGen;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(CompanyPage), typeof(CompanyPage));
        Routing.RegisterRoute(nameof(InventoryPage), typeof(InventoryPage));
        Routing.RegisterRoute(nameof(InvoicePage), typeof(InvoicePage));
    }
}
