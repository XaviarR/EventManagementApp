using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventManagementApp.Data;
using EventManagementApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementApp.ViewModels
{
	public partial class OrganizersViewModel : ObservableObject
	{
		private readonly DatabaseContext _context;
		public OrganizersViewModel(DatabaseContext context)
		{
			_context = context;
		}

		[ObservableProperty]
		private ObservableCollection<OrganizersModel> _organizers;

		[ObservableProperty]
		private OrganizersModel _operatingOrganizer = new();

		[ObservableProperty]
		private bool _isBusy;

		[ObservableProperty]
		private string _busyText;

		// Load Logic
		public async Task LoadOrganizerAsync()
		{
			await ExecuteAsync(async () =>
			{
				// Can create as many for each models, TaskModel change to any model
				var tasks = await _context.GetAllAsync<OrganizersModel>();

				if (tasks != null && tasks.Any())
				{
					// If null create new observable collection
					if (Organizers == null)
					{
						Organizers = new ObservableCollection<OrganizersModel>();
					}

					// Add each task to the observable collection only if it doesn't already exist
					foreach (var task in tasks)
					{
						if (!Organizers.Any(t => t.OrganizerID == task.OrganizerID))
						{
							Organizers.Add(task);
						}
					}
				}
			}, "Fetching organizers...");
		}


		// Update Logic
		[RelayCommand]
		private void SetOperatingOrganizer(OrganizersModel? product)
		{
			OperatingOrganizer = product ?? new();
		}

		// Save Logic, handles both Adding and Updating by if statement
		[RelayCommand]
		private async Task SaveOrganizerAsync()
		{
			if (OperatingOrganizer == null)
			{
				return;
			}

			// Update BusyText, if Id == 0 then text should display Creating Task, else Updating Task
			var busyText = OperatingOrganizer.OrganizerID == 0 ? "Creating Event..." : "Updating Event...";

			await ExecuteAsync(async () =>
			{
				if (OperatingOrganizer.OrganizerID == 0)
				{
					// Create Task
					await _context.AddItemAync<OrganizersModel>(OperatingOrganizer);
					// Add the new task to the collection only if it doesn't already exist
					if (!Organizers.Any(t => t.OrganizerID == OperatingOrganizer.OrganizerID))
					{
						Organizers.Add(OperatingOrganizer);
					}
				}
				else
				{
					// Updating Task
					await _context.UpdateItemAync<OrganizersModel>(OperatingOrganizer);
					// create clone to keep data
					var taskCopy = OperatingOrganizer.Clone();
					// Get the index of the item to remove then add back
					var index = Organizers.IndexOf(OperatingOrganizer);
					Organizers.RemoveAt(index);
					Organizers.Insert(index, taskCopy);
				}

				// Reset data of OperatingTask
				SetOperatingOrganizerCommand.Execute(new());
			}, busyText);
		}


		// Delete Logic, When Task is deleted the ID is set to auto increment, meaning that it will continue even when task is deleted resulting in gaps of ID integer
		[RelayCommand]
		private async Task DeleteOrganizerAsync(int id)
		{
			await ExecuteAsync(async () =>
			{
				if (await _context.DeleteItemByKeyAync<OrganizersModel>(id))
				{
					var task = Organizers.FirstOrDefault(p => p.OrganizerID == id);
					Organizers.Remove(task);
					await Shell.Current.DisplayAlert("Delete Successful", "Organizer was successfully deleted", "OK");
				}
				else
				{
					await Shell.Current.DisplayAlert("Delete Error", "Organizer was unsuccessfully deleted", "OK");
				}
			}, "Deleting Organizer...");
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
	}
}
