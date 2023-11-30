using EventManagementApp.Data;
using EventManagementApp.ViewModels;
using Microsoft.Maui.Platform;

namespace EventManagementApp.Pages.UserPages;

public partial class RegisterPage : ContentPage
{
	private readonly LoginViewModel _viewmodel;
	public RegisterPage() : this(new LoginViewModel(new DatabaseContext()))
	{
	}
	public RegisterPage(LoginViewModel viewmodel)
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
			await _viewmodel.LoadUserAsync();
		}
		catch (Exception ex)
		{
			// Handle or log the exception as needed
			Console.WriteLine($"An error occurred: {ex.Message}");
		}
	}

}