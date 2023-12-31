﻿using EventManagementApp.Pages;
using EventManagementApp.Pages.UserPages;

namespace EventManagementApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
		Routing.RegisterRoute(nameof(DetailPage), typeof(DetailPage));
		Routing.RegisterRoute(nameof(DataVisualizationPage), typeof(DataVisualizationPage));
		Routing.RegisterRoute(nameof(AddEventPage), typeof(AddEventPage));
		Routing.RegisterRoute(nameof(AddOrganizerPage), typeof(AddOrganizerPage));
		Routing.RegisterRoute(nameof(AddSpeakerPage), typeof(AddSpeakerPage));
		Routing.RegisterRoute(nameof(AddSponsorPage), typeof(AddSponsorPage));
		Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
		Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
	}
}
