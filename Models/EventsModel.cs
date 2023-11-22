using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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

		// Cloned to get index, removes item at index then adds back, during update functionality
		public EventsModel Clone() => MemberwiseClone() as EventsModel;

		[Ignore] // This tells SQLite to ignore this property during table creation
		public OrganizersModel AssignedOrganizer { get; set; }

		public string AssignedOrganizerJson
		{
			get => Newtonsoft.Json.JsonConvert.SerializeObject(AssignedOrganizer);
			set => AssignedOrganizer = Newtonsoft.Json.JsonConvert.DeserializeObject<OrganizersModel>(value);
		}

		[Ignore] // This tells SQLite to ignore this property during table creation
		public SpeakersModel AssignedSpeaker { get; set; }

		public string AssignedSpeakerJson
		{
			get => Newtonsoft.Json.JsonConvert.SerializeObject(AssignedSpeaker);
			set => AssignedSpeaker = Newtonsoft.Json.JsonConvert.DeserializeObject<SpeakersModel>(value);
		}

	}
}
