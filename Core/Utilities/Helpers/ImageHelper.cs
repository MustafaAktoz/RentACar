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
        static string path = Directory.GetCurrentDirectory() + "\\wwwroot\\"+"images\\";
        public static string DefaultImagePath = "default.png";
        public static IResult Upload(string imagePath,IFormFile file)
        {
            var result = BusinessRules.Run(CheckImage(Path.GetExtension(file.FileName)));
            if (result != null) return result;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (FileStream fileStream = File.Create(path + imagePath))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
                return new SuccessResult();
            }
        }

        public static IResult Update(string imagePath,IFormFile file)
        {
            Delete(imagePath);
            Upload(imagePath,file);
            return new SuccessResult();
        }

        public static IResult Delete(string imagePath)
        {
            if (imagePath == DefaultImagePath) return new ErrorResult(CoreMessages.DefaultImageCannotBeDeleted);
                
                
            File.Delete(path+imagePath);
            return new SuccessResult();
        }

        private static IResult CheckImage(string extension)
        {
            if(extension==".jpg"||extension==".png"||extension==".jpeg")
            {
                return new SuccessResult();
            }

            return new ErrorResult(CoreMessages.ThisIsNotImage);
        }
    }
}
