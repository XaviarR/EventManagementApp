using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementApp.Models
{
	public class EventsModel
	{
		[PrimaryKey, AutoIncrement]
		public int EventID { get; set; }
		public string EventTitle { get; set; }
		public string EventDescription { get; set; }
		public string EventImage { get; set; }
		public DateTime EventBooking { get; set; }
		public TimeSpan EventBookingTime { get; set; }
		public bool EventAttending { get; set; }
		public string Sponsors { get; set; }
		public string Organizers { get; set; }
		public string Speakers { get; set; }
		// Cloned to get index, removes item at index then adds back, during update functionality
		public EventsModel Clone() => MemberwiseClone() as EventsModel;
	}
}
