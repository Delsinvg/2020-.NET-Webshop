using Microsoft.AspNetCore.Http;
using project.models.Images;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace project.models.Products
{
    public class PutProductModel : BaseProductModel
    {
        public Guid CategoryId { get; set; }
        public Guid CompanyId { get; set; }

        [JsonIgnore]
        public ICollection<IFormFile> Images { get; set; }

        public ICollection<ImageModel> ImageModels { get; set; }

        public string AfbeeldingNamen { get; set; }
    }
}
