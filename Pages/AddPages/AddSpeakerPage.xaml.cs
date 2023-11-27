using EventManagementApp.Data;
using EventManagementApp.ViewModels;
using Microsoft.Maui.Platform;

namespace EventManagementApp.Pages;

public partial class AddSpeakerPage : ContentPage
{
	private readonly SpeakersViewModel _viewmodel;
	public AddSpeakerPage() : this(new SpeakersViewModel(new DatabaseContext()))
	{
	}
	public AddSpeakerPage(SpeakersViewModel viewmodel)
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
			await _viewmodel.LoadSpeakerAsync();
		}
		catch (Exception ex)
		{
			// Handle or log the exception as needed
			Console.WriteLine($"An error occurred: {ex.Message}");
		}
	}
	private async void EditButton_Clicked(object sender, EventArgs e)
	{
		// Your existing logic to handle editing...

		// Scroll to the top of the ScrollView
		await mainScrollView.ScrollToAsync(0, 0, true);
	}
}