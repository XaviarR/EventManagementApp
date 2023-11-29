using EventManagementApp.Pages.UserPages;
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

	private async void Button_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.Navigation.PushAsync(new LoginPage());
	}
}

