using System;

namespace project.models.Images
{
    public class GetImageModel : BaseImageModel
    {
        public Guid Id { get; set; }

        public string Product { get; set; }
    }
}
