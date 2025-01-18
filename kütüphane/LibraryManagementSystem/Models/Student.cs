using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    /// <summary>
    /// Bu bizim öğrenci modelimiz
    /// Kütüphaneden kitap alacak öğrencilerin bilgilerini tutuyoruz burada
    /// </summary>
    public class Student
    {
        /// <summary>
        /// Öğrenci oluşturulduğunda boş bir kitap listesi oluşturuyoruz
        /// Böylece null reference hatası almayız
        /// </summary>
        public Student()
        {
            BookLoans = new List<BookLoan>();
        }

        /// <summary>
        /// Her öğrenciye otomatik verilen benzersiz numara
        /// Veritabanında primary key olarak kullanılıyor
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Öğrencinin okul numarası
        /// Boş geçilemez, zorunlu alan
        /// </summary>
        [Required(ErrorMessage = "Öğrenci numarası boş bırakılamaz")]
        [Display(Name = "Öğrenci Numarası")]
        public string StudentNumber { get; set; }

        /// <summary>
        /// Öğrencinin adı
        /// Boş geçilemez, zorunlu alan
        /// </summary>
        [Required(ErrorMessage = "Öğrencinin adını girmelisiniz")]
        [Display(Name = "Ad")]
        public string FirstName { get; set; }

        /// <summary>
        /// Öğrencinin soyadı
        /// Boş geçilemez, zorunlu alan
        /// </summary>
        [Required(ErrorMessage = "Öğrencinin soyadını girmelisiniz")]
        [Display(Name = "Soyad")]
        public string LastName { get; set; }

        /// <summary>
        /// Öğrencinin adı ve soyadını birleştirip veren özellik
        /// Örnek: "Ahmet Yılmaz"
        /// </summary>
        [Display(Name = "Ad Soyad")]
        public string FullName => $"{FirstName} {LastName}";

        /// <summary>
        /// Öğrencinin sınıfı
        /// Boş geçilemez, zorunlu alan
        /// </summary>
        [Required(ErrorMessage = "Sınıf bilgisi girmelisiniz")]
        [Display(Name = "Sınıf")]
        public string Class { get; set; }

        /// <summary>
        /// Öğrencinin telefon numarası
        /// İsteğe bağlı alan, boş geçilebilir
        /// </summary>
        [Display(Name = "Telefon")]
        public string Phone { get; set; }

        /// <summary>
        /// Öğrencinin e-posta adresi
        /// İsteğe bağlı alan, ama girilirse geçerli bir e-posta olmalı
        /// </summary>
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi yazmalısınız")]
        [Display(Name = "E-posta")]
        public string Email { get; set; }

        /// <summary>
        /// Öğrencinin aldığı tüm kitapların listesi
        /// BookLoan tablosuyla ilişkili
        /// Hem aktif hem de iade edilmiş kitapları içerir
        /// </summary>
        public virtual ICollection<BookLoan> BookLoans { get; set; }
    }
}