using EventManagementApp.ViewModels;

namespace EventManagementApp.Pages;

public partial class AddOrganizerPage : ContentPage
{
	private readonly OrganizersViewModel _viewmodel;

	public AddOrganizerPage(OrganizersViewModel viewmodel)
	{
		InitializeComponent();
	}
}