using Core.Exceptions;
using Core.Utilities.Business;
using Core.Utilities.Messages;
using Core.Utilities.Result;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Core.Utilities.Helpers
{
    public class ImageHelper
    {
        static string _simplePath = Directory.GetCurrentDirectory() + "\\wwwroot";
        static string _folder = "\\images\\";
        static string _folderName = _folder.Replace('\\', '/');
        static string _folderPath = _simplePath + _folder;
        public static string DefaultImagePath = _folderName + "default.png";

        public static string Upload(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName);
            CheckImage(extension);

            string imageName = Guid.NewGuid().ToString() + extension;

            if (!Directory.Exists(_folderPath)) Directory.CreateDirectory(_folderPath);

            using (FileStream fileStream = File.Create(_folderPath + imageName))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
                return _folderName + imageName;
            }
        }

        public static string Update(string oldImagePath, IFormFile file)
        {
            Delete(oldImagePath);
            return Upload(file);
        }

        public static void Delete(string imagePath)
        {
            DefaultImageCannotBeDeleted(imagePath);
            File.Delete(_simplePath + imagePath);
        }

        private static void CheckImage(string extension)
        {
            var extensions = new List<string> { ".jpg", ".png", "jpeg" };

            if (!extensions.Contains(extension))
                throw new ImageHelperException(CoreMessages.ThisIsNotImage);
        }

        private static void DefaultImageCannotBeDeleted(string imagePath)
        {
            if (imagePath == DefaultImagePath)
                throw new ImageHelperException(CoreMessages.DefaultImageCannotBeDeleted);
        }
    }
}
