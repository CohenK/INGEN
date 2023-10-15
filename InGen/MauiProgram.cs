using CommunityToolkit.Maui;
using InGen.Services;
using InGen.ViewModels;
using InGen.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InGen;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("Quattrocento-Regular.ttf", "Quattrocento-Regular");
				fonts.AddFont("Quattrocento-Bold.ttf", "Quattrocento-Bold");
				fonts.AddFont("Italiana-Regular.ttf", "Italiana-Regular");
				fonts.AddFont("fontello.ttf", "Icons");
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<CompanyPage>();
        builder.Services.AddTransient<InventoryPage>();
        builder.Services.AddTransient<InvoicePage>();

        builder.Services.AddSingleton<FrontPageViewModel>();

        builder.Services.AddSingleton<CompanyService>();
        builder.Services.AddTransient<CompanyViewModel>();

        //builder.Services.AddSingleton<InventoryService>();
		builder.Services.AddTransient<InventoryViewModel>();

        builder.Services.AddSingleton<InvoiceViewModel>();

        return builder.Build();
	}
}
