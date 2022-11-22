using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFC_VoxMe.Infrastructure.Models
{
    public class Address
    {
		public int MovingDataID { get; set; }
		public bool IsDestination { get; set; }
		public string Street { get; set; }
		public string City { get; set; }
		public string Country { get; set; }
		public string Floor { get; set; }
		public bool Elevator { get; set; }
		public string ElevatorDetails { get; set; }
		public int DistanceToParking { get; set; }
		public bool NeedCrane { get; set; }
		public string PrimaryPhone { get; set; }
		public string SecondaryPhone { get; set; }
		public string Email { get; set; }
		public string Fax { get; set; }
		public string Comment { get; set; }
		public string Zip { get; set; }
		public string State { get; set; }
		public string PictureFileName { get; set; }
		public string Company { get; set; }
		public string PropertyType { get; set; }
		public string PropertySize { get; set; }
		public bool ParkingReservationRequired { get; set; }
		public string ParkingType { get; set; }
		public int NumOfParkingSpots { get; set; }
		public int ParkingSpotSize { get; set; }
		public bool CarryRequired { get; set; }
		public int CarryLength { get; set; }
		public string CarryDescription { get; set; }
		public string ExternalElevatorType { get; set; }
		public bool ShuttleRequired { get; set; }
		public int ShuttleDistance { get; set; }
		public bool StairCarryRequired { get; set; }
		public int StairCarryLength { get; set; }
		public string StairCarryDescription { get; set; }
		public bool AdditionalStopRequired { get; set; }
		public string AdditionalStop { get; set; }
	}
}
