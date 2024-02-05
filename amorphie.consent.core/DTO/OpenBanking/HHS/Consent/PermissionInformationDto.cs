using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amorphie.consent.core.DTO.OpenBanking.HHS
{
    /// <summary>
    /// Permission detail of consent
    /// </summary>
    public class PermissionInformationDto
    {
        /// <summary>
        /// Permissions in consent is set as code and description.
        /// Turkish description is set for now.
        /// </summary>
        public Dictionary<string,string> PermissionType { get; set; }
        public DateTime LastValidAccessDate { get; set; }
        public DateTime? TransactionInquiryStartTime { get; set; }
        public DateTime? TransactionInquiryEndTime { get; set; }
    }
}
