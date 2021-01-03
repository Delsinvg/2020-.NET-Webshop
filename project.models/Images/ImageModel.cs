using System.ComponentModel.DataAnnotations;

namespace project.models.Images
{
    public class ImageModel
    {
        [Display(Name = "Naam")]
        public string Name { get; set; }
        [Display(Name = "Bestandstype")]
        public string FileType { get; set; }
        [Display(Name = "Extensie")]
        public string Extension { get; set; }
        [Display(Name = "Omschrijving")]
        public string Description { get; set; }
        [Display(Name = "data")]
        public byte[] Data { get; set; }
    }
}
