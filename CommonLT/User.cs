using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLT
{
    [Description("users")]
    public class User
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        public string IdentityToken { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool? IsOwner { get; set; }
        public string DepartmentName { get; set; }
        public string Biography { get; set; }
        public string Responsibilities { get; set; }
        public string Timezone { get; set; }
        public string Position { get; set; }
        public string Workplace { get; set; }
        public string PhoneNumber { get; set; }
        public bool Enabled { get; set; }
        public string ImportData { get; set; }
        public string DisplayEmail { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime? Birthdate { get; set; }
        public DateTime? Birthyear { get; set; }
        public DateTime? LastLoginDate { get; set; }

        public string IdOrganization { get; set; }

        [JsonIgnore]
        public virtual Organization Organization { get; set; }

        [JsonIgnore]
        public static string IdxOrg = "idx_id_org";

        public static string GetTableName()
        {
            return Helpers.GetDescription(typeof(User));
        }

        public override bool Equals(object obj)
        {
            var objTo = obj as User;
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
