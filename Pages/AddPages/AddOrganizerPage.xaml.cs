using EventManagementApp.Data;
using EventManagementApp.ViewModels;
using Microsoft.Maui.Platform;

namespace EventManagementApp.Pages;

public partial class AddOrganizerPage : ContentPage
{
	private readonly OrganizersViewModel _viewmodel;
	public AddOrganizerPage() : this(new OrganizersViewModel(new DatabaseContext()))
	{
	}
	public AddOrganizerPage(OrganizersViewModel viewmodel)
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
			await _viewmodel.LoadOrganizerAsync();
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