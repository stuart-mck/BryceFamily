using ImageSharp;
using ImageSharp.Processing;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BryceFamily.Web.MVC.Infrastructure
{
    public static class FileResizer
    {
        public static byte[] GetFileResized(byte[] fileData, double limit)
        {

            var image = new Image(fileData);
            var ratioX = (double)limit / image.Width;
            var ratioY = (double)limit / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            byte[] resizedFileData;

            using (var ms = new MemoryStream())
            {
                var resized = image.Resize(new ResizeOptions
                {
                    Size = new Size(newWidth, newHeight),
                    Mode = ResizeMode.Max
                });

                resized.Save(ms);
                resizedFileData = ms.ToArray();
            }
            return resizedFileData;

        }
    }
}
