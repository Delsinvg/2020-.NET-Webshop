using System;
using System.Text.Json.Serialization;

namespace project.models.Roles
{
    public class GetRoleModel : BaseRoleModel
    {
        public Guid Id { get; set; }
        [JsonIgnore]
        public bool Checked { get; set; }
    }
}
