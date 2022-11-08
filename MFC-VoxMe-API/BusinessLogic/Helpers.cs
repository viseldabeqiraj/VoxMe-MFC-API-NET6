using MFC_VoxMe.Core.Dtos.Common;
using MFC_VoxMe_API.Dtos.Common;
using MFC_VoxMe_API.Dtos.Jobs;
using MFC_VoxMe_API.Dtos.Management;
using MFC_VoxMe_API.Dtos.Transactions;
using MFC_VoxMe_API.Models;
using MFC_VoxMe_API.Profiles;
using Newtonsoft.Json;
using Serilog;
using System.Net;
using System.Text;
using System.Xml.Serialization;
using static MFC_VoxMe_API.Dtos.Jobs.CreateJobDto;
using static MFC_VoxMe_API.Dtos.Transactions.AssignStaffDesignateForemanDto;

namespace MFC_VoxMe_API.BusinessLogic
{
    public class Helpers : IHelpers
    {
		public static MovingDataDto _MovingData;

		public MovingDataDto XMLParse(string xml)
		{
			try
			{
				string xmldummy = @"<MovingData ID=""0227432"">
	<GeneralInfo>
		<ClientNumber>RC0184239</ClientNumber>
		<ClientSalutation>Mr.</ClientSalutation>
		<ClientFirstName>Lee</ClientFirstName>
		<Name>Aaron</Name>
		<EstimatorName>Stoltz, Dianna</EstimatorName>
		<EMFID>RS0227432</EMFID>
		<Groupageid>RM0157242</Groupageid>
		<Coordinatoremail>Dianna.Stoltz@jkmoving.com</Coordinatoremail>
		<CoordinatorID>DC6CA71E-6511-EC11-8150-00155D50DB1C</CoordinatorID>
		<ShipmentType>Pack</ShipmentType>
		<State>3</State>
		<Address>
			<Street>357 N St SW</Street>
			<City>Washington</City>
			<State>DC</State>
			<Country>USA</Country>
			<Zip>20024</Zip>
			<PrimaryPhone>703-624-7960</PrimaryPhone>
			<Email>leekaaron@gmail.com</Email>
			<Comment>Customer will pack the rest    Leekaaron@gmail.com 3 TV Boxes </Comment>
			<AccessInfo>
				<PropertyType>House</PropertyType>
				<PropertySize/>
				<Floor>0</Floor>
				<HasElevator>false</HasElevator>
				<CarryRequired>false</CarryRequired>
				<ShuttleRequired>false</ShuttleRequired>
				<StairCarryRequired>false</StairCarryRequired>
				<AdditionalStopRequired>false</AdditionalStopRequired>
				<AdditionalStop/>
			</AccessInfo>
			<Rooms/>
		</Address>
		<Destination>
			<Street>5 Shiplock Row</Street>
			<City>Henrico</City>
			<State>VA</State>
			<Country>USA</Country>
			<Zip>23231</Zip>
			<SecondaryPhone>703-624-7960</SecondaryPhone>
			<Email>leekaaron@gmail.com</Email>
			<Comment>Customer will pack the rest    Leekaaron@gmail.com 3 TV Boxes </Comment>
			<AccessInfo>
				<PropertyType>Townhouse</PropertyType>
				<PropertySize/>
				<Floor>0</Floor>
				<HasElevator>false</HasElevator>
				<CarryRequired>false</CarryRequired>
				<ShuttleRequired>false</ShuttleRequired>
				<StairCarryRequired>false</StairCarryRequired>
				<AdditionalStopRequired>false</AdditionalStopRequired>
				<AdditionalStop/>
			</AccessInfo>
			<DestRooms>
				<DestRoom density=""7"" volumeAllowance=""0"" weightAllowance=""0"">TRUCK</DestRoom>
			</DestRooms>
		</Destination>
		<Preferences>
			<PreferredLanguage>English</PreferredLanguage>
			<VacationDate>10/04/22 09:00</VacationDate>
			<PackingDate>10/04/22 16:00</PackingDate>
			<ServiceLevel>HHG</ServiceLevel>
			<Comment>Customer will pack the rest    Leekaaron@gmail.com 3 TV Boxes </Comment>
			<PackingFinishDate>10/06/22 09:00</PackingFinishDate>
			<DeliveryDate>10/06/22 09:00</DeliveryDate>
		</Preferences>
		<Comment>JK to pack China crystal and pictures, dates, brand new home at destination</Comment>
	</GeneralInfo>
	<InventoryData Uom=""Feet"" Offset=""0"">
		<Skids>
			<Skid ID=""1"">
				<Type>DELIVERY</Type>
				<Volume>0.00</Volume>
				<Height>0.0000</Height>
				<Width>0.0000</Width>
				<Length>0.0000</Length>
				<Weight>0.00</Weight>
				<SealNo/>
				<SerialNo>UNPACKED</SerialNo>
				<Barcode>S-1000139001</Barcode>
				<Location/>
				<PictureFileName/>
				<ReadOnly>0</ReadOnly>
				<LocationScanTime/>
				<Handler/>
			</Skid>
		</Skids>
		<Packers>
			<Packer name=""ALVINO-JENNIFER"" id=""2635FC19-3F0D-EB11-8133-369E029457BB"" isForeman=""false""/>
			<Packer name=""CARROLL-THOMAS"" id=""5FAACFD2-BFA6-E611-80D5-CE1A72BF755F"" isForeman=""true""/>
		</Packers>
		<Materials>
			<Material>
				<Category>Material</Category>
				<Type>1.5  Carton</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>3.1  Carton</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>4.5  Carton</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Bike Box</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Bubble Wrap</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Carpet Shield</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Clock  Carton</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Crate Count</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Crib Mattress Carton</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>D Air Freight Container</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Dish  Carton</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Double Mattress Carton</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Flat Wardrobe</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Gator Box</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>King Mattress Carton</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Kraft Bubble</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Lamp Carton</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Ldn Air Freight Container</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Lift Vans</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Memoryfoam</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Mirror 4 Piece</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Office Totes</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Paper</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Paper Pad</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Parts Box</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Peanuts</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Pillowtop Carton</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Queen Mattress Carton</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Single Mattress Carton</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Stretch Wrap</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Tape</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Triwall 10</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Triwall 15</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Triwall 5</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>TV Box</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Used Liftvans</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Wardrobe Rent</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Wardrobe Sold</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Washer Kit Fl</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Washer Kit Tl</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
			<Material>
				<Category>Material</Category>
				<Type>Wine Box</Type>
				<QtyTaken>0.00</QtyTaken>
				<Description/>
				<Value/>
				<QtyReturned>0</QtyReturned>
			</Material>
		</Materials>
		<Services>
			<Service>
				<Category/>
				<Type>Form.LocalStorage.NumberofPallets</Type>
				<Description/>
				<QtyTaken>0</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Service>
			<Service>
				<Category/>
				<Type>Form.LocalStorage.RatePerPallet</Type>
				<Description/>
				<QtyTaken>0.0000</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Service>
			<Service>
				<Category/>
				<Type>Form.LocalStorage.NumberofOverflows</Type>
				<Description/>
				<QtyTaken>0</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Service>
			<Service>
				<Category/>
				<Type>Form.LocalStorage.RatePerOverflow</Type>
				<Description/>
				<QtyTaken>0.0000</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Service>
			<Service>
				<Category/>
				<Type>Form.LocalStorage.NumberOfValets</Type>
				<Description/>
				<QtyTaken>0</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Service>
			<Service>
				<Category/>
				<Type>Form.LocalStorage.RatePerValets</Type>
				<Description/>
				<QtyTaken>0.0000</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Service>
			<Service>
				<Category/>
				<Type>Form.LocalStorage.MonthlyPerPalletRate</Type>
				<Description/>
				<QtyTaken>0.0000</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Service>
			<Service>
				<Category/>
				<Type>Form.LocalStorage.MonthlyPerOverflowRate</Type>
				<Description/>
				<QtyTaken>0.0000</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Service>
			<Service>
				<Category/>
				<Type>Form.LocalStorage.MonthlyValetRate</Type>
				<Description/>
				<QtyTaken>0.0000</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Service>
			<Service>
				<Category/>
				<Type>Form.LocalStorage.RemainingBalance</Type>
				<Description/>
				<QtyTaken>0.0000</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Service>
			<Service>
				<Category/>
				<Type>Forms.SCF.Packing</Type>
				<Description/>
				<QtyTaken>1.00</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Service>
			<Service>
				<Category/>
				<Type>Forms.SCF.PackingDescription</Type>
				<Description>I acknowledge all items have been packed to my instructions and satisfaction. I acknowledge that all rooms have been checked with crew foreman, including closets, storage spaces, attics, pantries etc. I acknowledge no damage has occured to my residence</Description>
				<QtyTaken>1.00</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Service>
			<Service>
				<Category/>
				<Type>Forms.SCF.Loading</Type>
				<Description/>
				<QtyTaken>1.00</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Service>
			<Service>
				<Category/>
				<Type>Forms.SCF.LoadingDescription</Type>
				<Description>I, Lee Aaron, acknowledge that I am responsible for the final walk-through at the origin. It is the customer's responsibility to make sure that nothing is left behind. Please check all closets, cabinets, drawers, attics, basements, and outside areas before signing this notice. JK Moving Services drivers, helpers and crew members cannot be held responsible after loading is complete. I acknowledge all items I want moved have been removed from origin to my satisfaction. During the final walk-through of the origin residence, there was no evidence of property damage including home, driveway, landscape, etc. except as noted in the Description section.</Description>
				<QtyTaken>1.00</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Service>
			<Service>
				<Category/>
				<Type>Forms.SCF.DeliveryDescription</Type>
				<Description>I, Lee Aaron, acknowledge that I am responsible for the final walk-through at the destination. I am satisfied that all of my items have been delivered and that none of my items are missing or remain on the truck. Additionally, any floor protection which includes manosite, carpet runners ans/or carpet shield has been removed. If floor protection is not removed immediately upon completion of the move, the shipper assumes all liability. During the final walk-through of the destination residence, there was no evidence of property damage including home, driveway, landscape, etc. except as noted below.</Description>
				<QtyTaken>1.00</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Service>
			<Service>
				<Category/>
				<Type>Forms.SCF.UnpackDescription</Type>
				<Description>If unpacking is to be performed, please verify all requested unpacking has been completed to your satisfaction.</Description>
				<QtyTaken>1.00</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Service>
			<Service>
				<Category/>
				<Type>Forms.NOR.ServiceNatureOfRisk</Type>
				<Description>The driver has advised me that the handling of the items noted above may result in damage to the item itself, or the residence, e.g., carpeting, flooring, or walls. Therefore, for good and adequate consideration received, I, Lee Aaron, hereby release JK Moving Services and its employees from any and all claims for damage to the items noted or to our residence. In addition, this may include soiling of the interior carpeting or flooring, damage related to or caused by the mud and soil from the surrounding exterior, and any damage caused by requested protective materials.</Description>
				<QtyTaken>1.00</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Service>
			<Service>
				<Category/>
				<Type>Form.Bingo.Signature</Type>
				<Description/>
				<QtyTaken>1.00</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Service>
			<Service>
				<Category/>
				<Type>Form.Bingo.CustomerBingo</Type>
				<Description/>
				<QtyTaken>1.00</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value>Bingo not required.</Value>
			</Service>
			<Service>
				<Category/>
				<Type>Forms.HVI.CustomerAgreement</Type>
				<Description>I acknowledge that I have prepared and retained a copy of the ""Inventory of Items Valued in Excess of $100 Per Pound per Article"" that are included in my shipment and that I have given a copy of this Inventory to the mover's representative. I also acknowledge that the mover's liability for loss of or damage to any article valued in excess of $100 per pound will be limited to $100 per pound for each pound of such lost or damaged article(s) (based on actual article weight), not to exceed the declared value of the entire shipment, unless I have specifically identified such articles for which a claim for loss or damage may be made in the attached inventory.</Description>
				<QtyTaken>1.00</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Service>
			<Service>
				<Category/>
				<Type>Forms.HVI.HighValueItemsIncluded</Type>
				<Description/>
				<QtyTaken>0.00</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Service>
			<Service>
				<Category/>
				<Type>Form.WeightTicket.WeightTicketRequired</Type>
				<Description/>
				<QtyTaken>0.00</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Service>
			<Service>
				<Category/>
				<Type>Form.ASP.ServicesPerformed</Type>
				<Description/>
				<QtyTaken>1.00</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Service>
		</Services>
		<Properties>
			<Property>
				<Category/>
				<Type>Form.General.Account</Type>
				<QtyTaken>1.00</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Property>
			<Property>
				<Category/>
				<Type>Form.General.Organization</Type>
				<QtyTaken>1.00</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Property>
			<Property>
				<Category/>
				<Type>Form.General.Contract</Type>
				<Description>Common Carriage</Description>
				<QtyTaken>1.00</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Property>
			<Property>
				<Category/>
				<Type>Form.General.Authority</Type>
				<Description>Interstate</Description>
				<QtyTaken>1.00</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Property>
			<Property>
				<Category/>
				<Type>Form.General.StartTime</Type>
				<Description>8-10 AM</Description>
				<QtyTaken>1.00</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Property>
			<Property>
				<Category/>
				<Type>Form.General.Weight</Type>
				<Description/>
				<QtyTaken>12462</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Property>
			<Property>
				<Category/>
				<Type>Form.General.HourlyRate</Type>
				<Description/>
				<QtyTaken>0.0000</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Property>
			<Property>
				<Category/>
				<Type>Form.General.HoursOfLabor</Type>
				<Description/>
				<QtyTaken>0.0000000000</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Property>
			<Property>
				<Category/>
				<Type>Form.General.MinimumLaborHours</Type>
				<Description/>
				<QtyTaken>0.0000000000</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Property>
			<Property>
				<Category/>
				<Type>Form.General.TravelRate</Type>
				<Description/>
				<QtyTaken>0.0000</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Property>
			<Property>
				<Category/>
				<Type>Form.General.TravelTime</Type>
				<Description/>
				<QtyTaken>0.0000000000</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Property>
			<Property>
				<Category/>
				<Type>Form.General.MinimumTravelTime</Type>
				<Description/>
				<QtyTaken>0</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Property>
			<Property>
				<Category/>
				<Type>Form.General.AdditionalServices</Type>
				<Description>&#x0D; Adjustment (Charge)</Description>
				<QtyTaken>1.00</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Property>
			<Property>
				<Category/>
				<Type>Form.General.AdditionalServicesTotalCharge</Type>
				<Description/>
				<QtyTaken>276.0000</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Property>
			<Property>
				<Category/>
				<Type>Form.General.NumberofCrates</Type>
				<Description/>
				<QtyTaken>0</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Property>
			<Property>
				<Category/>
				<Type>Form.General.CratesTotalCost</Type>
				<Description/>
				<QtyTaken>0.0000</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Property>
			<Property>
				<Category/>
				<Type>Form.General.CratesBuiltBy</Type>
				<QtyTaken>1.00</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Property>
			<Property>
				<Category/>
				<Type>Form.General.Equipment</Type>
				<Description/>
				<QtyTaken>1.00</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Property>
			<Property>
				<Category/>
				<Type>Form.General.Materials</Type>
				<Description>1.5 Carton:[1], Dish Carton:[7], King Mattress Carton:[1], Mirror (4pc):[24], Queen Mattress Carton:[1], TV Crate:[3]</Description>
				<QtyTaken>1.00</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Property>
			<Property>
				<Category/>
				<Type>Form.General.Trucks</Type>
				<Description>PV: 1, ;  2106 - Cube Van</Description>
				<QtyTaken>1.00</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Property>
			<Property>
				<Category/>
				<Type>Form.General.PackType</Type>
				<Description/>
				<QtyTaken>1.00</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value>Partial</Value>
			</Property>
			<Property>
				<Category/>
				<Type>Form.General.UnpackType</Type>
				<Description/>
				<QtyTaken>1.00</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value>Partial</Value>
			</Property>
			<Property>
				<Category/>
				<Type>Form.General.Paytype</Type>
				<Description>COD</Description>
				<QtyTaken>1.00</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Property>
			<Property>
				<Category/>
				<Type>Form.General.PayStatus</Type>
				<Description/>
				<QtyTaken>1.00</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value>Paid in Full</Value>
			</Property>
			<Property>
				<Category/>
				<Type>Form.ValuationConfirmation.ValuationType</Type>
				<Description/>
				<QtyTaken>1.00</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value>B - Full Replacement</Value>
			</Property>
			<Property>
				<Category/>
				<Type>Form.ValuationConfirmation.ValuationDeductible</Type>
				<Description/>
				<QtyTaken>1.00</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value>$500</Value>
			</Property>
			<Property>
				<Category/>
				<Type>Form.ValuationConfirmation.DeclaredValue</Type>
				<Description/>
				<QtyTaken>74772.0000</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Property>
			<Property>
				<Category/>
				<Type>Form.ValuationConfirmation.Cost</Type>
				<Description/>
				<QtyTaken>373.8600</QtyTaken>
				<QtyReturned>0.00</QtyReturned>
				<Value/>
			</Property>
		</Properties>
	</InventoryData>
	<Documents>
		<Document>
			<FileName>ASP.pdf</FileName>
		</Document>
		<Document>
			<FileName>InterstateBOL.pdf</FileName>
		</Document>
		<Document>
			<FileName>Estimate.pdf</FileName>
		</Document>
		<Document>
			<FileName>TableOfMeasurements.pdf</FileName>
		</Document>
	</Documents>
</MovingData>";

				XmlSerializer serializer = new XmlSerializer(typeof(MovingDataDto));
				MemoryStream memStream = new MemoryStream(Encoding.UTF8.GetBytes(xml));
				MovingDataDto movingDataFromXml = (MovingDataDto)serializer.Deserialize(memStream);
				_MovingData = movingDataFromXml;
				setProperties();

				return movingDataFromXml;
			}
			catch (Exception ex)
			{
				Log.Error($"Method XMLParse in {this.GetType().Name} failed. Exception thrown :{ex.Message}");
				return null;
			}
		}

		public void setProperties()
		{
			var moving = new MovingData();
			//PropertyMatcher<CreateJobDto, MovingData>.GenerateMatchedObject(CreateJobObjectFromXml(), moving);
			//PropertyMatcher<CreateJobDto.Client, MovingData>.GenerateMatchedObject(CreateJobObjectFromXml().client, moving);
			var json = JsonConvert.SerializeObject(CreateJobObjectFromXml());//JsonSerializer.Serialize(CreateJobObjectFromXml());
			var to = JsonConvert.DeserializeObject<MovingData>(json);//JsonSerializer.Deserialize<MovingData>(json);
		}


		public CreateJobDto CreateJobObjectFromXml()
        {
			try
            {
				CreateJobDto createJobDto = new CreateJobDto();
	
				var generalInfo = _MovingData.GeneralInfo;
				createJobDto.externalRef = generalInfo.Groupageid;
				var properties = _MovingData.InventoryData.Properties.Property;

				createJobDto.serviceType = "Enum.ServiceType." + properties.FirstOrDefault
					(s => s.Type == "Form.General.Contract").Description.Replace(" ", "");
				createJobDto.jobType = "Enum.JobType." + properties.FirstOrDefault
					(s => s.Type == "Form.General.Authority").Description.Replace(" ", "");

				createJobDto.serviceLevel = generalInfo.Preferences.ServiceLevel;
				createJobDto.client.legalName = generalInfo.ClientFirstName + " " + generalInfo.Name;
				createJobDto.client.code = generalInfo.ClientNumber;
				createJobDto.instructionsCrewOrigin = generalInfo.Preferences.Comment + "\n" + generalInfo.Comment;
				createJobDto.instructionsCrewDestination = generalInfo.Preferences.Comment + "\n" + generalInfo.Comment;


				createJobDto.clientPerson = new ClientPerson()
				{
					code = generalInfo.ClientNumber,
					partyCode = generalInfo.ClientNumber,
					personDetails = new PersonDetails()
                    {
						firstName = generalInfo.ClientFirstName,
						lastName= generalInfo.Name,
						salutation = "Enum.Salutation." + generalInfo.ClientSalutation,
						contactDetails = new ContactDetails()
                        {
							mobilePhone = generalInfo.Address.PrimaryPhone,
							email = generalInfo.Address.Email
                        }
					},
					
				};

				createJobDto.managedBy = new ManagedBy()
				{
					code = generalInfo.CoordinatorID,
					personDetails = new PersonDetails()
                    {
						firstName = generalInfo.EstimatorName.Substring(generalInfo.EstimatorName.LastIndexOf(',') + 1),
						lastName = generalInfo.EstimatorName.Split(',')[0],
						contactDetails = new ContactDetails()
                        {
							email = generalInfo.Coordinatoremail,
							mobilePhone =  generalInfo.CoordinatorMobile
                        },						
						
					}
					
				};
				createJobDto.bookerPerson.personDetails = createJobDto.managedBy.personDetails;
				createJobDto.bookerPerson.code = generalInfo.CoordinatorID;

				if (properties.Any(s => s.Type == "Form.General.Account" && s.Description != null))
				{
					createJobDto.account = new Account();
					createJobDto.accountPerson = new AccountPerson();
					createJobDto.account.code = "Enum.PartyType." + properties.FirstOrDefault
						(s => s.Type == "Form.General.Account").Description.Replace(" ", "");
					createJobDto.account.legalName = createJobDto.account.code;
					createJobDto.accountPerson.personDetails = createJobDto.managedBy.personDetails;
				}

				createJobDto.originAddress = new CreateJobDto.OriginAddress()
				{
					partyCode = generalInfo.ClientNumber,
					addressDetails = new AddressDetails()
					{
						street1 = generalInfo.Address.Street,
						city = generalInfo.Address.City,
						area = generalInfo.Address.State,
						country = generalInfo.Address.Country,
						floor = generalInfo.Address.AccessInfo.Floor,
						notes = generalInfo.Address.Comment,
						zip = generalInfo.Address.Zip,
					}
				};

				createJobDto.destinationAddress = new DestinationAddress()
				{
					partyCode = generalInfo.EMFID,
					addressDetails = new AddressDetails()
					{
						street1 = generalInfo.Destination.Street,
						city = generalInfo.Destination.City,
						area = generalInfo.Destination.State,
						country = generalInfo.Destination.Country,
						floor = generalInfo.Destination.AccessInfo.Floor,
						notes = generalInfo.Comment,
						zip = generalInfo.Destination.Zip,
					}
				};

				createJobDto.originPartyContact = new OriginPartyContact()
				{
					code = generalInfo.ClientNumber,
					partyCode = generalInfo.ClientNumber, 
					personDetails = createJobDto.clientPerson.personDetails
				};

				createJobDto.destinationPartyContact = new DestinationPartyContact()
				{

					code = generalInfo.ClientNumber,
					partyCode = generalInfo.ClientNumber, 
					personDetails = createJobDto.clientPerson.personDetails
				};

				return createJobDto;
			}
			catch (Exception ex)
            {
				Log.Error($"Method CreateJobObjectFromXml in {this.GetType().Name} failed. Exception thrown :{ex.Message}");
				return null;
            }
        }

		public string GetTransactionEnum(string str)
        {
			var transType ="Enum.TransactionType.";
			switch (str)
			{
				case "APU Floor":
				case "APU Trailer":
				case "Crate & Freight":
				case "Export":
				case "Final Pickup":
				case "Load":
				case "Load & Deliver":
				case "Pack":
				case "Pickup":
				case "SIT at Origin":
				case "Storage In":
					return transType + "Pickup";
				case "Claims":
				case "Debris Pickup":
				case "Deliver":
				case "Import":
				case "SIT at Destination":
				case "Storage Out":
				case "Unpack":
					return transType + "Delivery";
				case "Receive":
				case "Release Floor":
				case "SIT Inbound":
						return transType + "WarehouseReceiveIn";
				case "Release":
				case "Release Overflow":
				case "Release Trailer":
					return transType + "WarehouseOutload";
				case "Storage Access":
					return transType + "OnSite";
				default:
					return transType + str.Replace(" ", "");
			}
		}

		//public List<string> TransactionDictionary(string code, string str)
  //      {
		//	var transType = "Enum.TransactionType.";
		//	var transService = "Enum.TransactionService.";

		//	var mappingA = new Dictionary<string, List<string>>()
		//	{
		//		{ "APU Floor", new List<string>() 
		//		{ transType + "Pickup", transService + str.Replace(" ", "")} },
		//		{ "APU Trailer",new List<string>()
		//		{ transType + "Pickup", transService + str.Replace(" ", "")} },
		//		{ "Crate & Freight",new List<string>()
		//		{ transType + "Pickup", transService + str.Replace(" ", "")} },
		//		{ "Export", new List<string>() { "ff" } },
		//		{ "Final Pickup", new List<string>() { "ff" }},
		//		{ "Load", new List<string>() { "ff" } },
		//		{ "Load & Deliver", new List<string>() { "ff" } },
		//		{ "Pack", new List<string>() { "ff" } },
		//		{ "Pickup", new List<string>() { "ff" } },
		//		{ "SIT at Origin",new List<string>() { "ff" } },
		//		{ "Storage In",new List<string>() { "ff" } }
		//	};
		//		return mappingA[str];
			
		//}


		public CreateTransactionDto CreateTransactionObjectFromXml()
		{
			try
			{
				CreateTransactionDto createTransaction = new CreateTransactionDto();
				var generalInfo = _MovingData.GeneralInfo;

				createTransaction.externalRef = generalInfo.EMFID;
				createTransaction.transactionType = GetTransactionEnum(generalInfo.ShipmentType);
				createTransaction.services = new List<string>(1)
				{ $"Enum.TransactionService.{generalInfo.ShipmentType.Replace(" ", "")}" };
				createTransaction.originParty = generalInfo.ClientNumber;
				createTransaction.destinationParty = generalInfo.ClientNumber;
				createTransaction.jobExternalRef = generalInfo.Groupageid;
				createTransaction.instructionsCrewOrigin = generalInfo.Preferences.Comment + "\n" + generalInfo.Comment;
				createTransaction.instructionsCrewDestination = generalInfo.Preferences.Comment + "\n" + generalInfo.Comment;
				createTransaction.scheduledDate = Convert.ToDateTime(generalInfo.Preferences.PackingDate);

				var properties = _MovingData.InventoryData.Properties.Property;

				if (properties.Any())
				{
					List<CreateTransactionDto.QuestionnaireQuestion> questionnaireQuestions = properties.Select
						(a => new CreateTransactionDto.QuestionnaireQuestion()
						{
							name = a.Type,
							numericValue = a.QtyTaken,
							stringValue = a.Description,
							listValues = a.Value,
							booleanValue = a.QtyTaken > 0 ? true : false

						}).ToList();
					createTransaction.questionnaireQuestions = questionnaireQuestions;
				}
				var services = _MovingData.InventoryData.Services.Service;
				if (services.Any())
				{
					List<CreateTransactionDto.AuxService> auxServices = services.Select
						(a => new CreateTransactionDto.AuxService()
						{
							name = a.Type,
							numericValue = a.QtyTaken,
							stringValue = a.Description,
							listValues = a.Value,
							booleanValue = a.QtyTaken > 0 ? true : false

						}).ToList();
					createTransaction.auxServices = auxServices;
				}
				List<CreateTransactionDto.LoadingUnit> loadingUnits = new List<CreateTransactionDto.LoadingUnit>(1);
				CreateTransactionDto.LoadingUnit loadingUnit = new CreateTransactionDto.LoadingUnit()
				{
					uniqueId = generalInfo.EMFID + ".Delivery",
					loadingUnitDetails = new CreateTransactionDto.LoadingUnitDetails()
					{
						labelNr = 1 //TODO
					}
				};
				loadingUnits.Add(loadingUnit);
				createTransaction.loadingUnits = loadingUnits;


				return createTransaction;
			}
			catch (Exception ex)
			{
				Log.Error($"Method CreateTransactionObjectFromXml in {this.GetType().Name} failed. Exception thrown :{ex.Message}");
				return null;
			}
		}


		public AssignMaterialsToTransactionDto GetTransactionMaterials()
        {
            try
            {				
				var materials = _MovingData.InventoryData.Materials.Material.ToList();
                AssignMaterialsToTransactionDto assignMaterialsToTransaction =
                    new AssignMaterialsToTransactionDto();

				if (materials != null)
				{
					List<AssignMaterialsToTransactionDto.HandedMaterial> materialList = materials.Select
						(a => new AssignMaterialsToTransactionDto.HandedMaterial()
						{
							code = "Enum.MaterialType." + (a.Type).Replace(" ", ""),
							qty = Convert.ToDouble(a.QtyTaken)
						}).ToList();
					assignMaterialsToTransaction.handedMaterials = materialList;
				}
				return assignMaterialsToTransaction;

			}
            catch (Exception ex)
            {
				Log.Error($"Method GetTransactionMaterials in {this.GetType().Name} failed. Exception thrown :{ex.Message}");
				return null;
            }
        }

		public AssignStaffDesignateForemanDto GetTransactionResources()
		{
			try
			{
				var packers = _MovingData.InventoryData.Packers.Packer.ToList();
				AssignStaffDesignateForemanDto resourceCodesDto =
					new AssignStaffDesignateForemanDto();

				if (packers.Any())
				{
					List<StaffResourceCode> packerList = packers.Select
						(a => new StaffResourceCode()
						{
							code = a.Name,
							isForeman = bool.Parse(a.IsForeman)
						}).ToList();
					resourceCodesDto.staffResourceCodes = packerList;					
				}
				return resourceCodesDto;
			}
			catch (Exception ex)
			{
				Log.Error($"Method GetTransactionResources in {this.GetType().Name} failed. Exception thrown :{ex.Message}");
				return null;
			}
		}
	}
}
