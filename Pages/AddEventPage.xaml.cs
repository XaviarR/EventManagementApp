using EventManagementApp.ViewModels;

namespace EventManagementApp.Pages;

public partial class AddEventPage : ContentPage
{
	private readonly EventsViewModel _viewmodel;
	private readonly OrganizersViewModel _viewmodel2;

	public AddEventPage(EventsViewModel viewmodel)
	{
		InitializeComponent();
		BindingContext = viewmodel;
		_viewmodel = viewmodel;
	}

	protected async override void OnAppearing()
	{
		base.OnAppearing();
		try
		{
			await _viewmodel.LoadEventAsync();
		}
		catch (Exception ex)
		{
			// Handle or log the exception as needed
			Console.WriteLine($"An error occurred: {ex.Message}");
		}
	}

	private async void OrganizerButton_Clicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new AddOrganizerPage(_viewmodel2));
	}
	private async void SpeakerButton_Clicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new AddSpeakerPage());

	}
	private async void SponsorButton_Clicked(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new AddSponsorPage());
	}
}