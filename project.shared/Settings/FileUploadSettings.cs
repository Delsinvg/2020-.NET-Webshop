using System;
using System.Collections.Generic;
using System.Text;

namespace project.shared.Settings
{
    public class FileUploadSettings
    {
        public string FileSizeLimit { get; set; }
        public string[] FileExtensions { get; set; }
        public string[] FileContentTypes { get; set; }
    }
}
