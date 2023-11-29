using EventManagementApp.Data;
using EventManagementApp.ViewModels;
using Microsoft.Maui.Platform;

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
			await _viewmodel.OrganizersViewModel.LoadOrganizerAsync();
			await _viewmodel.SpeakersViewModel.LoadSpeakerAsync();
			await _viewmodel.SponsorsViewModel.LoadSponsorAsync();
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
	private async void EditButton_Clicked(object sender, EventArgs e)
	{
		// Scroll to the top of the ScrollView
		await mainScrollView.ScrollToAsync(0, 0, true);
	}
}