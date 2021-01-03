using System.ComponentModel.DataAnnotations;

namespace project.models.Roles
{
    public class BaseRoleModel
    {
        [Required(ErrorMessage = "de naam van de rol moet ingevuld worden")]
        [Display(Name = "Rolnaam")]
        public string Name { get; set; }

        [Required(ErrorMessage = "de omschrijving moet ingevuld worden")]
        [Display(Name = "Omschrijving")]
        public string Description { get; set; }
    }
}
