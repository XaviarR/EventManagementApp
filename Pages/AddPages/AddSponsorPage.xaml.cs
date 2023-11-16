using EventManagementApp.Data;
using EventManagementApp.ViewModels;

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
}