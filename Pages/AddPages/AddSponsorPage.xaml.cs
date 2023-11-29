using EventManagementApp.Data;
using EventManagementApp.ViewModels;
using Microsoft.Maui.Platform;

namespace EventManagementApp.Pages;

public partial class AddSponsorPage : ContentPage
{
	private readonly SponsorsViewModel _viewmodel;
	public AddSponsorPage() : this(new SponsorsViewModel(new DatabaseContext()))
	{
	}
	public AddSponsorPage(SponsorsViewModel viewmodel)
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
			await _viewmodel.LoadSponsorAsync();
		}
		catch (Exception ex)
		{
			// Handle or log the exception as needed
			Console.WriteLine($"An error occurred: {ex.Message}");
		}
	}
	private async void EditButton_Clicked(object sender, EventArgs e)
	{
		// Scroll to the top of the ScrollView
		await mainScrollView.ScrollToAsync(0, 0, true);
	}
}