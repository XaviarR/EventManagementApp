using EventManagementApp.ViewModels;

namespace EventManagementApp.Pages;

public partial class DataVisualizationPage : ContentPage
{
	private readonly EventsViewModel _viewmodel;

	public DataVisualizationPage(EventsViewModel viewmodel)
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

	private async void Button_Clicked(object sender, EventArgs e)
	{
		// Create an instance of AddOrganizerPage
		FormPage formPage = new FormPage();

		// Navigate to AddOrganizerPage
		await Navigation.PushAsync(formPage);
	}
}