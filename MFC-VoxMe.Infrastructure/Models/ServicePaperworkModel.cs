using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFC_VoxMe.Infrastructure.Models
{
    public class ServicePaperworkModel
    {
        public string PaperworkName { get; set; }
        public string PaperworkGuid { get; set; }
    }

    // List all paperwork names
    public class ServicePaperwork
    {
        public const string MoveOrderContract = "Move Order Contract";
        public const string AdditionalServicesProvided = "Additional Services Provided";

        public const string JobDetails = "Job Details";
        public const string BingoSheet = "Bingo Sheet";
        public const string InterstateBillOfLading = "Interstate Bill Of Lading";

        public const string IntrastateBillOfLading = "Intrastate Bill Of Lading";
        public const string RegularInventories = "Regular Inventories";
        public const string ScaleTicket = "Scale Ticket";
        public const string GypsyMoth = "Gypsy Moth";

        public const string RatedInterstateBillOfLading = "Rated Interstate Bill Of Lading";
        public const string RatedSignedInterstateBillOfLading = "Rated Signed Interstate Bill Of Lading";
        public const string RatedIntrastateBillOfLading = "Rated Intrastate Bill Of Lading";
        public const string RatedSignedIntrastateBillOfLading = "Rated Signed Intrastate Bill Of Lading";



    }
}
