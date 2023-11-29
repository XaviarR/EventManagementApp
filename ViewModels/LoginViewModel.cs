using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventManagementApp.Data;
using EventManagementApp.Models;
using System.Collections.ObjectModel;

namespace EventManagementApp.ViewModels
{
	public partial class LoginViewModel : ObservableObject
	{
		private readonly DatabaseContext _context;
		public LoginViewModel(DatabaseContext context)
		{
			//Create instance
			_context = context;
		}
		[ObservableProperty]
		private ObservableCollection<UserModel> _users;

		[ObservableProperty]
		private UserModel _operatingUser = new();

		[ObservableProperty]
		private bool _isBusy;

		[ObservableProperty]
		private string _busyText;

		// Load Logic
		public async Task LoadUserAsync()
		{
			await ExecuteAsync(async () =>
			{
				// Can create as many for each models, TaskModel change to any model
				var tasks = await _context.GetAllAsync<UserModel>();

				if (tasks != null && tasks.Any())
				{
					// If null create new observable collection
					if (Users == null)
					{
						Users = new ObservableCollection<UserModel>();
					}

					// Add each task to the observable collection only if it doesn't already exist
					foreach (var task in tasks)
					{
						if (!Users.Any(t => t.UserID == task.UserID))
						{
							Users.Add(task);
						}
					}
				}
			}, "Fetching organizers...");
		}


		// Update Logic
		[RelayCommand]
		private void SetOperatingUser(UserModel? userModel)
		{
			OperatingUser = userModel ?? new();
		}

		// Save Logic, handles both Adding and Updating by if statement
		[RelayCommand]
		private async Task SaveUserAsync()
		{
			if (OperatingUser == null)
			{
				return;
			}

			// Update BusyText, if Id == 0 then text should display Creating Task, else Updating Task
			var busyText = OperatingUser.UserID == 0 ? "Creating User..." : "Updating User...";

			await ExecuteAsync(async () =>
			{
				if (OperatingUser.UserID == 0)
				{
					// Create Task
					await _context.AddItemAync<UserModel>(OperatingUser);
					// Add the new task to the collection only if it doesn't already exist
					if (!Users.Any(t => t.UserID == OperatingUser.UserID))
					{
						Users.Add(OperatingUser);
						await Shell.Current.DisplayAlert("Registered Successfully", "", "Ok");
						// Navigate back to the previous page
						await Shell.Current.Navigation.PopAsync();
					}
				}
				else
				{
					// Updating Task
					await _context.UpdateItemAync<UserModel>(OperatingUser);
					// create clone to keep data
					var taskCopy = OperatingUser.Clone();
					// Get the index of the item to remove then add back
					var index = Users.IndexOf(OperatingUser);
					Users.RemoveAt(index);
					Users.Insert(index, taskCopy);
				}

				// Reset data of OperatingTask
				SetOperatingUser(OperatingUser);
			}, busyText);
		}


		// Delete Logic, When Task is deleted the ID is set to auto increment, meaning that it will continue even when task is deleted resulting in gaps of ID integer
		[RelayCommand]
		private async Task DeleteUserAsync(int id)
		{
			await ExecuteAsync(async () =>
			{
				if (await _context.DeleteItemByKeyAync<UserModel>(id))
				{
					var task = Users.FirstOrDefault(p => p.UserID == id);
					Users.Remove(task);
					await Shell.Current.DisplayAlert("Delete Successful", "User was successfully deleted", "OK");
				}
				else
				{
					await Shell.Current.DisplayAlert("Delete Error", "User was unsuccessfully deleted", "OK");
				}
			}, "Deleting User...");
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

		// Login Logic
		[RelayCommand]
		private async Task LoginAsync()
		{
			if (string.IsNullOrEmpty(OperatingUser.Email) || string.IsNullOrEmpty(OperatingUser.Password))
			{
				// Handle empty email or password
				await Shell.Current.DisplayAlert("Login Failed", "Invalid email or password", "OK");
				return;
			}

			await ExecuteAsync(async () =>
			{
				// Retrieve user based on email
				var user = await _context.GetFilteredAsync<UserModel>(u => u.Email == OperatingUser.Email);

				if (user.Any() && user.First().Password == OperatingUser.Password)
				{
					// Login successful, navigate to the main application screen or perform other actions
					await Shell.Current.DisplayAlert("Login Successful", "Welcome!", "OK");
					// Navigate back to the previous page
					await Shell.Current.Navigation.PopAsync();
				}
				else
				{
					// Login failed, inform the user
					await Shell.Current.DisplayAlert("Login Failed", "Invalid email or password", "OK");
				}
			}, "Logging in...");
		}
	}
}