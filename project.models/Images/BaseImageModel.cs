using System.ComponentModel.DataAnnotations;

namespace project.models.Images
{
    public class BaseImageModel
    {
        [StringLength(50)]
        public string Path { get; set; }
    }
}
