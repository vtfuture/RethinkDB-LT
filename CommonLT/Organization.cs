using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLT
{
    [Description("organizations")]
    public class Organization
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        public string Guid { get; set; }
        public bool Enabled { get; set; }
        public DateTime Timestamp { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string VatId { get; set; }
        public bool IsOwner { get; set; }
        public long? IdAddress { get; set; }
        public string AcceptedDomainList { get; set; }
        public long DocumentsTotalSize { get; set; }
        public long ImagesTotalSize { get; set; }
        public long VideosTotalSize { get; set; }
        public string ImportData { get; set; }
        public string AuthenticationData { get; set; }
        public bool IsSuspended { get; set; }
        public string SuspendedMessage { get; set; }

        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }

        [JsonIgnore]
        public static string IdxTimestamp = "idx_timestamp";

        public static string GetTableName()
        {
            return Helpers.GetDescription(typeof(Organization));
        }

        public override bool Equals(object obj)
        {
            var objTo = obj as Organization;
            if (objTo != null)
                return Id != null && objTo.Id != null && String.Equals(Id, objTo.Id);
            else
                return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            if (Id != null)
                return Id.GetHashCode();
            else
                return base.GetHashCode();
        }
    }
}
