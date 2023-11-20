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
	public partial class SpeakersViewModel : ObservableObject
	{
		private readonly DatabaseContext _context;
		public SpeakersViewModel(DatabaseContext context)
		{
			_context = context;
		}

		[ObservableProperty]
		private ObservableCollection<SpeakersModel> _speakers;

		[ObservableProperty]
		private SpeakersModel _operatingSpeaker = new();

		[ObservableProperty]
		private bool _isBusy;

		[ObservableProperty]
		private string _busyText;

		// Load Logic
		public async Task LoadSpeakerAsync()
		{
			await ExecuteAsync(async () =>
			{
				// Can create as many for each models, TaskModel change to any model
				var tasks = await _context.GetAllAsync<SpeakersModel>();

				if (tasks != null && tasks.Any())
				{
					// If null create new observable collection
					if (Speakers == null)
					{
						Speakers = new ObservableCollection<SpeakersModel>();
					}

					// Add each task to the observable collection only if it doesn't already exist
					foreach (var task in tasks)
					{
						if (!Speakers.Any(t => t.SpeakerID == task.SpeakerID))
						{
							Speakers.Add(task);
						}
					}
				}
			}, "Fetching speakers...");
		}


		// Update Logic
		[RelayCommand]
		private void SetOperatingSpeaker(SpeakersModel? product)
		{
			OperatingSpeaker = product ?? new();
		}

		// Save Logic, handles both Adding and Updating by if statement
		[RelayCommand]
		private async Task SaveSpeakerAsync()
		{
			if (OperatingSpeaker == null)
			{
				return;
			}

			// Update BusyText, if Id == 0 then text should display Creating Task, else Updating Task
			var busyText = OperatingSpeaker.SpeakerID == 0 ? "Creating Speaker..." : "Updating Speaker...";

			await ExecuteAsync(async () =>
			{
				if (OperatingSpeaker.SpeakerID == 0)
				{
					// Create Task
					await _context.AddItemAync<SpeakersModel>(OperatingSpeaker);
					// Add the new task to the collection only if it doesn't already exist
					if (!Speakers.Any(t => t.SpeakerID == OperatingSpeaker.SpeakerID))
					{
						Speakers.Add(OperatingSpeaker);
					}
				}
				else
				{
					// Updating Task
					await _context.UpdateItemAync<SpeakersModel>(OperatingSpeaker);
					// create clone to keep data
					var taskCopy = OperatingSpeaker.Clone();
					// Get the index of the item to remove then add back
					var index = Speakers.IndexOf(OperatingSpeaker);
					Speakers.RemoveAt(index);
					Speakers.Insert(index, taskCopy);
				}

				// Reset data of OperatingTask
				SetOperatingSpeaker(OperatingSpeaker);
			}, busyText);
		}


		// Delete Logic, When Task is deleted the ID is set to auto increment, meaning that it will continue even when task is deleted resulting in gaps of ID integer
		[RelayCommand]
		private async Task DeleteSpeakerAsync(int id)
		{
			await ExecuteAsync(async () =>
			{
				if (await _context.DeleteItemByKeyAync<SpeakersModel>(id))
				{
					var task = Speakers.FirstOrDefault(p => p.SpeakerID == id);
					Speakers.Remove(task);
					await Shell.Current.DisplayAlert("Delete Successful", "Speaker was successfully deleted", "OK");
				}
				else
				{
					await Shell.Current.DisplayAlert("Delete Error", "Speaker was unsuccessfully deleted", "OK");
				}
			}, "Deleting Speaker...");
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
