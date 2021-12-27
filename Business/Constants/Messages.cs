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
        public static string DetailsGeted = "Detaylar getirildi";
        public static string CanMustBeDelivered = "Araba henüz teslim edilmedi";
        public static string TokenCreated="Token başarıyla oluşturuldu";
        public static string RegistrationSuccessful = "Kayıt başarılı";
        public static string UserNotFound = "Kullanıcı bulunamadı";
        public static string PasswordIsWrong = "Şifre hatalı";
        public static string OldPasswordIsWrong ="Eski şifre hatalı";
        public static string LoginSuccessful = "Giriş başarılı";
        public static string UserAlreadyExist = "Kullanıcı zaten var";
        public static string MaximumImageLimitExceeded = "Bu araba için daha fazla resim eklenemez";
        public static string NoImagesFoundForThisCar = "Bu arabaya ait hiç resim bulunamadı";
        public static string ImageNotFound="Resim bulunamadı";
        public static string CarNotFound = "Araba bulunamadı";
        public static string LowPriceForThisBrand = "Bu marka için düşük bir fiyat girdiniz";
        public static string CarNameAlreadyExist = "Bu isimde bir araba zaten var";
        public static string DateRangeError="Bu tarihler arasında araba zaten kiralanmış";
        public static string RentalDateCannotBeBeforeThanToday = "Zaman makinen yoksa geçmişe araba kiralayamayız :)";
        public static string DeliveryDateCannotBeBeforeThanRentalDate = "Teslim tarihi kiralama tarihinden önce olamaz";
        public static string PaymentSuccessful="Ödeme başarılı";
        public static string RentalSuccessful="Kiralama başarılı";
        public static string PasswordChangedSuccessfully="Şifre başarıyla değiştirildi";
        public static string NotEnoughFindeksPoints="Findeks puanı yetersiz";
        public static string YouAlreadyExistASavedCardWithThisCardNumber="Bu kart numarasıyla eşleşen kayıtlı bir kartınız zaten mevcut";
        public static string NoMoreThanTwoDigitsCanBeEnteredForTheYear = "Yıl için son 2 haneden fazlası girilemez";
        public static string CvvMustBeThreeCharacters="CVV 3 karakterden oluşmalı";
        public static string CardSaved="Kart başarıyla kaydedildi";
    }
}