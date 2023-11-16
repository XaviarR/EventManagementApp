using EventManagementApp.ViewModels;

namespace EventManagementApp.Pages;

public partial class MainPage : ContentPage
{
	private readonly EventsViewModel _viewmodel;

	public MainPage(EventsViewModel viewmodel)
	{
		InitializeComponent();
		BindingContext = viewmodel;
		_viewmodel = viewmodel;
	}

	protected async override void OnAppearing()
	{
		base.OnAppearing();
		await _viewmodel.LoadEventAsync();
	}
}

