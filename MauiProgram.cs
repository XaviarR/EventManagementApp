using EventManagementApp.Data;
using EventManagementApp.Pages;
using EventManagementApp.ViewModels;
using Microsoft.Extensions.Logging;

namespace EventManagementApp;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
		builder.Services.AddSingleton<DatabaseContext>();
		builder.Services.AddSingleton<EventsViewModel>();

		builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
		// Main Pages
		builder.Services.AddSingleton<MainPage>();
		builder.Services.AddTransient<DetailPage>();
		builder.Services.AddTransient<DataVisualizationPage>();
		// Add Pages
		builder.Services.AddTransient<AddEventPage>();


		return builder.Build();
	}
}
