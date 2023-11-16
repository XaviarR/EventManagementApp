using EventManagementApp.Data;
using EventManagementApp.ViewModels;

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
}