namespace Cic.OpenOne.GateBANKNOW.Common.DTO
{
    using System;
    using System.Collections.Generic;

    public class DMRInputDto
    {
        public DMRInputDto()
        {
            Fields = new List<DMRInputFieldDto>();
            Media = new List<DMRInputMediaDto>();
        }

        public string ClientName { get; set; }

        public string ProcessName { get; set; }

        public string ProcessStepName { get; set; }

        public string DocumentType { get; set; }

        public List<DMRInputFieldDto> Fields { get; set; }

        public List<DMRInputMediaDto> Media { get; set; }

        public void AddToField(string name, string value)
        {
            Fields.Add(new DMRInputFieldDto()
            {
                Name = name,
                Value = value
            });
        }

        public void AddToMedia(string fileName, string mimeType, byte[] value)
        {
            Media.Add(new DMRInputMediaDto()
            {
                FileName = fileName,
                MimeType = mimeType,
                Base64Value = Convert.ToBase64String(value)
            });
        }
    }
}