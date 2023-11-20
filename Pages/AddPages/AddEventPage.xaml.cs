using EventManagementApp.Data;
using EventManagementApp.ViewModels;

namespace EventManagementApp.Pages;

public partial class AddEventPage : ContentPage
{
	private readonly EventsViewModel _viewmodel;

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
		// Create an instance of AddOrganizerPage
		AddOrganizerPage addOrganizerPage = new AddOrganizerPage();

		// Navigate to AddOrganizerPage
		await Navigation.PushAsync(addOrganizerPage);
	}
	private async void SpeakerButton_Clicked(object sender, EventArgs e)
	{
		// Create an instance of AddOrganizerPage
		AddSpeakerPage addOrganizerPage = new AddSpeakerPage();

		// Navigate to AddOrganizerPage
		await Navigation.PushAsync(addOrganizerPage);
	}
	private async void SponsorButton_Clicked(object sender, EventArgs e)
	{
		// Create an instance of AddOrganizerPage
		AddSponsorPage addOrganizerPage = new AddSponsorPage();

		// Navigate to AddOrganizerPage
		await Navigation.PushAsync(addOrganizerPage);
	}
}