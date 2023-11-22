﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventManagementApp.Data;
using EventManagementApp.Models;
using EventManagementApp.Pages;
using System.Collections.ObjectModel;

namespace EventManagementApp.ViewModels
{
	public partial class EventsViewModel : ObservableObject
	{
		private readonly DatabaseContext _context;
		public EventsViewModel(DatabaseContext context)
		{
			_context = context;
			OrganizersViewModel = new OrganizersViewModel(context);
			SpeakersViewModel = new SpeakersViewModel(context);
		}
		private OrganizersViewModel _organizersViewModel;

		public OrganizersViewModel OrganizersViewModel
		{
			get => _organizersViewModel;
			set => SetProperty(ref _organizersViewModel, value);
		}
		private SpeakersViewModel _speakersViewModel;

		public SpeakersViewModel SpeakersViewModel
		{
			get => _speakersViewModel;
			set => SetProperty(ref _speakersViewModel, value);
		}


		[ObservableProperty]
		private ObservableCollection<EventsModel> _events;

		[ObservableProperty]
		private EventsModel _operatingEvent = new();

		[ObservableProperty]
		private bool _isBusy;

		[ObservableProperty]
		private string _busyText;

		// Load Logic
		public async Task LoadEventAsync()
		{
			await ExecuteAsync(async () =>
			{
				// Can create as many for each models, TaskModel change to any model
				var tasks = await _context.GetAllAsync<EventsModel>();

				if (tasks != null && tasks.Any())
				{
					// If null create new observable collection
					if (Events == null)
					{
						Events = new ObservableCollection<EventsModel>();
					}

					// Add each task to the observable collection only if it doesn't already exist
					foreach (var task in tasks)
					{
						if (!Events.Any(t => t.EventID == task.EventID))
						{
							Events.Add(task);
						}
					}
				}
			}, "Fetching events...");
		}


		// Update Logic
		[RelayCommand]
		private void SetOperatingEvent(EventsModel? product)
		{
			OperatingEvent = product ?? new();
		}

		// Save Logic, handles both Adding and Updating by if statement
		[RelayCommand]
		private async Task SaveEventAsync()
		{
			if (OperatingEvent == null)
			{
				return;
			}

			// Update BusyText, if Id == 0 then text should display Creating Event, else Updating Event
			var busyText = OperatingEvent.EventID == 0 ? "Creating Event..." : "Updating Event...";

			await ExecuteAsync(async () =>
			{
				// Set the AssignedOrganizer property
				OperatingEvent.AssignedOrganizer = OrganizersViewModel.Organizers.FirstOrDefault(o => o == OperatingEvent.AssignedOrganizer);

				// Set the AssignedSpeaker property
				OperatingEvent.AssignedSpeaker = SpeakersViewModel.Speakers.FirstOrDefault(o => o == OperatingEvent.AssignedSpeaker);

				if (OperatingEvent.EventID == 0)
				{
					// Create Event
					await _context.AddItemAync<EventsModel>(OperatingEvent);
					// Add the new Event to the collection only if it doesn't already exist
					if (!Events.Any(t => t.EventID == OperatingEvent.EventID))
					{
						Events.Add(OperatingEvent);
					}
				}
				else
				{
					// Update Event
					await _context.UpdateItemAync<EventsModel>(OperatingEvent);
					// create clone to keep data
					var eventCopy = OperatingEvent.Clone();
					// Get the index of the item to remove then add back
					var index = Events.IndexOf(OperatingEvent);
					Events.RemoveAt(index);
					Events.Insert(index, eventCopy);
				}

				// Reset data of OperatingEvent
				SetOperatingEvent(OperatingEvent);
			}, busyText);
		}





		// Delete Logic, When Task is deleted the ID is set to auto increment, meaning that it will continue even when task is deleted resulting in gaps of ID integer
		[RelayCommand]
		private async Task DeleteEventAsync(int id)
		{
			await ExecuteAsync(async () =>
			{
				if (await _context.DeleteItemByKeyAync<EventsModel>(id))
				{
					var task = Events.FirstOrDefault(p => p.EventID == id);
					Events.Remove(task);
					await Shell.Current.DisplayAlert("Delete Successful", "Event was successfully deleted", "OK");
				}
				else
				{
					await Shell.Current.DisplayAlert("Delete Error", "Event was unsuccessfully deleted", "OK");
				}
			}, "Deleting Event...");
		}

		// Function to display text based on what CRUD function, Set Manually in code of each Function
		private async Task ExecuteAsync(Func<Task> operation, string? busyText = null)
		{
			IsBusy = true;
			BusyText = busyText ?? "Processing...";
			try
			{
				await operation?.Invoke();
			}
			finally
			{
				IsBusy = false;
				BusyText = "Processing...";
			}
		}
		public async Task LoadEventDetailsAsync(int eventID)
		{
			await ExecuteAsync(async () =>
			{
				var eventDetails = await _context.GetItemByKeyAync<EventsModel>(eventID);

				if (eventDetails != null)
				{
					SelectedEvent = eventDetails; // Update SelectedEvent
				}
			}, "Fetching event details...");
		}


		// Add a property to store the selected EventsModel
		private EventsModel _selectedEvent;

		public EventsModel SelectedEvent
		{
			get => _selectedEvent;
			set => SetProperty(ref _selectedEvent, value);
		}
		[RelayCommand]
		async Task Tap(EventsModel selectedEvent)
		{
			// Set the selected event
			SelectedEvent = selectedEvent;


			// Navigate to the DetailPage and pass the existing EventsViewModel instance
			await Shell.Current.GoToAsync($"{nameof(DetailPage)}", true);

		}

	}
}
