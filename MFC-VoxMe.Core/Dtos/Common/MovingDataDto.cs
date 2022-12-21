using System.Collections.Generic;
using System.Xml.Serialization;

namespace MFC_VoxMe_API.Dtos.Common
{

	[XmlRoot(ElementName = "MovingData")]
	public class MovingDataDto
	{
		[XmlElement(ElementName = "GeneralInfo")]
		public GeneralInfo GeneralInfo { get; set; } = new GeneralInfo();
		[XmlElement(ElementName = "InventoryData")]
		public InventoryData InventoryData { get; set; } = new InventoryData();
		[XmlElement(ElementName = "Documents")]
		public Documents Documents { get; set; } = new Documents();
		[XmlAttribute(AttributeName = "ID")]
		public string ID { get; set; }
	}

	[XmlRoot(ElementName = "AccessInfo")]
		public class AccessInfo
		{
			[XmlElement(ElementName = "PropertyType")]
			public string PropertyType { get; set; }
			[XmlElement(ElementName = "PropertySize")]
			public string PropertySize { get; set; }
			[XmlElement(ElementName = "Floor")]
			public string Floor { get; set; }
			[XmlElement(ElementName = "HasElevator")]
			public string HasElevator { get; set; }
			[XmlElement(ElementName = "CarryRequired")]
			public string CarryRequired { get; set; }
			[XmlElement(ElementName = "ShuttleRequired")]
			public string ShuttleRequired { get; set; }
			[XmlElement(ElementName = "StairCarryRequired")]
			public string StairCarryRequired { get; set; }
			[XmlElement(ElementName = "AdditionalStopRequired")]
			public string AdditionalStopRequired { get; set; }
			[XmlElement(ElementName = "AdditionalStop")]
			public string AdditionalStop { get; set; }
		}

		[XmlRoot(ElementName = "Address")]
		public class Address
		{
			[XmlElement(ElementName = "Street")]
			public string Street { get; set; }
			[XmlElement(ElementName = "City")]
			public string City { get; set; }
			[XmlElement(ElementName = "State")]
			public string State { get; set; }
			[XmlElement(ElementName = "Country")]
			public string Country { get; set; }
			[XmlElement(ElementName = "Zip")]
			public string Zip { get; set; }
			[XmlElement(ElementName = "PrimaryPhone")]
			public string PrimaryPhone { get; set; }
			[XmlElement(ElementName = "SecondaryPhone")]
			public string SecondaryPhone { get; set; }
			[XmlElement(ElementName = "Fax")]
			public string Fax { get; set; }
			[XmlElement(ElementName = "Email")]
			public string Email { get; set; }
			[XmlElement(ElementName = "Comment")]
			public string Comment { get; set; }
			[XmlElement(ElementName = "AccessInfo")]
			public AccessInfo AccessInfo { get; set; } = new AccessInfo();
			[XmlElement(ElementName = "Rooms")]
			public Rooms Rooms { get; set; }
		}

		[XmlRoot(ElementName = "Rooms")]
		public class Rooms
		{

			[XmlElement(ElementName = "Room")]
			public List<Room> Room { get; set; }
		}


		[XmlRoot(ElementName = "Room")]
		public class Room
		{

			[XmlElement(ElementName = "Nickname")]
			public string Nickname { get; set; }

			[XmlElement(ElementName = "NumOfPeople")]
			public int NumOfPeople { get; set; }

			[XmlElement(ElementName = "EstimatedVolume")]
			public int EstimatedVolume { get; set; }

			[XmlAttribute(AttributeName = "name")]
			public string Name { get; set; }

			[XmlText]
			public string Text { get; set; }
		}

	[XmlRoot(ElementName = "Box")]
	public class Box
	{

		[XmlElement(ElementName = "Quantity")]
		public int Quantity { get; set; }

		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }

		//[XmlText]
		//public int Text { get; set; }
	}

	[XmlRoot(ElementName = "Size")]
	public class Size
	{

		[XmlElement(ElementName = "Width")]
		public double Width { get; set; }

		[XmlElement(ElementName = "Height")]
		public double Height { get; set; }

		[XmlElement(ElementName = "Length")]
		public double Length { get; set; }
	}

	[XmlRoot(ElementName = "Item")]
	public class Item
	{

		[XmlElement(ElementName = "Type")]
		public string Type { get; set; }

		[XmlElement(ElementName = "Quantity")]
		public int Quantity { get; set; }

		[XmlElement(ElementName = "Value")]
		public int Value { get; set; }

		[XmlElement(ElementName = "Condition")]
		public string Condition { get; set; }

		[XmlElement(ElementName = "Comment")]
		public string Comment { get; set; }

		[XmlElement(ElementName = "Size")]
		public Size Size { get; set; }

		[XmlElement(ElementName = "IsPart")]
		public bool IsPart { get; set; }

		[XmlElement(ElementName = "Dismantling")]
		public bool Dismantling { get; set; }

		[XmlElement(ElementName = "Assembling")]
		public bool Assembling { get; set; }

		[XmlElement(ElementName = "SpecialContainer")]
		public bool SpecialContainer { get; set; }

		[XmlElement(ElementName = "Make")]
		public string Make { get; set; }

		[XmlElement(ElementName = "Model")]
		public string Model { get; set; }

		[XmlElement(ElementName = "Year")]
		public string Year { get; set; }

		[XmlElement(ElementName = "SerialNumber")]
		public string SerialNumber { get; set; }

		[XmlElement(ElementName = "PictureName")]
		public string PictureName { get; set; }

		[XmlElement(ElementName = "PictureAuthor")]
		public string PictureAuthor { get; set; }

		[XmlElement(ElementName = "PictureAuthorFirstName")]
		public string PictureAuthorFirstName { get; set; }

		[XmlElement(ElementName = "PictureYear")]
		public string PictureYear { get; set; }

		[XmlElement(ElementName = "MaterialsDesc")]
		public string MaterialsDesc { get; set; }

		[XmlElement(ElementName = "CountryOrigin")]
		public string CountryOrigin { get; set; }

		[XmlElement(ElementName = "SurfaceSize")]
		public object SurfaceSize { get; set; }

		[XmlElement(ElementName = "CarpetYear")]
		public string CarpetYear { get; set; }

		[XmlElement(ElementName = "PictureFileName")]
		public string PictureFileName { get; set; }

		[XmlElement(ElementName = "IsValuable")]
		public bool IsValuable { get; set; }

		[XmlElement(ElementName = "Category")]
		public string Category { get; set; }

		[XmlAttribute(AttributeName = "id")]
		public int Id { get; set; }

		[XmlAttribute(AttributeName = "name")]
		public string Name { get; set; }

		[XmlText]
		public string Text { get; set; }
	}

	[XmlRoot(ElementName = "Piece")]
	public class Piece
	{

		[XmlElement(ElementName = "Barcode")]
		public string Barcode { get; set; }

		[XmlElement(ElementName = "Location")]
		public string Location { get; set; }

		[XmlElement(ElementName = "Volume")]
		public double Volume { get; set; }

		[XmlElement(ElementName = "Weight")]
		public double Weight { get; set; }

		[XmlElement(ElementName = "Copies")]
		public int Copies { get; set; }

		[XmlElement(ElementName = "PBO")]
		public bool PBO { get; set; }

		[XmlElement(ElementName = "NeedBox")]
		public bool NeedBox { get; set; }

		[XmlElement(ElementName = "Destination")]
		public string Destination { get; set; }

		[XmlElement(ElementName = "Box")]
		public Box Box { get; set; }

		[XmlElement(ElementName = "NumberOfElements")]
		public int NumberOfElements { get; set; }

		[XmlElement(ElementName = "LineItemNumber")]
		public int LineItemNumber { get; set; }

		[XmlElement(ElementName = "NumberOfPackages")]
		public int NumberOfPackages { get; set; }

		[XmlElement(ElementName = "SkidID")]
		public int SkidID { get; set; }

		[XmlElement(ElementName = "Packer")]
		public string Packer { get; set; }

		[XmlElement(ElementName = "IsSplit")]
		public int IsSplit { get; set; }

		[XmlElement(ElementName = "Parent")]
		public object Parent { get; set; }

		[XmlElement(ElementName = "Size")]
		public Size Size { get; set; }

		[XmlElement(ElementName = "Item")]
		public Item Item { get; set; }

		[XmlElement(ElementName = "AssociatedMaterials")]
		public object AssociatedMaterials { get; set; }

		[XmlElement(ElementName = "Void")]
		public bool Void { get; set; }

		[XmlAttribute(AttributeName = "id")]
		public int Id { get; set; }

		[XmlText]
		public string Text { get; set; }
	}


	[XmlRoot(ElementName = "DestRoom")]
		public class DestRoom
		{
			[XmlAttribute(AttributeName = "density")]
			public string Density { get; set; }
			[XmlAttribute(AttributeName = "volumeAllowance")]
			public string VolumeAllowance { get; set; }
			[XmlAttribute(AttributeName = "weightAllowance")]
			public string WeightAllowance { get; set; }
			[XmlText]
			public string Text { get; set; }
		}

		[XmlRoot(ElementName = "DestRooms")]
		public class DestRooms
		{
		[XmlElement(ElementName = "DestRoom")]
		public DestRoom DestRoom { get; set; } = new DestRoom();
		}

		[XmlRoot(ElementName = "Destination")]
		public class Destination
		{
			[XmlElement(ElementName = "Street")]
			public string Street { get; set; }
			[XmlElement(ElementName = "City")]
			public string City { get; set; }
			[XmlElement(ElementName = "State")]
			public string State { get; set; }
			[XmlElement(ElementName = "Country")]
			public string Country { get; set; }
			[XmlElement(ElementName = "Zip")]
			public string Zip { get; set; }
			[XmlElement(ElementName = "PrimaryPhone")]
			public string PrimaryPhone { get; set; }
			[XmlElement(ElementName = "SecondaryPhone")]
			public string SecondaryPhone { get; set; }
			[XmlElement(ElementName = "Fax")]
			public string Fax { get; set; }
			[XmlElement(ElementName = "Email")]
			public string Email { get; set; }
			[XmlElement(ElementName = "Comment")]
			public string Comment { get; set; }
			[XmlElement(ElementName = "AccessInfo")]
			public AccessInfo AccessInfo { get; set; } = new AccessInfo();
			[XmlElement(ElementName = "DestRooms")]
			public DestRooms DestRooms { get; set; } = new DestRooms();
		}

		[XmlRoot(ElementName = "Preferences")]
		public class Preferences
		{
			[XmlElement(ElementName = "PreferredLanguage")]
			public string PreferredLanguage { get; set; }
			[XmlElement(ElementName = "VacationDate")]
			public string VacationDate { get; set; }
			[XmlElement(ElementName = "PackingDate")]
			public string PackingDate { get; set; }
			[XmlElement(ElementName = "ServiceLevel")]
			public string ServiceLevel { get; set; }
			[XmlElement(ElementName = "Comment")]
			public string Comment { get; set; }
			[XmlElement(ElementName = "PackingFinishDate")]
			public string PackingFinishDate { get; set; }
			[XmlElement(ElementName = "DeliveryDate")]
			public string DeliveryDate { get; set; }
		}

		[XmlRoot(ElementName = "GeneralInfo")]
		public class GeneralInfo
		{
			[XmlElement(ElementName = "CustomerMobilePhone")]
			public string CustomerMobilePhone { get; set; }
			[XmlElement(ElementName = "CustomerHomePhone")]
			public string CustomerHomePhone { get; set; }
			[XmlElement(ElementName = "CustomerBusinessPhone")]
			public string CustomerBusinessPhone { get; set; }
            [XmlElement(ElementName = "ClientNumber")]
			public string ClientNumber { get; set; }
			[XmlElement(ElementName = "ClientSalutation")]
			public string ClientSalutation { get; set; }
			[XmlElement(ElementName = "ClientFirstName")]
			public string ClientFirstName { get; set; }
			[XmlElement(ElementName = "Name")]
			public string Name { get; set; }
			[XmlElement(ElementName = "EstimatorName")]
			public string EstimatorName { get; set; }
			[XmlElement(ElementName = "EMFID")]
			public string EMFID { get; set; }
			[XmlElement(ElementName = "Groupageid")]
			public string Groupageid { get; set; }
			[XmlElement(ElementName = "Coordinatoremail")]
			public string Coordinatoremail { get; set; }
			[XmlElement(ElementName = "CoordinatorMobile")]
			public string CoordinatorMobile { get; set; }
			[XmlElement(ElementName = "CoordinatorID")]
			public string CoordinatorID { get; set; }
			[XmlElement(ElementName = "ShipmentType")]
			public string ShipmentType { get; set; }
			[XmlElement(ElementName = "State")]
			public string State { get; set; }
			[XmlElement(ElementName = "Address")]
			public Address Address { get; set; } = new Address();
			[XmlElement(ElementName = "Destination")]
			public Destination Destination { get; set; } = new Destination();
			[XmlElement(ElementName = "Preferences")]
			public Preferences Preferences { get; set; } = new Preferences();
			[XmlElement(ElementName = "Comment")]
			public string Comment { get; set; }
		}

		[XmlRoot(ElementName = "Skid")]
		public class Skid
		{
			[XmlElement(ElementName = "Type")]
			public string Type { get; set; }
			[XmlElement(ElementName = "Volume")]
			public string Volume { get; set; }
			[XmlElement(ElementName = "Height")]
			public string Height { get; set; }
			[XmlElement(ElementName = "Width")]
			public string Width { get; set; }
			[XmlElement(ElementName = "Length")]
			public string Length { get; set; }
			[XmlElement(ElementName = "Weight")]
			public string Weight { get; set; }
			[XmlElement(ElementName = "SealNo")]
			public string SealNo { get; set; }
			[XmlElement(ElementName = "SerialNo")]
			public string SerialNo { get; set; }
			[XmlElement(ElementName = "Barcode")]
			public string Barcode { get; set; }
			[XmlElement(ElementName = "Location")]
			public string Location { get; set; }
			[XmlElement(ElementName = "PictureFileName")]
			public string PictureFileName { get; set; }
			[XmlElement(ElementName = "ReadOnly")]
			public string ReadOnly { get; set; }
			[XmlElement(ElementName = "LocationScanTime")]
			public string LocationScanTime { get; set; }
			[XmlElement(ElementName = "Handler")]
			public string Handler { get; set; }
			[XmlAttribute(AttributeName = "ID")]
			public string ID { get; set; }
		}

		[XmlRoot(ElementName = "Skids")]
		public class Skids
		{
			[XmlElement(ElementName = "Skid")]
			public List<Skid> Skid { get; set; } //= new Skid();
		}

		[XmlRoot(ElementName = "Packer")]
		public class Packer
		{
			[XmlAttribute(AttributeName = "name")]
			public string Name { get; set; }
			[XmlAttribute(AttributeName = "id")]
			public string Id { get; set; }
			[XmlAttribute(AttributeName = "isForeman")]
			public string IsForeman { get; set; }
		}

		[XmlRoot(ElementName = "Packers")]
		public class Packers
		{
			[XmlElement(ElementName = "Packer")]
			public List<Packer> Packer { get; set; }
		}

		[XmlRoot(ElementName = "Material")]
		public class Material
		{
			[XmlElement(ElementName = "Category")]
			public string Category { get; set; }
			[XmlElement(ElementName = "Type")]
			public string Type { get; set; }
			[XmlElement(ElementName = "QtyTaken")]
			public double QtyTaken { get; set; }
			[XmlElement(ElementName = "Description")]
			public string Description { get; set; }
			[XmlElement(ElementName = "Value")]
			public string Value { get; set; }
			[XmlElement(ElementName = "QtyReturned")]
			public double QtyReturned { get; set; }
		}

		[XmlRoot(ElementName = "Materials")]
		public class Materials
		{
			[XmlElement(ElementName = "Material")]
			public List<Material> Material { get; set; }
		}

		[XmlRoot(ElementName = "Service")]
		public class Service
		{
			[XmlElement(ElementName = "Category")]
			public string Category { get; set; }
			[XmlElement(ElementName = "Type")]
			public string Type { get; set; }
			[XmlElement(ElementName = "Description")]
			public string Description { get; set; }
			[XmlElement(ElementName = "QtyTaken")]
			public double QtyTaken { get; set; }
			[XmlElement(ElementName = "QtyReturned")]
			public double QtyReturned { get; set; }
			[XmlElement(ElementName = "Value")]
			public string Value { get; set; }
		}

		[XmlRoot(ElementName = "Services")]
		public class Services
		{
			[XmlElement(ElementName = "Service")]
			public List<Service> Service { get; set; }
		}

		[XmlRoot(ElementName = "Property")]
		public class Property
		{
			[XmlElement(ElementName = "Category")]
			public string Category { get; set; }
			[XmlElement(ElementName = "Type")]
			public string Type { get; set; }
			[XmlElement(ElementName = "QtyTaken")]
			public double QtyTaken { get; set; }
			[XmlElement(ElementName = "QtyReturned")]
			public double QtyReturned { get; set; }
			[XmlElement(ElementName = "Value")]
			public string Value { get; set; }
			[XmlElement(ElementName = "Description")]
			public string Description { get; set; }
		}

		[XmlRoot(ElementName = "Properties")]
		public class Properties
		{
			[XmlElement(ElementName = "Property")]
			public List<Property> Property { get; set; }
		}

		[XmlRoot(ElementName = "InventoryData")]
		public class InventoryData
		{
			[XmlElement(ElementName = "Skids")]
			public Skids Skids { get; set; } = new Skids();
			[XmlElement(ElementName = "Packers")]
			public Packers Packers { get; set; } = new Packers();
			[XmlElement(ElementName = "Materials")]
			public Materials Materials { get; set; } = new Materials();
			[XmlElement(ElementName = "Services")]
			public Services Services { get; set; } = new Services();
			[XmlElement(ElementName = "Properties")]
			public Properties Properties { get; set; } = new Properties();
			//[XmlAttribute(AttributeName = "Uom")]
			//public string Uom { get; set; }
			[XmlAttribute(AttributeName = "Offset")]
				public string Offset { get; set; }

			[XmlElement(ElementName = "Piece")]
			public List<Piece> Piece { get; set; }
	}

		[XmlRoot(ElementName = "Document")]
		public class Document
		{
			[XmlElement(ElementName = "FileName")]
			public string FileName { get; set; }
		}

		[XmlRoot(ElementName = "Documents")]
		public class Documents
		{
			[XmlElement(ElementName = "Document")]
			public List<Document> Document { get; set; }
		}

		

	}





