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
	public partial class SponsorsViewModel : ObservableObject
	{
		private readonly DatabaseContext _context;
		public SponsorsViewModel(DatabaseContext context)
		{
			_context = context;
		}

		[ObservableProperty]
		private ObservableCollection<SponsorsModel> _sponsors;

		[ObservableProperty]
		private SponsorsModel _operatingSponsor = new();

		[ObservableProperty]
		private bool _isBusy;

		[ObservableProperty]
		private string _busyText;

		// Load Logic
		public async Task LoadSponsorAsync()
		{
			await ExecuteAsync(async () =>
			{
				// Can create as many for each models, TaskModel change to any model
				var tasks = await _context.GetAllAsync<SponsorsModel>();

				if (tasks != null && tasks.Any())
				{
					// If null create new observable collection
					if (Sponsors == null)
					{
						Sponsors = new ObservableCollection<SponsorsModel>();
					}

					// Add each task to the observable collection only if it doesn't already exist
					foreach (var task in tasks)
					{
						if (!Sponsors.Any(t => t.SponsorID == task.SponsorID))
						{
							Sponsors.Add(task);
						}
					}
				}
			}, "Fetching sponsors...");
		}


		// Update Logic
		[RelayCommand]
		private void SetOperatingSponsor(SponsorsModel? product)
		{
			OperatingSponsor = product ?? new();
		}

		// Save Logic, handles both Adding and Updating by if statement
		[RelayCommand]
		private async Task SaveSponsorAsync()
		{
			if (OperatingSponsor == null)
			{
				return;
			}

			// Update BusyText, if Id == 0 then text should display Creating Task, else Updating Task
			var busyText = OperatingSponsor.SponsorID == 0 ? "Creating Sponsor..." : "Updating Sponsor...";

			await ExecuteAsync(async () =>
			{
				if (OperatingSponsor.SponsorID == 0)
				{
					// Create Task
					await _context.AddItemAync<SponsorsModel>(OperatingSponsor);
					// Add the new task to the collection only if it doesn't already exist
					if (!Sponsors.Any(t => t.SponsorID == OperatingSponsor.SponsorID))
					{
						Sponsors.Add(OperatingSponsor);
					}
				}
				else
				{
					// Updating Task
					await _context.UpdateItemAync<SponsorsModel>(OperatingSponsor);
					// create clone to keep data
					var taskCopy = OperatingSponsor.Clone();
					// Get the index of the item to remove then add back
					var index = Sponsors.IndexOf(OperatingSponsor);
					Sponsors.RemoveAt(index);
					Sponsors.Insert(index, taskCopy);
				}

				// Reset data of OperatingTask
				SetOperatingSponsorCommand.Execute(new());
			}, busyText);
		}


		// Delete Logic, When Task is deleted the ID is set to auto increment, meaning that it will continue even when task is deleted resulting in gaps of ID integer
		[RelayCommand]
		private async Task DeleteSpeakerAsync(int id)
		{
			await ExecuteAsync(async () =>
			{
				if (await _context.DeleteItemByKeyAync<OrganizersModel>(id))
				{
					var task = Sponsors.FirstOrDefault(p => p.SponsorID == id);
					Sponsors.Remove(task);
					await Shell.Current.DisplayAlert("Delete Successful", "Sponsor was successfully deleted", "OK");
				}
				else
				{
					await Shell.Current.DisplayAlert("Delete Error", "Sponsor was unsuccessfully deleted", "OK");
				}
			}, "Deleting Sponsor...");
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
