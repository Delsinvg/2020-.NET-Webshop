using Microsoft.AspNetCore.Http;
using project.models.Images;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace project.models.Products
{
    public class PostProductModel : BaseProductModel
    {
        [Required(ErrorMessage = "de categorie moet ingevuld worden")]
        [Display(Name = "Categorie")]
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "Het bedrijf moet ingevuld worden")]
        [Display(Name = "Bedrijf")]
        public Guid CompanyId { get; set; }

        [JsonIgnore]
        public ICollection<IFormFile> Images { get; set; }

        public ICollection<ImageModel> ImageModels { get; set; }
    }
}
