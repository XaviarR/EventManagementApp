using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementApp.Models
{
	public class UserModel
	{
		[PrimaryKey, AutoIncrement]
		public int UserID { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		// Cloned to get index, removes item at index then adds back, during update functionality
		public UserModel Clone() => MemberwiseClone() as UserModel;
	}
}
