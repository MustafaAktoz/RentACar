using Core.Entities.Concrete;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Business.Constants
{
    public class Messages
    {
        public static string Added = "Başarıyla eklendi";
        public static string Deleted = "Başarıyla silindi";
        public static string Updated = "Başarıyla güncellendi";
        public static string Listed = "Başarıyla listelendi";
        public static string Filtered = "Başarıyla filtrelendi";
        public static string Geted="Başarıyla getirildi";
        public static string GetDetails = "Detaylar getirildi";
        public static string CanMustBeDelivered = "Araba henüz teslim edilmedi";
        public static string TokenCreated="Token başarıyla oluşturuldu";
        public static string RegistrationSuccessful = "Kayıt başarılı";
        public static string UserNotFound = "Kullanıcı bulunamadı";
        public static string PasswordIsWrong = "Şifre hatalı";
        public static string LoginSuccessful = "Giriş başarılı";
        public static string UserAlreadyExist = "Kullanıcı zaten var";
        public static string MaximumImageLimitExceeded = "Bu araba için daha fazla resim eklenemez";
        public static string NoImagesFoundForThisCar = "Bu arabaya ait hiç resim bulunamadı";
        public static string NoImagesFoundForThisId="Bu idye ait hiç resim bulunamadı";
        public static string CarNotFound = "Bu idye ait bir araba bulunamadı";
    }
}
