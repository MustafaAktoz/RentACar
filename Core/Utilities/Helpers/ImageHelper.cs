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

        static string _folder = "\\images\\";
        static string _folderName = _folder.Replace('\\', '/');
        static string _path = Directory.GetCurrentDirectory() + "\\wwwroot";
        public static string DefaultImagePath = _folderName + "default.png";

        public static string Upload(IFormFile file)
        {
            CheckImage(Path.GetExtension(file.FileName));

            string imagePath = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            if (!Directory.Exists(_path + _folder)) Directory.CreateDirectory(_path + _folder);

            using (FileStream fileStream = File.Create(_path + _folder + imagePath))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
                return _folderName + imagePath;
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
            File.Delete(_path + imagePath);
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
