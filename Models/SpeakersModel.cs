using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementApp.Models
{
	public class SpeakersModel
	{
		[PrimaryKey, AutoIncrement]
		public int SpeakerID { get; set; }
		public string SpeakerName { get; set; }
		public string SpeakerImage { get; set; }
		public string SpeakerDetails { get; set; }
		// Cloned to get index, removes item at index then adds back, during update functionality
		public SpeakersModel Clone() => MemberwiseClone() as SpeakersModel;
	}
}
