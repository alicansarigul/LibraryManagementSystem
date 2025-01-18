using System.ComponentModel.DataAnnotations;

namespace LibraryManagementSystem.Models
{
    /// <summary>
    /// Bu bizim ödünç alma işlemlerini takip ettiğimiz model
    /// Hangi öğrenci hangi kitabı ne zaman almış, iade etmiş mi etmemiş mi hepsini buradan takip ediyoruz
    /// </summary>
    public class BookLoan
    {
        /// <summary>
        /// Her ödünç alma kaydı için benzersiz numara
        /// Otomatik artıyor, primary key olarak kullanıyoruz
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Ödünç alınan kitabın Id'si
        /// Books tablosuyla bağlantı kurmak için kullanıyoruz
        /// </summary>
        public int BookId { get; set; }

        /// <summary>
        /// Kitabı alan öğrencinin Id'si
        /// Students tablosuyla bağlantı kurmak için kullanıyoruz
        /// </summary>
        public int StudentId { get; set; }

        /// <summary>
        /// Kitabın ne zaman ödünç alındığı
        /// Otomatik olarak o anki tarihi alıyor
        /// </summary>
        [Display(Name = "Ödünç Alma Tarihi")]
        public DateTime LoanDate { get; set; }

        /// <summary>
        /// Kitabın ne zaman iade edilmesi gerektiği
        /// Genelde ödünç alma tarihinden 14 gün sonrası
        /// </summary>
        [Display(Name = "Son İade Tarihi")]
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Kitap iade edildiyse iade tarihi
        /// Eğer null ise kitap hala öğrencide demektir
        /// </summary>
        [Display(Name = "İade Edildiği Tarih")]
        public DateTime? ReturnDate { get; set; }

        /// <summary>
        /// Ödünç alınan kitabın bilgileri
        /// Book modeliyle ilişki kuruyoruz
        /// </summary>
        public virtual Book Book { get; set; }

        /// <summary>
        /// Kitabı alan öğrencinin bilgileri
        /// Student modeliyle ilişki kuruyoruz
        /// </summary>
        public virtual Student Student { get; set; }
    }
}