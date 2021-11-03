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
        static string _path = Directory.GetCurrentDirectory() + "\\wwwroot\\"+"images\\";
        public static string DefaultImagePath = "default.png";
        public static IResult Upload(string imagePath,IFormFile file)
        {
            var result = BusinessRules.Run(CheckImage(Path.GetExtension(file.FileName)));
            if (result != null) return result;

            if (!Directory.Exists(_path))
            {
                Directory.CreateDirectory(_path);
            }
            using (FileStream fileStream = File.Create(_path + imagePath))
            {
                file.CopyTo(fileStream);
                fileStream.Flush();
                return new SuccessResult();
            }
        }

        public static IResult Update(string imagePath,IFormFile file)
        {
            Upload(imagePath,file);
            return new SuccessResult();
        }

        public static IResult Delete(string imagePath)
        {
            var result=BusinessRules.Run(DefaultImageCannotBeDeleted(imagePath));
            if (result != null) return result;
            
            File.Delete(_path+imagePath);
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

        private static IResult DefaultImageCannotBeDeleted(string imagePath)
        {
            if (imagePath == DefaultImagePath) return new ErrorResult(CoreMessages.DefaultImageCannotBeDeleted);

            return new SuccessResult();
        }
    }
}
