using EventManagementApp.Data;
using EventManagementApp.ViewModels;

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
}