using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementApp.Models
{
	public class OrganizersModel
	{
		[PrimaryKey, AutoIncrement] 
		public int OrganizerID { get; set; }
		public string OrganizerName { get; set; }
		public string OrganizerImage { get; set; }
		public string OrganzierDetails { get; set; }
		// Cloned to get index, removes item at index then adds back, during update functionality
		public OrganizersModel Clone() => MemberwiseClone() as OrganizersModel;
	}
}
