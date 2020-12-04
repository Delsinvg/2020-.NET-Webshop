using System.ComponentModel.DataAnnotations;

namespace project.models.Roles
{
    public class BaseRoleModel
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
