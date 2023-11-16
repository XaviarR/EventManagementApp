using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventManagementApp.Models
{
	public class SponsorsModel
	{
		[PrimaryKey, AutoIncrement]
		public int SponsorID { get; set; }
		public string SponsorName { get; set; }
		public string SponsorImage { get; set; }
		public string SponsorDetails { get; set; }
		// Cloned to get index, removes item at index then adds back, during update functionality
		public SponsorsModel Clone() => MemberwiseClone() as SponsorsModel;
	}
}
