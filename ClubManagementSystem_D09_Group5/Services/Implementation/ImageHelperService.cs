using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class ImageHelperService : IImageHelperService
    {
        public string ConvertToBase64(byte[]? imageData, string? fileExtension)
        {
            if (imageData == null || imageData.Length == 0)
                return string.Empty;

            if (fileExtension == null)
                fileExtension = "png";

            return $"data:image/{fileExtension.TrimStart('.')};base64,{Convert.ToBase64String(imageData)}";
        }
    }

}
