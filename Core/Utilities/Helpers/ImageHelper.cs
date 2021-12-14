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
        static string _basePath = Directory.GetCurrentDirectory() + "/wwwroot/";
        static string _folder = "images/";
        public static string DefaultImagePath = _folder + "default.png";

        public static string Upload(IFormFile file)
        {
            if (!Directory.Exists(_basePath + _folder)) Directory.CreateDirectory(_basePath + _folder);

            string extension = Path.GetExtension(file.FileName);
            CheckImage(extension);

            string imagePath = _folder + Guid.NewGuid().ToString() + extension;

            using (FileStream fileStream = File.Create(_basePath + imagePath))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
                return imagePath;
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
            File.Delete(_basePath + imagePath);
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
