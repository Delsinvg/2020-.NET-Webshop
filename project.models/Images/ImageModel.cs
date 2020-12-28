using System;
using System.Collections.Generic;
using System.Text;

namespace project.models.Images
{
    public class ImageModel
    {
        public string Name { get; set; }
        public string FileType { get; set; }
        public string Extension { get; set; }
        public string Description { get; set; }
        public byte[] Data { get; set; }
    }
}
