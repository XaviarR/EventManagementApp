using EventManagementApp.Data;
using EventManagementApp.ViewModels;
using System.Diagnostics;

namespace EventManagementApp.Pages;

public partial class DetailPage : ContentPage
{
	private readonly EventsViewModel _viewModel;

	public DetailPage(EventsViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;
	}

	protected async override void OnAppearing()
	{
		base.OnAppearing();

		// Check if there is a selected event
		if (_viewModel.SelectedEvent != null)
		{
			await _viewModel.LoadEventDetailsAsync(_viewModel.SelectedEvent.EventID);
			Debug.WriteLine($"SelectedEvent ID: {_viewModel.SelectedEvent.EventID}");
		}
	}
}

